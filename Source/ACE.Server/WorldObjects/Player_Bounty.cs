using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Common;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Entity.Actions;
using ACE.Server.Entity.Bounties;
using ACE.Server.Entity.PKQuests;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using log4net;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        private bool BountiesInitialized = false;

        private double? LastBountyHunterSeenTimestamp;

        private BountyCollection BountyCollection;

        private bool IsValidBountyTarget
        {
            get
            {
                var validLocation = (CurrentLandblock?.IsBountyLocation ?? false);
                var validLogout = !IsLoggingOut;

                if (PropertyManager.GetBool("bounty_allow_all_locations").Item)
                    validLocation = true;
                if (PropertyManager.GetBool("bounty_allow_logged_out").Item)
                    validLogout = true;

                var minimumPlayerLevel = PropertyManager.GetLong("bounty_minimum_player_level").Item;

                return
                    !IsAdmin &&
                    validLogout &&
                    validLocation &&
                    Level >= minimumPlayerLevel &&
                    this.IsAllegianceWhitelisted() &&
                    IsPK;
            }
        }

        private void InitializeBounties()
        {
            if (!BountyContract.IsBountySystemEnabled)
                return;

            var bountyContracts = GetInventoryItemsOfWCID(BountyContract.BountyContractWcid)
                .OfType<BountyContract>()
                .ToList();

            foreach (var contract in bountyContracts)
            {
                contract.UpdateUiEffects(this);
            }

            BountyCollection = new BountyCollection(this, bountyContracts);

            BountiesInitialized = true;
        }

        private void BountyTick(double currentUnixTime)
        {
            if (!BountiesInitialized || BountyCollection == null)
                return;
            BountyCollection.CheckContracts(this);
        }

        // BountyCollection Queries and Mutations
        private bool TryGetBountyContract(uint targetGuid, out BountyContract contract)
        {
            contract = null;
            if (!BountiesInitialized || BountyCollection == null)
                return false;
            return BountyCollection.TryGetBountyContract(targetGuid, out contract);
        }

        private bool TryGetActiveBountyContract(uint targetGuid, out BountyContract contract)
        {
            contract = null;
            if (!BountiesInitialized || BountyCollection == null)
                return false;
            return BountyCollection.TryGetActiveBountyContract(targetGuid, this, out contract);
        }

        private bool TryAddCompletedBountyContract(BountyContract contract)
        {
            if (!BountiesInitialized || BountyCollection == null)
                return false;
            if (!contract.BountyTargetGuid.HasValue)
                return false;
            return BountyCollection.CompleteContract((uint)contract.BountyTargetGuid.Value, this);
        }

        private bool TryAddActiveBountyContract(BountyContract contract)
        {
            if (!BountiesInitialized || BountyCollection == null)
                return false;
            if (!contract.BountyTargetGuid.HasValue)
                return false;
            return BountyCollection.AddBountyContract((uint)contract.BountyTargetGuid.Value, contract);
        }

        public bool TryRemoveBountyContract(uint targetGuid)
        {
            if (!BountiesInitialized || BountyCollection == null)
                return false;
            return BountyCollection.RemoveBountyContract(targetGuid);
        }

        // Bounty Transactions with NPC
        enum BountyTransactionResult
        {
            Consume,   // destroy item
            Return,    // give back to player
            Keep,      // leave item with NPC (rare, but future-proof)
            Ignore     // not handled
        }

        private void HandleTransactionResult(BountyTransactionResult result, WorldObject npc, WorldObject itemToGive)
        {
            switch (result)
            {
                case BountyTransactionResult.Return:
                    TryCreateForGive(npc, itemToGive);
                    break;

                case BountyTransactionResult.Consume:
                    itemToGive.Destroy();
                    break;

                case BountyTransactionResult.Keep:
                    // do nothing (NPC keeps item)
                    break;

                case BountyTransactionResult.Ignore:
                default:
                    itemToGive.Destroy();
                    break;
            }
        }

        private BountyTransactionResult CheckBountyTransactions(WorldObject npc, WorldObject itemToGive)
        {
            if (npc.WeenieClassId != BountyContract.BountyNPCWcid)
                return BountyTransactionResult.Ignore;

            var result = CheckBountyContractTurnIns(npc, itemToGive);
            if (result != BountyTransactionResult.Ignore)
                return result;

            result = CheckBountyPurchase(npc, itemToGive);
            if (result != BountyTransactionResult.Ignore)
                return result;

            result = CheckWritOfPursuit(npc, itemToGive);
            if (result != BountyTransactionResult.Ignore)
                return result;

            return BountyTransactionResult.Ignore;
        }

        /// <summary>
        /// Called when the player tries to turn in a bounty contract to the bounty NPC.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private BountyTransactionResult CheckBountyContractTurnIns(WorldObject npc, WorldObject item)
        {
            if (npc.WeenieClassId != BountyContract.BountyNPCWcid || item is not BountyContract contract)
                return BountyTransactionResult.Ignore;

            try
            {
                if (!BountyContract.IsBountySystemEnabled)
                {
                    SendDelayedNpcResponse(npc, "Bounties are not enabled on this server.");
                    return BountyTransactionResult.Return;
                }

                if (!BountiesInitialized)
                {
                    SendDelayedNpcResponse(npc, "Bounties have not finished initializing yet, please try again in a few seconds.");
                    return BountyTransactionResult.Return;
                }

                if (!contract.BountyTargetGuid.HasValue)
                {
                    SendDelayedNpcResponse(npc, "This contract is invalid and cannot be turned in. Destroying the contract now.");
                    return BountyTransactionResult.Consume;
                }

                var targetGuid = (uint)contract.BountyTargetGuid.Value;

                if (contract.IsBountyExpired)
                {
                    var bountyCurrencyWeenie = BountyContract.BountyCurrencyWeenie;
                    var bountyCurrencyReturnAmount = BountyContract.BountyCurrencyReturnAmount;

                    TryRemoveBountyContract(targetGuid);
                    SaveBountyExpiration(targetGuid);

                    SendDelayedNpcResponse(npc,
                        $"Your bounty contract has expired, you will be compensated {bountyCurrencyWeenie.BuildAmountString(bountyCurrencyReturnAmount)} for your time.",
                        () => GiveFromEmote(npc, BountyContract.BountyCurrencyWcid, amount: (int)bountyCurrencyReturnAmount));

                    return BountyTransactionResult.Consume;
                }

                if (contract.State != BountyContract.BountyState.Completed)
                {
                    SendDelayedNpcResponse(npc, "Your bounty contract is not completed yet. Keep hunting!");
                    return BountyTransactionResult.Return;
                }

                var bountyTarget = PlayerManager.FindByGuid(new ObjectGuid(targetGuid));
                if (bountyTarget == null || bountyTarget.IsPendingDeletion || bountyTarget.IsDeleted)
                {
                    SendDelayedNpcResponse(npc, "The bounty target for this contract no longer exists.");
                    return BountyTransactionResult.Return;
                }

                var result = UpdateCompletedBountyInformation(bountyTarget, contract);
                HandleBountyQuests(result);
                SaveBountyInformation();

                var rewardAmount = bountyTarget.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.BountyPriorityTargetRewardAmount);
                var rewardCurrencyWcid = bountyTarget.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.BountyPriorityCurrency);

                bountyTarget.ClearHighPriorityBountyProps();

                TryRemoveBountyContract(targetGuid);
                BountyEndTimestamp = Time.GetUnixTime();

                SendDelayedNpcResponse(npc,
                    $"You have successfully turned in your bounty for player \"{bountyTarget.Name}\".");

                if (result.IsHighPriorityTarget && rewardAmount.HasValue && rewardCurrencyWcid.HasValue)
                {
                    var wopCurrencyWeenie = DatabaseManager.World.GetOrThrowCachedWeenie((uint)rewardCurrencyWcid.Value);
                    var rewardedAmountString = wopCurrencyWeenie.BuildAmountString(rewardAmount.Value);

                    SendDelayedNpcResponse(npc,
                        $"This bounty was a high priority target, you have been rewarded {rewardedAmountString}.",
                        () => GiveFromEmote(npc, (uint)rewardCurrencyWcid.Value, amount: rewardAmount.Value));

                    PlayerManager.BroadcastToAll(new GameMessageSystemChat(
                        $"{Name} has completed a bounty contract for high priority target {bountyTarget.Name} and received {rewardedAmountString}!",
                        ChatMessageType.WorldBroadcast));

                    var online = PlayerManager.GetAllOnline();

                    foreach (var p in online)
                    {
                        if (p.TryGetActiveBountyContract(targetGuid, out var activeContract))
                        {
                            activeContract.SetUiEffect(
                                ACE.Entity.Enum.UiEffects.Fire,
                                p,
                                message: $"{bountyTarget.Name} is no longer a high priority target, your contract has returned to normal.");
                        }
                    }
                }

                return BountyTransactionResult.Consume;
            }
            catch (Exception ex)
            {
                log.Error($"Error in CheckBountyContractTurnIns for player {Name} (Guid: {Guid.Full}): ", ex);
                SendDelayedMessage("An error occurred while processing the bounty turn-in. Please notify an admin.", ChatMessageType.System);
                return BountyTransactionResult.Consume;
            }
        }

        /// <summary>
        /// Called when the player tries to give a Writ of Pursuit to the bounty NPC.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private BountyTransactionResult CheckWritOfPursuit(WorldObject npc, WorldObject item)
        {
            if (npc.WeenieClassId != BountyContract.BountyNPCWcid ||
                item.WeenieClassId != BountyContract.WritOfPursuitWcid)
                return BountyTransactionResult.Ignore;

            try
            {
                if (!BountyContract.IsBountySystemEnabled)
                {
                    SendDelayedNpcResponse(npc, "Bounties are not enabled on this server.");
                    return BountyTransactionResult.Return;
                }

                if (!BountiesInitialized)
                {
                    SendDelayedNpcResponse(npc, "Bounties have not finished initializing yet, please try again in a few seconds.");
                    return BountyTransactionResult.Return;
                }

                if (!BountyContract.IsWritOfPursuitEnabled)
                {
                    SendDelayedNpcResponse(npc, "Writs of Pursuit are not enabled on this server.");
                    return BountyTransactionResult.Return;
                }

                if (!this.IsAllegianceWhitelisted())
                {
                    SendDelayedNpcResponse(npc, "You must be in a whitelisted allegiance to purchase a bounty.");
                    return BountyTransactionResult.Return;
                }

                var wopCurrencyWeenie = BountyContract.BountyWopCurrencyWeenie;

                if (!TryParseBountyWrit(item.Inscription, out var targetName, out var rewardAmount))
                {
                    SendDelayedNpcResponse(npc,
                        "Unable to parse your Writ of Refuge, please inscribe it with the player's name and reward amount in this format <Name>:<Amount>.");
                    SendDelayedNpcResponse(npc,
                        $"The reward amount must be between 1-{wopCurrencyWeenie.GetMaxStackSize()}.");
                    return BountyTransactionResult.Return;
                }

                var player = PlayerManager.FindByName(targetName);

                if (player == null)
                {
                    SendDelayedNpcResponse(npc,
                        $"You Writ of Pursuit does not contain a valid player name, please inscribe your Writ of Pursuit with a valid name.");
                    return BountyTransactionResult.Return;
                }

                if (!player.IsAllegianceWhitelisted())
                {
                    SendDelayedNpcResponse(npc,
                        $"The player \"{targetName}\" is not in a whitelisted allegiance and cannot be targeted with a Writ of Pursuit.");
                    return BountyTransactionResult.Return;
                }

                if (player.Guid.Full == Guid.Full)
                {
                    SendDelayedNpcResponse(npc,
                        $"You cannot target yourself with a Writ of Pursuit.");
                    return BountyTransactionResult.Return;
                }

                var minimumPlayerLevel = PropertyManager.GetLong("bounty_minimum_player_level").Item;

                if (player.Level <= minimumPlayerLevel)
                {
                    SendDelayedNpcResponse(npc,
                        $"The player \"{targetName}\" does not meet the minimum level requirement of {minimumPlayerLevel} to be targeted with a Writ of Pursuit.");
                    return BountyTransactionResult.Return;
                }

                if (this.IsSameAllegiance(player))
                {
                    SendDelayedNpcResponse(npc,
                        $"You cannot target a player of the same allegiance with a Writ of Pursuit.");
                    return BountyTransactionResult.Return;
                }

                if (player.GetProperty(ACE.Entity.Enum.Properties.PropertyBool.IsBountyHighPriorityTarget) == true)
                {
                    var ownerName = player.GetProperty(ACE.Entity.Enum.Properties.PropertyString.BountyPriorityOwnerName);
                    SendDelayedNpcResponse(npc,
                        $"The player \"{targetName}\" is already a high priority target for {ownerName}, you must wait until someone completes a bounty for them.");
                    return BountyTransactionResult.Return;
                }

                var wopRewardsInventoryCount = GetNumInventoryItemsOfWCID(BountyContract.BountyWopCurrencyWcid);

                if (wopRewardsInventoryCount < rewardAmount)
                {
                    SendDelayedNpcResponse(npc,
                        $"You do not have enough {wopCurrencyWeenie.GetPluralName()} to turn in your Writ of Pursuit. You have {wopRewardsInventoryCount} but your Writ requires {rewardAmount}.");
                    return BountyTransactionResult.Return;
                }

                if (!TryConsumeFromInventoryWithNetworking(BountyContract.BountyWopCurrencyWcid, rewardAmount))
                {
                    return BountyTransactionResult.Return;
                }

                // Apply high priority target
                player.SetProperty(ACE.Entity.Enum.Properties.PropertyBool.IsBountyHighPriorityTarget, true);
                player.SetProperty(ACE.Entity.Enum.Properties.PropertyInt.BountyPriorityTargetRewardAmount, rewardAmount);
                player.SetProperty(ACE.Entity.Enum.Properties.PropertyInt.BountyPriorityCurrency, (int)BountyContract.BountyWopCurrencyWcid);
                player.SetProperty(ACE.Entity.Enum.Properties.PropertyString.BountyPriorityOwnerName, Name);

                SendDelayedNpcResponse(npc,
                    $"You have successfully turned in your Writ of Pursuit for player \"{player.Name}\" with a reward amount of {wopCurrencyWeenie.BuildAmountString(rewardAmount)}.");

                // Update UI
                var targetGuid = player.Guid.Full;

                foreach (var p in PlayerManager.GetAllOnline())
                {
                    if (p.TryGetActiveBountyContract(targetGuid, out var activeContract))
                    {
                        activeContract.SetUiEffect(
                            ACE.Entity.Enum.UiEffects.Magical,
                            p,
                            message: $"{targetName} is now a high priority bounty target, your contract has been upgraded.");
                    }
                }

                return BountyTransactionResult.Consume;
            }
            catch (Exception ex)
            {
                log.Error($"Error in CheckWritOfPursuit for player {Name} (Guid: {Guid.Full}): ", ex);
                SendDelayedMessage("An error occurred while trying to turn in a Writ of Pursuit. Please notify an admin.", ChatMessageType.System);
                return BountyTransactionResult.Consume;
            }
        }

        private bool TryParseBountyWrit(string inscription, out string playerName, out int reward)
        {
            playerName = null;
            reward = 0;
            var wopCurrencyWeenie = BountyContract.BountyWopCurrencyWeenie;

            if (string.IsNullOrWhiteSpace(inscription))
                return false;

            var parts = inscription.Split(':');

            if (parts.Length != 2)
                return false;

            playerName = parts[0].Trim();

            if (string.IsNullOrWhiteSpace(playerName))
                return false;

            if (!int.TryParse(parts[1].Trim(), out reward))
                return false;

            if (reward <= 0)
                return false;

            if (reward > wopCurrencyWeenie.GetMaxStackSize()) // cap it
                return false;

            if (playerName.Length > 32)
                return false;

            return true;
        }

        /// <summary>
        /// Called when the player tries to purchase a bounty contract from the bounty NPC.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private BountyTransactionResult CheckBountyPurchase(WorldObject npc, WorldObject item)
        {
            if (npc.WeenieClassId != BountyContract.BountyNPCWcid ||
                item.WeenieClassId != BountyContract.BountyPurchaseTokenWcid)
                return BountyTransactionResult.Ignore;

            try
            {
                if (!BountyContract.IsBountySystemEnabled)
                {
                    SendDelayedNpcResponse(npc, "Bounties are not enabled on this server.");
                    return BountyTransactionResult.Return;
                }

                if (!BountiesInitialized)
                {
                    SendDelayedNpcResponse(npc, "Bounties have not finished initializing yet, please try again in a few seconds.");
                    return BountyTransactionResult.Return;
                }

                if (!this.IsAllegianceWhitelisted())
                {
                    SendDelayedNpcResponse(npc, "You must be in a whitelisted allegiance to purchase a bounty.");
                    return BountyTransactionResult.Return;
                }

                if (BountyCollection.GetTotalCount() >= BountyContract.MaxBountyContracts)
                {
                    SendDelayedNpcResponse(npc,
                        $"You have reached the maximum number of active bounty contracts ({BountyContract.MaxBountyContracts}). " +
                        "Please complete your current bounties before purchasing new ones.");
                    return BountyTransactionResult.Return;
                }

                var eligibleTargets = PlayerManager.GetAllOnline().Where(p =>
                    p != null &&
                    p.Guid.Full != Guid.Full &&
                    p.IsValidBountyTarget &&
                    !TryGetBountyContract(p.Guid.Full, out _) &&
                    !this.IsSameAllegiance(p) &&
                    IsDifferentIPAddress(p) &&
                    !IsOnHardCooldown(p)).ToList();

                if (eligibleTargets.Count == 0)
                {
                    SendDelayedNpcResponse(npc, "There are currently no bounty players available at this time.");
                    return BountyTransactionResult.Return;
                }

                var bountyTarget = SelectRandomWeightedTarget(eligibleTargets);
                var contract = WorldObjectFactory.CreateNewWorldObject(BountyContract.BountyContractWcid) as BountyContract;

                if (contract == null)
                    return BountyTransactionResult.Consume;

                var durationMinutes = PropertyManager.GetLong("bounty_expiration_time").Item;
                var now = Time.GetUnixTime();

                contract.BountyOwnerGuid = (int?)Guid.Full;
                contract.BountyOwnerName = Name;
                contract.BountyTargetGuid = (int?)bountyTarget.Guid.Full;
                contract.BountyTargetName = bountyTarget.Name;
                contract.BountyCreationTimestamp = now;
                contract.BountyContractState = (int)BountyContract.BountyState.Pending;
                contract.Name = $"Bounty Contract: {bountyTarget.Name}";

                if (bountyTarget.IsHighPriorityTarget())
                {
                    contract.SetUiEffect(
                        ACE.Entity.Enum.UiEffects.Magical,
                        this,
                        message: $"{bountyTarget.Name} is a high priority target, this contract has a higher reward than normal!");
                }

                if (!TryCreateForGive(npc, contract))
                {
                    contract.Destroy();
                    return BountyTransactionResult.Return;
                }

                contract.SetState(BountyContract.BountyState.Active, this);

                if (!TryAddActiveBountyContract(contract))
                {
                    TryRemoveFromInventoryWithNetworking(contract.Guid.Full, out _, Player.RemoveFromInventoryAction.ConsumeItem);
                    return BountyTransactionResult.Return;
                }

                SendDelayedNpcResponse(npc,
                    $"A new bounty for player \"{bountyTarget.Name}\" has been assigned to you. A bounty contract has been added to your inventory.");

                var killStreak = bountyTarget.PlayerKillStreak;
                var killStreakMinimum = PropertyManager.GetLong("bounty_kill_streak_minimum").Item;

                if (killStreak > killStreakMinimum)
                {
                    PlayerManager.BroadcastToAll(new GameMessageSystemChat(
                        $"{bountyTarget.Name} is currently on FIRE with a kill streak of {killStreak}! they have been targeted in a bounty contract!",
                        ChatMessageType.WorldBroadcast));
                }

                return BountyTransactionResult.Consume;
            }
            catch (Exception ex)
            {
                log.Error($"Error in CheckBountyPurchase for player {Name} (Guid: {Guid.Full}): ", ex);
                SendDelayedMessage("An error occurred while trying to purchase a bounty. Please notify an admin.", ChatMessageType.System);
                return BountyTransactionResult.Consume;
            }
        }

        private Player SelectRandomWeightedTarget(List<Player> players)
        {
            var weightedPlayers = new List<(Player player, double weight)>();
            double totalWeight = 0;

            foreach (var p in players)
            {
                var w = GetBountyWeight(p);
                totalWeight += w;
                weightedPlayers.Add((p, w));
            }

            if (totalWeight <= 0)
                return players[ThreadSafeRandom.Next(0, players.Count - 1)];

            double roll = ThreadSafeRandom.Next(0f, (float)totalWeight);

            double cumulative = 0;

            foreach (var entry in weightedPlayers)
            {
                cumulative += entry.weight;

                if (roll <= cumulative)
                    return entry.player;
            }

            return weightedPlayers.Last().player;
        }

        private double GetBountyWeight(Player p)
        {
            double weight = 1;

            if (p.IsBountyHighPriorityTarget)
            {
                weight *= 2; 
            }

            var bountyWeightExponent = PropertyManager.GetDouble("bounty_weight_exponent").Item;
            var bountyWeightMultiplier = PropertyManager.GetDouble("bounty_weight_multiplier").Item;
            var bountyWeightMaxStackScale = PropertyManager.GetDouble("bounty_weight_maxstack_scale").Item;

            bountyWeightMultiplier = Math.Max(0, bountyWeightMultiplier);
            bountyWeightMaxStackScale = Math.Clamp(bountyWeightMaxStackScale, 0.01, 1.0);
            bountyWeightExponent = Math.Clamp(bountyWeightExponent, 0.25, 1.0);

            if (p.BountyPriorityTargetRewardAmount.HasValue)
            {
                var wopCurrencyWeenie = BountyContract.BountyWopCurrencyWeenie;
                var rewardAmount = p.BountyPriorityTargetRewardAmount.Value;
                var maxStack = wopCurrencyWeenie.GetMaxStackSize();
                if (maxStack <= 0)
                    maxStack = 1;

                var effectiveMaxStack = Math.Max(1, maxStack * bountyWeightMaxStackScale);

                var normalized = rewardAmount / effectiveMaxStack;
                normalized = Math.Clamp(normalized, 0.0, 1.0);

                weight += Math.Pow(normalized, bountyWeightExponent) * bountyWeightMultiplier;
            }

            var killStreak = p.PlayerKillStreak;
            var killStreakMinimum = PropertyManager.GetLong("bounty_kill_streak_minimum").Item;

            if (killStreak > killStreakMinimum)
            {
                var normalized = (killStreak - killStreakMinimum) / (double)Math.Max(1, killStreakMinimum);
                normalized = Math.Clamp(normalized, 0.0, 1.0);

                weight += Math.Pow(normalized, bountyWeightExponent) * bountyWeightMultiplier;
            }

            return weight;
        }

        /// <summary>
        /// Called when a player is killed to check if they are a bounty target and mark the corresponding contract as completed if so.
        /// </summary>
        /// <param name="bountyTargetGuid"></param>
        /// <returns></returns>
        private void MarkBountyComplete(Player bountyTarget, double damageDealt, double maxHealth, double currentHealth)
        {
            var targetGuid = bountyTarget.Guid.Full;
            var killStreak = bountyTarget.PlayerKillStreak;

            if (!TryGetActiveBountyContract(targetGuid, out var contract))
                return;

            if (!contract.BountyTargetGuid.HasValue)
                return;

            if (contract.IsBountyExpired)
                return;

            if (contract.State != BountyContract.BountyState.Active)
                return;

            contract.SetState(BountyContract.BountyState.Completed, this);

            contract.BountyTargetDamageDealt = Math.Floor(damageDealt);
            contract.BountyOwnerRemainingHealthPercentage = GetPercentageRemaining(maxHealth, currentHealth, 1);
            contract.BountyOwnerDamageReceived = Math.Floor(maxHealth - currentHealth);
            contract.BountyKillStreakCount = killStreak;

            TryAddCompletedBountyContract(contract);
        }

        private double GetPercentageRemaining(double maxHp, double currentHp, int decimals = 1)
        {
            if (maxHp <= 0)
                return 0;

            var clampedCurrent = Math.Clamp(currentHp, 0f, maxHp);
            var percentage = (clampedCurrent / maxHp) * 100.0;

            return Math.Round(percentage, decimals);
        }

        private static void TryMarkHunterTarget(Player hunter, Player target)
        {
            if (!hunter.TryGetActiveBountyContract(target.Guid.Full, out _))
                return;

            target.RefreshBountyPkTimer();
        }

        private void CheckVisibleBounties()
        {
            var visiblePlayers = PhysicsObj.ObjMaint.GetVisibleObjectsValuesOfTypePlayer();

            foreach (var player in visiblePlayers)
            {
               TryMarkHunterTarget(player, this); // player hunts me
               TryMarkHunterTarget(this, player); // I hunt player
            }
        }

        public void RefreshBountyPkTimer()
        {
            LastBountyHunterSeenTimestamp = Time.GetUnixTime();
            UpdatePKTimer();
        }

        private void SendDelayedNpcResponse(WorldObject npc, string message, Action action = null)
        {
            SendDelayedMessage($"{npc.Name} tells you, \"{message}\"", ChatMessageType.Tell, action: action, delaySeconds: 0);
        }

        public void SendDelayedMessage(string message, ChatMessageType type = ChatMessageType.System, double delaySeconds = 0.5, Action action = null)
        {
            if (delaySeconds > 0)
            {
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(delaySeconds);
                actionChain.AddAction(this, () =>
                {
                    Session?.Network?.EnqueueSend(new GameMessageSystemChat(message, type));
                    action?.Invoke();
                });
                actionChain.EnqueueChain();
            } else
            {
                Session?.Network?.EnqueueSend(new GameMessageSystemChat(message, type));
                action?.Invoke();
            }
        }

        public void SendBountyMessage(string message, Action action = null, uint delay = 0)
        {
            SendDelayedMessage($"[BOUNTY] {message}", ChatMessageType.System, delay, action);
        }

        private bool HasCooldownPenaltyExpiredForHunter()
        {
            if (!BountyEndTimestamp.HasValue)
                return true;

            var bountyCooldownExpirationDuration = PropertyManager.GetLong("bounty_cooldown_expiration_time").Item;
            return DateTime.UtcNow - Time.GetDateTimeFromTimestamp(BountyEndTimestamp.Value) > TimeSpan.FromMinutes(bountyCooldownExpirationDuration);
        }

        private bool IsOnHardCooldown(Player p)
        {
            if (!HasCooldownPenaltyExpiredForHunter())
                return true;

            var cooldown = GetBountyCooldown(p.Guid.Full);

            if (cooldown == -1)
                return false;

            var durationMinutes = PropertyManager.GetLong("bounty_cooldown_target_expiration_time").Item;

            var elapsed = DateTime.UtcNow - Time.GetDateTimeFromTimestamp(cooldown);

            return elapsed.TotalMinutes < durationMinutes;
        }

        private void HandleBountyQuests(BountyCompletionResult result)
        {
            // any bounty
            CompletePkQuestTasks(PKQuests.PKQuests_BountyAny);

            // unique targets
            if (result.IsNewUniqueTarget) CompletePkQuestTasks(PKQuests.PKQuests_BountyUnique);

            // repeat kills (same player)
            if (result.RepeatCount == 3) CompletePkQuestTask("BOUNTY_REPEAT_3", 1);
            if (result.RepeatCount == 5) CompletePkQuestTask("BOUNTY_REPEAT_5", 1);

            // speed-based
            if (result.CountLast30Min >= 2) CompletePkQuestTask("BOUNTY_FAST_2", 1);
            if (result.CountLast30Min >= 3) CompletePkQuestTask("BOUNTY_FAST_3", 1);
            if (result.CountLast60Min >= 8) CompletePkQuestTask("BOUNTY_FAST_8", 1);
            if (result.CountLast90Min >= 12) CompletePkQuestTask("BOUNTY_FAST_12", 1);

            // high priority target
            if (result.IsHighPriorityTarget) CompletePkQuestTasks(PKQuests.PKQuests_BountyPriority);

            // kill streaks
            if (result.IsKillStreakCompletion)
            {
                CompletePkQuestTasks(PKQuests.PKQuests_BountyKillStreak);

                if (result.KillStreak >= 5) CompletePkQuestTask("BOUNTY_STREAKBREAKER_5", 1);
                if (result.KillStreak >= 10) CompletePkQuestTask("BOUNTY_STREAKBREAKER_10", 1);
            }

            /*
            // damage dealt
            if (result.DamageDealt > 0)
                CompletePkQuestTasks(PKQuests.PKQuests_BountyDamageTotal, (int)result.DamageDealt);

            // single contract damage dealt
            if (result.DamageDealt >= 2000)
                CompletePkQuestTask("BOUNTY_DAMAGE_SINGLE_2K", 1);

            // dealt more than twice the damage received
            if (result.DamageReceived > 0 && result.DamageDealt >= result.DamageReceived * 2)
                CompletePkQuestTask("BOUNTY_EFFICIENT", 1);

            // damage taken 
            if (result.DamageReceived > 0)
                CompletePkQuestTask("BOUNTY_TANK_5K", (int)result.DamageReceived);
            */
        }
    }
}

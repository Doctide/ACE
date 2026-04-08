using System.Collections.Generic;
using System.Linq;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity.Bounties
{
    public class BountyCollection
    {
        private readonly Dictionary<uint, BountyContract> _contracts;

        public BountyCollection(Player owner, List<BountyContract> contracts)
        {
            _contracts = new Dictionary<uint, BountyContract>();
            LoadBountyContracts(owner, contracts);
        }

        private void LoadBountyContracts(Player owner, List<BountyContract> contracts)
        {
            foreach (var contract in contracts)
            {
                if (contract.IsDestroyed)
                {
                    owner.TryRemoveFromInventoryWithNetworking(contract.Guid.Full, out _, Player.RemoveFromInventoryAction.ConsumeItem);
                    continue;
                }

                if (!contract.BountyTargetGuid.HasValue)
                {
                    owner.TryRemoveFromInventoryWithNetworking(contract.Guid.Full, out _, Player.RemoveFromInventoryAction.ConsumeItem);
                    continue;
                }

                if (contract.State == BountyContract.BountyState.Pending)
                {
                    owner.TryRemoveFromInventoryWithNetworking(contract.Guid.Full, out _, Player.RemoveFromInventoryAction.ConsumeItem);
                    continue;
                }

                var targetGuid = (uint)contract.BountyTargetGuid.Value;

                // prevent duplicates
                if (_contracts.ContainsKey(targetGuid))
                {
                    owner.TryRemoveFromInventoryWithNetworking(contract.Guid.Full, out _, Player.RemoveFromInventoryAction.ConsumeItem);
                    continue;
                }

                _contracts[targetGuid] = contract;
            }
        }

        public bool TryGetBountyContract(uint targetGuid, out BountyContract contract)
        {
            return _contracts.TryGetValue(targetGuid, out contract);
        }

        public bool TryGetActiveBountyContract(uint targetGuid, Player owner, out BountyContract contract)
        {
            contract = null;

            if (!_contracts.TryGetValue(targetGuid, out var c))
                return false;

            if (c.IsBountyExpired && c.State != BountyContract.BountyState.Expired)
            {
                c.SetState(BountyContract.BountyState.Expired, owner);
                return false;
            }

            if (c.State != BountyContract.BountyState.Active)
                return false;

            contract = c;
            return true;
        }

        public int GetTotalCount() => _contracts.Count;

        public bool AddBountyContract(uint targetGuid, BountyContract contract)
        {
            if (contract.State == BountyContract.BountyState.Pending)
            {
                return false;
            }

            if (_contracts.ContainsKey(targetGuid))
                return false;

            _contracts[targetGuid] = contract;
            return true;
        }

        public bool RemoveBountyContract(uint targetGuid)
        {
            return _contracts.Remove(targetGuid);
        }

        public bool CompleteContract(uint targetGuid, Player owner = null)
        {
            if (!_contracts.TryGetValue(targetGuid, out var contract))
                return false;

            if (contract.State != BountyContract.BountyState.Active)
                return false;

            contract.SetState(BountyContract.BountyState.Completed, owner);
            return true;
        }

        public bool ExpireContract(uint targetGuid, Player owner = null)
        {
            if (!_contracts.TryGetValue(targetGuid, out var contract))
                return false;

            if (contract.State == BountyContract.BountyState.Expired)
                return false;

            contract.SetState(BountyContract.BountyState.Expired, owner);
            return true;
        }

        public void CheckContracts(Player owner)
        {
            foreach (var kvp in _contracts.ToList())
            {
                var targetGuid = kvp.Key;
                var contract = kvp.Value;

                // remove destroyed contracts (should never happen)
                if (contract.IsDestroyed)
                {
                    _contracts.Remove(targetGuid);

                    owner.TryRemoveFromInventoryWithNetworking(
                        contract.Guid.Full,
                        out _,
                        Player.RemoveFromInventoryAction.ConsumeItem
                    );

                    continue;
                }

                // remove invalid contracts without a target (should never happen)
                if (!contract.BountyTargetGuid.HasValue)
                {
                    _contracts.Remove(targetGuid);

                    owner.TryRemoveFromInventoryWithNetworking(
                        contract.Guid.Full,
                        out _,
                        Player.RemoveFromInventoryAction.ConsumeItem
                    );

                    continue;
                }

                // remove contracts that are in pending (should never happen except for migration cases)
                if (contract.State == BountyContract.BountyState.Pending)
                {
                    _contracts.Remove(targetGuid);

                    owner.TryRemoveFromInventoryWithNetworking(
                        contract.Guid.Full,
                        out _,
                        Player.RemoveFromInventoryAction.ConsumeItem
                    );

                    continue;
                }

                // expire contracts that are past their expiration time
                var wasExpired = contract.State == BountyContract.BountyState.Expired;

                if (contract.IsBountyExpired && !wasExpired)
                {
                    contract.SetState(BountyContract.BountyState.Expired, owner);
                    contract.UpdateUiEffects(owner);
                }
            }
        }
    }
}

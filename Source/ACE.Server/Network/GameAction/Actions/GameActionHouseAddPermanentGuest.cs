using ACE.Common.Extensions;
using ACE.Server.Command.Handlers;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Adds a guest to your house guest list
    /// </summary>
    public static class GameActionHouseAddPermanentGuest
    {
        [GameAction(GameActionType.AddPermanentGuest)]
        public static void Handle(ClientMessage message, Session session)
        {
            //Console.WriteLine("Received 0x245 - HouseAddPermanentGuest");

            if (!PlayerCommands.CheckPlayerCommandRateLimit(session))
            {
                return;
            }

            var guestName = message.Payload.ReadString16L();

            session.Player.HandleActionAddGuest(guestName);
        }
    }
}

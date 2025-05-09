using ACE.Common.Extensions;
using ACE.Server.Command.Handlers;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionRemovePlayerPermission
    {
        [GameAction(GameActionType.RemovePlayerPermission)]
        public static void Handle(ClientMessage message, Session session)
        {
            if (!PlayerCommands.CheckPlayerCommandRateLimit(session))
            {
                return;
            }

            // player to revoke corpse looting permissions from
            var playerName = message.Payload.ReadString16L();

            session.Player.HandleActionRemovePlayerPermission(playerName);
        }
    }
}

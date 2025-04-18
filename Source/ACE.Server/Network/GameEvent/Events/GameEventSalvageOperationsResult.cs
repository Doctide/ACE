using System.Collections.Generic;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Network.Structure;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventSalvageOperationsResult : GameEventMessage
    {
        public GameEventSalvageOperationsResult(Session session, Skill skill, List<SalvageMessage> messages)
            : base(GameEventType.SalvageOperationsResult, GameMessageGroup.UIQueue, session, 40) // 40 is the max seen in retail pcaps
        {
            Writer.Write((uint)skill);
            Writer.Write(0);    // not salvagable item guid list?
            Writer.Write(messages.Count);
            foreach (var message in messages)
                Writer.Write(new SalvageResult(message));

            Writer.Write(skill == Skill.Salvaging ? (session.Player.AugmentationBonusSalvage + (session.Player.EnlightenmentCustomLevel >= 1 ? 1 : 0))* 25 : 0);
        }
    }
}

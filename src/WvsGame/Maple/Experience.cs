using Destiny.Maple.Characters;
using Destiny.Constants;
using Destiny.Network.Common;

namespace Destiny.Maple
{
    public sealed class Experience
    {
        public int Amount { get; }

        public Experience(int amount) : base()
        {
            Amount = amount;
        }

        public const int GivenExpLimit = int.MaxValue;

        public static int GetExpNeededForLevel(int level)
        {
            return level > 200 ? 2000000000 : CharacterConstants.ExperienceTables.CharacterLevel[level];
        }

        public static void giveExp(Character character, int exp)
        {
            long myPlusGivenExp = character.Stats.Experience + (long)exp;

            if (myPlusGivenExp > GivenExpLimit)
            {
                character.Stats.Experience = GivenExpLimit;
            }
            else
            {
                character.Stats.Experience += exp;
                Packet ShowGivenExp = GetShowExpGainPacket(true, exp, false, 0, 0);
                character.Client.Send(ShowGivenExp);
            }
        }

        public static Packet GetShowExpGainPacket(bool white, int ammount, bool inChat, int partyBonus, int equipBonus)
        {
            return Character.GetShowSidebarInfoPacket(ServerConstants.MessageType.IncreaseEXP, white, 0, ammount, inChat, partyBonus, equipBonus);
        }

    }
}
using Destiny.Maple.Characters;
using Destiny.Network.PacketFactory;

namespace Destiny.Maple.Commands.Implementation
{
    public sealed class AdminShopCommand : Command
    {
        // NOTE: The Npc that is the shop owner.
        public const int Npc = 2084001;

        public override string Name
        {
            get
            {
                return "adminshop";
            }
        }

        public override string Parameters
        {
            get
            {
                return string.Empty;
            }
        }

        public override bool IsRestricted
        {
            get
            {
                return true;
            }
        }

        public override void Execute(Character caller, string[] args)
        {
            if (args.Length != 0)
            {
                this.ShowSyntax(caller);
            }
            else
            {
                caller.Client.Send(AdminPackets.ShowAdminShop(Npc));
            }
        }
    }
}
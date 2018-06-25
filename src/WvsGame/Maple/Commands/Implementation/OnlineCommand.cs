using Destiny.Maple.Characters;

namespace Destiny.Maple.Commands.Implementation
{
    public sealed class OnlineCommand : Command
    {
        public override string Name
        {
            get
            {
                return "online";
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
                ShowSyntax(caller);
            }
            else
            {
                Character.Notify(caller, "[Online]");

                //foreach (WorldServer world in MasterServer.Worlds)
                //{
                //    foreach (ChannelServer channel in world)
                //    {
                //        foreach (Character loopCharacter in channel.Characters)
                //        {
                //            caller.Notify("   -" + loopCharacter.Name);
                //        }
                //    }
                //}
            }
        }
    }
}

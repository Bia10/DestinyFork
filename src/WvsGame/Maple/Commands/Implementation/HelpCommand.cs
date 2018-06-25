﻿using Destiny.Maple.Characters;

namespace Destiny.Maple.Commands.Implementation
{
    public sealed class HelpCommand : Command
    {
        public override string Name
        {
            get
            {
                return "help";
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
                return false;
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
                Character.Notify(caller, "[Help]");

                foreach (Command command in CommandFactory.Commands)
                {
                    if ((command.IsRestricted && caller.IsMaster) || !command.IsRestricted && !(command is HelpCommand))
                    {
                        Character.Notify(caller, string.Format("    !{0} {1}", command.Name, command.Parameters.ClearFormatters()));
                    }
                }
            }
        }
    }
}

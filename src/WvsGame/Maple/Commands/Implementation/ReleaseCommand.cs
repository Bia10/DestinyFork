﻿using Destiny.Maple.Characters;

namespace Destiny.Maple.Commands.Implementation
{
    public sealed class ReleaseCommand : Command
    {
        public override string Name
        {
            get
            {
                return "release";
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
                Character.Release(caller);
            }
        }
    }
}

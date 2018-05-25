using Destiny.Maple.Characters;
using System;
using Destiny.Constants;

namespace Destiny.Maple.Commands.Implementation
{
    public sealed class JobCommand : Command
    {
        public override string Name
        {
            get
            {
                return "job";
            }
        }

        public override string Parameters
        {
            get
            {
                return "{ id | name}";
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
            if (args.Length != 1)
            {
                this.ShowSyntax(caller);
            }
            else
            {
                try
                {
                    short jobID = short.Parse(args[0]);

                    if (Enum.IsDefined(typeof(CharacterConstants.Job), jobID))
                    {
                        caller.Jobs.Job = (CharacterConstants.Job)jobID;
                    }
                    else
                    {
                        Character.Notify(caller, "[Command] Invalid job ID.");
                    }
                }
                catch (FormatException)
                {
                    try
                    {
                        caller.Jobs.Job = (CharacterConstants.Job)Enum.Parse(typeof(CharacterConstants.Job), args[0], true);
                    }
                    catch (ArgumentException)
                    {
                        Character.Notify(caller, "[Command] Invalid job name.");
                    }
                }
            }
        }
    }
}

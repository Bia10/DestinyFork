﻿using System;

using Destiny.Maple.Characters;
using Destiny.Data;

namespace Destiny.Maple.Commands.Implementation
{
    public sealed class SearchCommand : Command
    {
        public override string Name
        {
            get
            {
                return "search";
            }
        }

        public override string Parameters
        {
            get
            {
                return "[ -item | -map | -mob | -npc | -quest ] label";
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
            if (args.Length == 0)
            {
                ShowSyntax(caller);
            }
            else
            {
                string query;

                query = args[0].StartsWith("-", StringComparison.Ordinal) ? CombineArgs(args, 1).ToLower() : CombineArgs(args).ToLower();

                if (query.Length < 2)
                {
                    Character.Notify(caller, "[Command] Please enter at least 2 characters.");
                }
                else
                {
                    bool hasResult = false;

                    Character.Notify(caller, "[Results]");

                    using (Database.TemporarySchema(Database.SchemaMCDB))
                    {
                        foreach (Datum datum in new Datums("strings").Populate("`label` LIKE '%{0}%'", query))
                        {
                            Character.Notify(caller, string.Format("   -{0}: {1}", (string)datum["label"], (int)datum["objectid"]));

                            hasResult = true;
                        }
                    }

                    if (!hasResult)
                    {
                        Character.Notify(caller, "   No result found.");
                    }
                }
            }
        }
    }
}

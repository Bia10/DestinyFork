﻿using Destiny.Maple.Characters;
using Destiny.Maple.Data;

namespace Destiny.Maple.Commands.Implementation
{
    public sealed class LearnSkillCommand : Command
    {
        public override string Name
        {
            get
            {
                return "learn";
            }
        }

        public override string Parameters
        {
            get
            {
                return "{ skillID } [ skillLvl ]";
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
            if (args.Length < 1)
            {
                ShowSyntax(caller);
            }
            else
            {
                int skillID = int.Parse(args[0]);
                int skillLVL = int.Parse(args[1]);
             
                if (DataProvider.Skills.ContainsKey(skillID))
                {
                    if (caller.Skills.Contains(skillID))
                    {
                        Skill skillToModify = caller.Skills[skillID];
                        skillToModify.CurrentLevel = (byte)skillLVL;

                        CharacterSkills.UpdateSkill(skillToModify);
                    }

                    // TODO: needs proper treatment
                    else if(!caller.Skills.Contains(skillID))
                    {
                        Skill skillToAdd = new Skill(skillID)
                        {
                            CurrentLevel = (byte) skillLVL,
                            MapleID = skillID
                        };

                        CharacterSkills.UpdateSkill(skillToAdd);
                    }
                }
                else
                {
                    Character.Notify(caller, "[Command] Invalid skill ID.");
                }

            }
        }

    }
}
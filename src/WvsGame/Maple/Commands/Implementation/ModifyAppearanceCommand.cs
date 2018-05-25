using Destiny.Constants;
using Destiny.Maple.Characters;

namespace Destiny.Maple.Commands.Implementation
{
    public sealed class ModifyAppearanceCommand : Command
    {
        public override string Name
        {
            get
            {
                return "appearance";
            }
        }

        public override string Parameters
        {
            get
            {
                return "{ appearance } [ appearanceID ]";
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
            // invalid use need ID also
            if (args.Length < 1)
            {
                ShowSyntax(caller);
            }
            // valid use
            else
            {
                int appearanceID = 0;

                // valid num of args for parsing
                if (args.Length > 1)
                {
                    int.TryParse(args[args.Length - 1], out appearanceID);
                }

                // valid appearance?
                if (appearanceID < 1)
                {
                   return;
                }
                
                // parse appearance
                string appearance = args[0];

                switch (appearance)
                {
                    case "gender":
                    case "Gender":
                        CharacterConstants.Gender newGender = (CharacterConstants.Gender) appearanceID;

                        CharacterAppearance.ChangeGender(caller, newGender);
                        break;

                    case "skin":
                    case "Skin":
                        byte newSkin = (byte) appearanceID;

                        CharacterAppearance.ChangeSkin(caller, newSkin);
                        break;

                    case "hair":
                    case "Hair":
                        int newHair =  appearanceID;

                        CharacterAppearance.ChangeHair(caller, newHair);
                        break;

                    case "face":
                    case "Face":
                        int newFace = appearanceID;

                        CharacterAppearance.ChangeFace(caller, newFace);
                        break;
                }

            }
        }
    }
}
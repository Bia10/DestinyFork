using System;

using Destiny.Maple.Characters;
using Destiny.Constants;

namespace Destiny.Maple.Commands.Implementation
{
    public class MapCommand : Command
    {
        public override string Name
        {
            get
            {
                return "map";
            }
        }

        public override string Parameters
        {
            get
            {
                return "{ { id | keyword | exact name } [portal] | -current }";
            }
        }

        public override bool IsRestricted
        {
            get
            {
                return true;
            }
        }

        public override void Execute(Character caller, string[] args) // TODO: fugly rework
        {
            if (args.Length == 0)
            {
                this.ShowSyntax(caller);
            }
            else
            {
                if (args.Length == 1 && args[0] == "-current")
                {
                    Character.Notify(caller, "[Command] Current map: " + caller.Map.MapleID);
                    Character.Notify(caller, "   -X: " + caller.Position.X);
                    Character.Notify(caller, "   -Y: " + caller.Position.Y);
                }
                else
                {
                    string mapName = "";
                    int mapID = int.TryParse(args[0], out mapID) ? mapID : -1;
                    byte portalID = 0;

                    if (args.Length >= 2)
                    {
                        byte.TryParse(args[1], out portalID);
                    }

                    if (mapID == -1)
                    {
                        mapName = string.Join(" ", args);
                        MapConstants.CommandMaps val;
                        Enum.TryParse(mapName.ToAlphaNumeric(), true, out val);
                        if (val > 0)
                            mapID = (int)val;
                    }

                    if (mapID > -1)
                    {
                        if (true) // TODO: Check if map exists.
                            caller.SendChangeMapRequest(mapID, portalID);
                        //else
                            //caller.Notify(string.Format("[Command] Invalid map ID {0}.", mapID));
                    }
                    else
                    {
                        Character.Notify(caller, string.Format("[Command] Invalid map name \"{0}\".", mapName));
                    }
                }
            }
        }
    }
}

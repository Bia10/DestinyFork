using System;

namespace Destiny.Constants
{
    public class WorldConstants
    {
        #region World
        public enum WorldFlag : byte
        {
            None,
            Event,
            New,
            Hot
        }

        public enum WorldNames : byte
        {
            Scania,
            Bera,
            Broa,
            Windia,
            Khaini,
            Bellocan,
            Mardia,
            Kradia,
            Yellonde,
            Demethos,
            Elnido,
            Kastia,
            Judis,
            Arkenia,
            Plana,
            Galicia,
            Kalluna,
            Stius,
            Croa,
            Zenith,
            Medere
        }

        public enum WorldStatus : short
        {
            Normal,
            HighlyPopulated,
            Full
        }

        public static class WorldNameResolver
        {
            public static byte GetID(string name)
            {
                try
                {
                    return (byte)Enum.Parse(typeof(WorldNames), name.ToCamel());
                }
                catch
                {
                    throw new ArgumentException("The specified World name is invalid.");
                }
            }

            public static string GetName(byte id)
            {
                try
                {
                    return Enum.GetName(typeof(WorldNames), id);
                }
                catch
                {
                    throw new ArgumentException("The specified World ID is invalid.");
                }
            }

            public static bool IsValid(byte id)
            {
                return Enum.IsDefined(typeof(WorldNames), id);
            }

            public static bool IsValid(string name)
            {
                try
                {
                    WorldNameResolver.GetID(name);
                    return true;
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }
        }
        #endregion
    }
}
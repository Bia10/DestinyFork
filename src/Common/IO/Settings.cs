using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using Destiny.Data;
using Destiny.Network.Common;

namespace Destiny.IO
{
    public static class Settings
    {
        private static Dictionary<string, string> Dictionary { get; set; }

        public static string Path { get; private set; }

        public static void Initialize(string path = null, bool showRefresh = false)
        {
            if (path == null)
            {
                path = Application.ExecutablePath + "Configuration.ini";
            }

            Dictionary?.Clear();

            Path = path;
            Dictionary = new Dictionary<string, string>();

            string[] array = Path.Split('\\');
            string name = array[array.Length - 1];

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(string.Format("Unable to find configuration file '{0}'.", name));
            }

            string currentSection = string.Empty;

            using (StreamReader file = new StreamReader(path))
            {
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    if (line.StartsWith("[", StringComparison.Ordinal) && line.EndsWith("]"))
                    {
                        currentSection = line.Trim('[', ']');
                    }
                    else if (line.Contains("="))
                    {
                        Dictionary.Add(string.Format("{0}{1}{2}",
                            currentSection,
                            (currentSection != string.Empty) ? "/" : string.Empty,
                            line.Split('=')[0]),
                            line.Split('=')[1].Split(';')[0]);
                    }
                }
            }

            Log.SkipLine();
            Log.Success("Settings file '{0}' {1}.", name, showRefresh ? "refreshed" : "loaded");

            Packet.LogLevel = GetEnum<LogLevel>("Log/Packets");
            Log.ShowStackTrace = GetBool("Log/StackTrace");
            LoadingIndicator.ShowTime = GetBool("Log/LoadTime");

            Database.Host = GetString("Database/Host");
            Database.Schema = GetString("Database/Schema");

            // This is only available from WvsGame, so it's better to not have errors thrown in-case it's ran from somewhere else.
            // Obviously this isn't the best way to do that, but feedback is welcomed.
            try
            {
                Database.SchemaMCDB = GetString("Database/SchemaMCDB");
            }

            catch
            { }

            Database.Username = GetString("Database/Username");
            Database.Password = GetString("Database/Password");
        }

        public static void Refresh()
        {
            Initialize(Path, true);
        }

        public static int GetInt(string key, params object[] args)
        {
            try
            {
                return int.Parse(Dictionary[string.Format(key, args)]);
            }

            catch
            {
                throw new SettingReadException(key);
            }
        }

        public static short GetShort(string key, params object[] args)
        {
            try
            {
                return short.Parse(Dictionary[string.Format(key, args)]);
            }

            catch
            {
                throw new SettingReadException(key);
            }
        }

        public static byte GetByte(string key, params object[] args)
        {
            try
            {
                return byte.Parse(Dictionary[string.Format(key, args)]);
            }

            catch
            {
                throw new SettingReadException(key);
            }
        }

        public static sbyte GetSByte(string key, params object[] args)
        {
            try
            {
                return sbyte.Parse(Dictionary[string.Format(key, args)]);
            }

            catch
            {
                throw new SettingReadException(key);
            }
        }

        public static bool GetBool(string key, params object[] args)
        {
            try
            {
                return bool.Parse(Dictionary[string.Format(key, args)]);
            }

            catch
            {
                throw new SettingReadException(key);
            }
        }

        public static string GetString(string key, params object[] args)
        {
            try
            {
                return Dictionary[string.Format(key, args)];
            }

            catch
            {
                throw new SettingReadException(key);
            }
        }

        public static IPAddress GetIPAddress(string key, params object[] args)
        {
            try
            {
                return Dictionary[string.Format(key, args)] == "localhost" ? IPAddress.Loopback : IPAddress.Parse(Dictionary[string.Format(key, args)]);
            }

            catch
            {
                throw new SettingReadException(key);
            }
        }

        public static T GetEnum<T>(string key, params object[] args)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), Dictionary[string.Format(key, args)]);
            }

            catch
            {
                throw new SettingReadException(key);
            }
        }
    }
}
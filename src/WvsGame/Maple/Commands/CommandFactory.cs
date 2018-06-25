using System;
using System.Linq;
using System.Reflection;

using Destiny.IO;
using Destiny.Maple.Characters;

namespace Destiny.Maple.Commands
{
    public static class CommandFactory
    {
        public static Commands Commands { get; private set; }

        public static void Initialize()
        {
            Commands = new Commands();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsSubclassOf(typeof(Command))))
            {
                Commands.Add((Command)Activator.CreateInstance(type));
            }
        }

        public static void Execute(Character caller, string text)
        {
            string[] splitted = text.Split(' ');

            splitted[0] = splitted[0].ToLower();

            string commandName = splitted[0].TrimStart(Application.CommandIndiciator);

            string[] args = new string[splitted.Length - 1];

            for (int i = 1; i < splitted.Length; i++)
            {
                args[i - 1] = splitted[i];
            }

            if (Commands.Contains(commandName))
            {
                Command command = Commands[commandName];

                if (!command.IsRestricted || caller.IsMaster)
                {
                    try
                    {
                        command.Execute(caller, args);
                    }
                    catch (Exception e)
                    {
                        Character.Notify(caller, "[Command] Unknown error: " + e.Message);

                        Log.SkipLine();
                        Log.Error("{0} error by {1}: ", e, command.GetType().Name, caller.Name);
                        Log.SkipLine();
                    }
                }
                else
                {
                    Character.Notify(caller, "[Command] Restricted command.");
                }
            }
            else
            {
                Character.Notify(caller, "[Command] Invalid command.");
            }
        }
    }
}
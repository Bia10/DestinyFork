using Destiny.Data;
using Destiny.IO;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Net;

namespace Destiny
{
    public static class WvsGameSetup
    {
        private const string McDBFileName = @"..\..\sql\MCDB.sql";
        private const string GameDBFileName = @"..\..\sql\GameDB.sql";

        private static string databaseHost = string.Empty;
        private static string databaseSchema = string.Empty;
        private static string databaseSchemaMCDB = string.Empty;
        private static string databaseUsername = string.Empty;
        private static string databasePassword = string.Empty;

        public static void Run()
        {
            Log.Entitle("Welcome to WvsGame server Setup");

            Log.Inform("If you do not know what value to put in as your database credentials, leave the field blank to apply default option.");
            Log.SkipLine();
            Log.Inform("Default options are:\n DB Host: localhost;\n Game server DB Schema: game;\n MapleStory DB(MCDB) Schema: mcdb;\n DB Username: root;\n DB Password: ;");
            Log.SkipLine();
            Log.Inform("Your advised not to use default username and none password,           not that we care :) ");

            Log.Entitle("Game Database Setup");

            databaseConfiguration:
            Log.Inform("Please enter your game database credentials: ");
            Log.SkipLine();

            try
            {
                databaseHost = Log.Input("DB Host: ", "localhost");
                databaseSchema = Log.Input("Game server DB Schema: ", "game");
                databaseSchemaMCDB = Log.Input("MapleStory DB(MCDB) Schema: ", "mcdb");
                databaseUsername = Log.Input("DB Username (that can access both databases): ", "root");
                databasePassword = Log.Input("DB Password: ", "");

                using (Database.TemporaryConnection(databaseHost, databaseSchema, databaseUsername, databasePassword))
                {
                    Database.Test();
                    Log.SkipLine();
                    Log.Success("Connection to game database was tested and is ready to be populated with data!");
                    Log.SkipLine();
                    Log.Inform("The setup will now check for a Game server database(GameDB).");
                    Log.Inform("It is assumed that the file for creation of Game server database(GameDB) is present at path: " + GameDBFileName + "");
                    Log.SkipLine();

                    if (Log.YesNo("Populate the " + databaseSchema + " database as your game server DB? ", true))
                    {
                        Log.SkipLine();
                        Log.Inform("Please wait..., trying to populate game server DB");

                        if (PopulateGameDatabase())
                        {
                            Log.Inform("Done populating game database '{0}'!", databaseSchema);
                        }
                        else
                        {
                            Log.SkipLine();
                            Log.Error("Fatal error occurred cannot proceed, press ENTER to exit!");
                            Console.ReadLine(); // console wait for enter to exit
                            System.Environment.Exit(-1);
                        }
                    }
                }
            }

            catch (MySqlException e)
            {
                Log.SkipLine();
                Log.Error(e);
                Log.SkipLine();

                if (e.Message.Contains("Unknown database") && Log.YesNo("Create and populate the " + databaseSchema + " database as your game server DB? ", true))
                {
                    try
                    {
                        Log.SkipLine();
                        Log.Inform("Please wait..., trying to populate game server DB.");

                        if (PopulateGameDatabase())
                        {
                            Log.Inform("Database '{0}' created.", databaseSchema);
                        }
                        else
                        {
                            Log.SkipLine();
                            Log.Error("Fatal error occurred cannot proceed, press ENTER to exit!");
                            Console.ReadLine(); // console wait for enter to exit
                            System.Environment.Exit(-1);
                        }
                    }
                    catch (Exception gamedbE)
                    {
                        Log.SkipLine();
                        Log.Error("Error while creating '{0}': ", gamedbE, databaseSchema);
                        goto databaseConfiguration;
                    }
                }
                else
                {
                    goto databaseConfiguration;
                }
            }
            catch
            {
                Log.SkipLine();
                goto databaseConfiguration;
            }

            Log.SkipLine();
            Log.Success("Game database configured!");

            Log.Entitle("MapleStory Database Setup");

            mcdbConfiguration:
            Log.Inform("The setup will now check for a MapleStory database(MCDB).");
            Log.Inform("It is assumed that the file for creation of MapleStory database(MCDB) is present at path: " + McDBFileName + "");
            Log.SkipLine();

            try
            {
                using (Database.TemporaryConnection(databaseHost, databaseSchemaMCDB, databaseUsername, databasePassword, true))
                {
                    Database.Test();
                    Log.SkipLine();
                    Log.Success("Connection to MapleStory database was tested and is ready to be populated with data!");
                    Log.SkipLine();

                    if (Log.YesNo("Populate the " + databaseSchemaMCDB + " database as your MapleStory DB? ", true))
                    {
                        Log.SkipLine();
                        Log.Inform("Please wait..., trying to populate MaplestoryDB.");

                        if (PopulateMapleStoryDatabase())
                        {
                            Log.Inform("Database '{0}' created.", databaseSchemaMCDB);
                        }
                        else
                        {
                            Log.SkipLine();
                            Log.Error("Fatal error occurred cannot proceed, press ENTER to exit!");
                            Console.ReadLine(); // console wait for enter to exit
                            System.Environment.Exit(-1);
                        }
                    }
                }
            }

            catch (MySqlException ex)
            {
                Log.Error(ex);
                Log.SkipLine();

                if (ex.Message.Contains("Unknown database") && Log.YesNo("Create and populate the " + databaseSchemaMCDB + " database as your MapleStory DB? ", true))
                {
                    try
                    {
                        Log.SkipLine();
                        Log.Inform("Please wait...");
                        PopulateMapleStoryDatabase();
                        Log.Inform("Database '{0}' created.", databaseSchemaMCDB);
                    }
                    catch (Exception mcdbE)
                    {
                        Log.SkipLine();
                        Log.Error("Error while creating '{0}': ", mcdbE, databaseSchemaMCDB);
                        goto mcdbConfiguration;
                    }
                }
                else
                {
                    Log.SkipLine();
                    goto mcdbConfiguration;
                }
            }

            Log.SkipLine();
            Log.Success("MapleStory database configured!");

            Log.Entitle("Game Server Configuration");
            Log.Inform("Again you can leave the fields blank to apply default option.");
            Log.SkipLine();

            IPAddress centerIP = Log.Input("Enter the IP of the center server[Default: IPAddress.Loopback]: ", IPAddress.Loopback);
            string securityCode = Log.Input("Assign the security code between servers[Default: ]: ", "");
            int autoRestartTime = Log.Input("Automatic restart time in seconds[Default: 15]: ", 15);

            Log.SkipLine();
            Log.Success("Game server configured!");

            Log.Entitle("User Profile Setup");
            Log.Inform("Please choose what detail of debug information you want to display.\n  A. Hide packets (recommended)\n  B. Show names\n  C. Show content (expert usage, spam)");
            Log.SkipLine();

            LogLevel logLevel;

            multipleChoice:
            switch (Log.Input("Please enter your choice: ", "Hide").ToLower())
            {
                case "a":
                case "hide":
                    logLevel = LogLevel.None;
                    break;

                case "b":
                case "names":
                    logLevel = LogLevel.Name;
                    break;

                case "c":
                case "content":
                    logLevel = LogLevel.Full;
                    break;

                default:
                    goto multipleChoice;
            }

            Log.Entitle("Please wait...");
            Log.Inform("Writing settings to 'WvsGame.ini'...");

            string lines = string.Format(
                @"[Log]
                Packets={0}
                StackTrace=False
                LoadTime=False
                JumpLists=3
    
                [Server]
                AutoRestartTime={1}

                [Center]
                IP={2}
                Port=8485
                SecurityCode={3}
    
                [Database]
                Host={4}
                Schema={5}
                SchemaMCDB={6}
                Username={7}
                Password={8}",
                logLevel, autoRestartTime, centerIP, securityCode, databaseHost,
                databaseSchema, databaseSchemaMCDB, databaseUsername, databasePassword).Replace("  ", "");

            using (StreamWriter file = new StreamWriter(Application.ExecutablePath + "WvsGame.ini"))
            {
                file.WriteLine(lines);
            }

            Log.SkipLine();
            Log.Success("Configuration is done! Game server set up for use successfully.");
        }

        // TODO: in debug mode this may throw ContextSwitchDeadlock
        // TODO: add ascii progress bars
        private static bool PopulateMapleStoryDatabase() 
        {
            try
            {
                if (Database.ExecuteFile(databaseHost, databaseUsername, databasePassword,
                    Application.ExecutablePath + McDBFileName))
                {
                    return true;
                }
            return false;
            }

            catch (Exception ex)
            {
                Log.SkipLine();
                Log.Error(ex);
                Log.SkipLine();
                return false;
            }
        }

        // TODO: in debug mode this may throw ContextSwitchDeadlock
        // TODO: add ascii progress bars
        private static bool PopulateGameDatabase()
        {
            try
            {
                if (Database.ExecuteFile(databaseHost, databaseUsername, databasePassword,
                Application.ExecutablePath + GameDBFileName))
                {
                    return true;
                }
            return false;
            }

            catch (Exception ex)
            {
                Log.SkipLine();
                Log.Error(ex);
                Log.SkipLine();
                return false;
            }
        }

    }
}
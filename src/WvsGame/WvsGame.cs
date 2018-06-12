using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Destiny.Data;
using Destiny.Interoperability;
using Destiny.IO;
using Destiny.Maple.Data;
using Destiny.Network;
using Destiny.Shell;

namespace Destiny
{
    public static class WvsGame
    {
        private static bool isAlive;
        private static byte channelID;
        private static readonly ManualResetEvent AcceptDone = new ManualResetEvent(false);
        public static readonly ManualResetEvent CenterConnectionDone = new ManualResetEvent(false);

        public static TcpListener Listener { get; private set; }
        public static IPEndPoint RemoteEndPoint { get; set; }
        public static GameToCenterServer CenterConnection { get; set; }
        public static GameClients Clients { get; private set; }
        public static int AutoRestartTime { get; set; }

        public static byte WorldID { get; set; }
        public static string WorldName { get; set; }
        public static string TickerMessage { get; set; }
        public static int ExperienceRate { get; set; }
        public static int QuestExperienceRate { get; set; }
        public static int PartyQuestExperienceRate { get; set; }
        public static int MesoRate { get; set; }
        public static int DropRate { get; set; }
        public static bool AllowMultiLeveling { get; set; }

        public static bool IsAlive
        {
            get
            {
                return isAlive;
            }
            set
            {
                isAlive = value;

                if (!value)
                {
                    AcceptDone.Set();
                }
            }
        }

        public static byte ChannelID
        {
            get
            {
                return channelID;
            }
            set
            {
                channelID = value;

                Console.Title = string.Format("WvsGame v.{0}.{1} ({2}-{3}) - MOTD: {4}",
                    Application.MapleVersion,
                    Application.PatchVersion,
                    WorldName,
                    ChannelID,
                    TickerMessage);
            }
        }

        private static void InitiateGameServerSetup()
        {
            Log.SkipLine();
            Log.Inform("Initiating Destiny game server setup ...");
            WvsGameSetup.Run();
        }

        private static void HaltOnFatalError()
        {
            Log.SkipLine();
            Log.Error("Fatal error occurred cannot proceed, press ENTER to exit!");
            Console.ReadLine();
            Environment.Exit(-1);
        }

        private static void CloseGameServer()
        {
            // Disconnect clients
            foreach (GameClient client in Clients)
            {
                client.Stop();
            }
            // Dispose from thread
            Dispose();
            // Inform user
            Log.SkipLine();
            Log.Warn("Server stopped.");
            Log.SkipLine();
        }

        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].ToLower() == "setup")
            {
                InitiateGameServerSetup();
            }

            if (args.Length > 0 && args[0].ToLower() != "setup")
            {
                Log.Warn("Arguments found yet none were recognized! Arguments: " + args);

                HaltOnFatalError();
            }

            if (!File.Exists(Application.ExecutablePath + "WvsGame.ini"))
            {
                Log.Warn("Could not find file WvsGame.ini at path: {0}", Application.ExecutablePath);

                if (Log.YesNo("Would you like to initiate setup process for Destiny game server? ", true))
                {
                    InitiateGameServerSetup();
                }
                else
                {
                    Log.Warn("\n Argument setup was not found!" +
                             "\n Neither was found WvsGame.ini!" +
                             "\n And finally you chose not to setup game server!" +
                             "\n What shall i do then? Make a prophecy about your intent?");

                    HaltOnFatalError();
                }
            }
          
            start: 
            Clients = new GameClients();

            Log.Entitle("WvsGame v.{0}.{1}", Application.MapleVersion, Application.PatchVersion);

            try
            {
                // Read game-server settings from ini
                Settings.Initialize(Application.ExecutablePath + "WvsGame.ini"); 
                // Test connection to database
                Database.Test();
                // Parse thru MaplestoryDB
                Database.Analyze(true);
                // Create key shortcuts
                Shortcuts.Apply();
                // Set auto-reset
                AutoRestartTime = Settings.GetInt("Server/AutoRestartTime");
                Log.Inform("Automatic restart time set to {0} seconds.", AutoRestartTime);
                // Initiate data handler
                DataProvider.Initialize();
                // Success game-server is alive! 
                IsAlive = true;
            }
            catch (Exception ex)
            {
                Log.SkipLine();
                Tracer.TraceErrorMessage(ex, "Exception occurred during game-server initialization!");
            }

            if (IsAlive)
            {
                CenterConnectionDone.Reset();

                new Thread(new ThreadStart(GameToCenterServer.Main)).Start();

                CenterConnectionDone.WaitOne();
#if DEBUG
                string linkPath = Path.Combine(Application.ExecutablePath, "LaunchClient.lnk");

                if (File.Exists(linkPath) && WorldID == 0 && ChannelID == 0) //Only for the first WvsGame instance, and only if shortcut exists
                {
                    System.Diagnostics.Process proc = new System.Diagnostics.Process {StartInfo = {FileName = linkPath}};
                    proc.Start();
                }
#endif
            }
            else
            {
                HaltOnFatalError();
            }

            while (IsAlive)
            {
                AcceptDone.Reset();

                try
                {
                    Listener.BeginAcceptSocket(new AsyncCallback(OnAcceptSocket), null);
                }

                catch (Exception ex)
                {
                    Log.SkipLine();
                    Tracer.TraceErrorMessage(ex, "Ex occured in game server!");
                    throw;
                }

                AcceptDone.WaitOne();
            }

            CloseGameServer();

            if (AutoRestartTime > 0) // TODO: fugly rework
            {
                Log.Inform("Attempting auto-restart in {0} seconds.", AutoRestartTime);
                Thread.Sleep(AutoRestartTime * 1000);
                goto start;
            }

            Console.Read();
        }

        public static void Listen()
        {
            Listener = new TcpListener(IPAddress.Any, RemoteEndPoint.Port);
            Listener.Start();
            Log.Inform("Initialized clients listener on {0}.", Listener.LocalEndpoint);
        }

        public static void Stop()
        {
            IsAlive = false;
        }

        private static void OnAcceptSocket(IAsyncResult asyncResult)
        {
            AcceptDone.Set();

            try
            {
                var gameClient = new GameClient(Listener.EndAcceptSocket(asyncResult));
            }

            catch (ObjectDisposedException) { } // TODO: Figure out why this crashes.
        }

        private static void Dispose()
        {
            CenterConnection?.Dispose();

            Listener?.Stop();

            Log.SkipLine();
            Log.Inform("Server disposed from thread {0}.", Thread.CurrentThread.ManagedThreadId);
        }
    }
}
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Destiny.Data;
using Destiny.Interoperability;
using Destiny.IO;
using Destiny.Maple;
using Destiny.Network;

namespace Destiny
{
    public static class WvsLogin
    {
        private static bool isAlive;
        private static readonly ManualResetEvent AcceptDone = new ManualResetEvent(false);
        public static readonly ManualResetEvent CenterConnectionDone = new ManualResetEvent(false);

        public static TcpListener Listener { get; private set; }
        public static LoginToCenterServer CenterConnection { get; set; }
        public static Worlds Worlds { get; private set; }
        public static LoginClients Clients { get; private set; }

        public static bool AutoRegister { get; private set; }
        public static bool RequestPin { get; private set; }
        public static bool RequestPic { get; private set; }
        public static bool RequireStaffIP { get; private set; }
        public static int MaxCharacters { get; private set; }

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

        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length == 1 && args[0].ToLower() == "setup" || !File.Exists(Application.ExecutablePath + "WvsLogin.ini"))
            {
                WvsLoginSetup.Run();
            }

            Worlds = new Worlds();
            Clients = new LoginClients();

            Log.Entitle("Destiny - Login Server v.{0}.{1}", Application.MapleVersion, Application.PatchVersion);

            try
            {
                Settings.Initialize(Application.ExecutablePath + "WvsLogin.ini");

                Database.Test();
                Database.Analyze(false);

                RequireStaffIP = Settings.GetBool("Server/RequireStaffIP");
                Log.Inform("Staff will{0}be required to connect through a staff IP.", RequireStaffIP ? " " : " not ");

                AutoRegister = Settings.GetBool("Server/AutoRegister");
                Log.Inform("Automatic registration {0}.", AutoRegister ? "enabled" : "disabled");

                RequestPin = Settings.GetBool("Server/RequestPin");
                Log.Inform("Pin will{0}be requested upon login.", RequestPin ? " " : " not ");

                RequestPic = Settings.GetBool("Server/RequestPic");
                Log.Inform("Pic will{0}be requested upon character selection.", RequestPic ? " " : " not ");

                MaxCharacters = Settings.GetInt("Server/MaxCharacters");
                Log.Inform("Maximum of {0} characters per account.", MaxCharacters);

                for (byte i = 0; i < Settings.GetByte("Server/Worlds"); i++)
                {
                    Worlds.Add(new World(i));
                }

                isAlive = true;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            if (IsAlive)
            {
                CenterConnectionDone.Reset();

                new Thread(new ThreadStart(LoginToCenterServer.Main)).Start();

                CenterConnectionDone.WaitOne();
            }
            else
            {
                Log.SkipLine();
                Log.Inform("Could not start server because of errors.");
            }

            while (IsAlive)
            {
                AcceptDone.Reset();
                Listener.BeginAcceptSocket(new AsyncCallback(OnAcceptSocket), null);
                AcceptDone.WaitOne();
            }

            foreach (LoginClient client in Clients)
            {
                client.Stop();
            }

            Dispose();

            Log.SkipLine();
            Log.Warn("Server stopped.");

            Console.Read();
        }

        public static void Listen()
        {
            Listener = new TcpListener(IPAddress.Any, Settings.GetInt("Server/Port"));
            Listener.Start();
            Log.Inform("Initialized clients listener on {0}.", Listener.LocalEndpoint);
        }

        private static void OnAcceptSocket(IAsyncResult ar)
        {
            AcceptDone.Set();

            try
            {
                var loginClient = new LoginClient(Listener.EndAcceptSocket(ar));
            }

            catch (ObjectDisposedException) { } // TODO: Figure out why this crashes.
        }

        public static void Stop()
        {
            IsAlive = false;
        }

        private static void Dispose()
        {
            Listener?.Stop();

            Log.SkipLine();
            Log.Inform("Server disposed.", Thread.CurrentThread.ManagedThreadId);
        }
    }
}
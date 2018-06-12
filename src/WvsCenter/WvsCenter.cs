using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Destiny.Data;
using Destiny.IO;
using Destiny.Maple;
using Destiny.Network;

namespace Destiny
{
    public static class WvsCenter
    {
        private static bool isAlive;
        private static readonly ManualResetEvent AcceptDone = new ManualResetEvent(false);

        public static CenterClient Login { get; set; }
        public static Worlds Worlds { get; private set; }
        public static Migrations Migrations { get; private set; }

        public static TcpListener Listener { get; private set; }
        public static List<CenterClient> Clients { get; private set; }

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

        private static void Main(string[] args)
        {
            if (args.Length == 1 && args[0].ToLower() == "setup" || !File.Exists(Application.ExecutablePath + "WvsCenter.ini"))
            {
                WvsCenterSetup.Run();
            }

            Worlds = new Worlds();
            Migrations = new Migrations();
            Clients = new List<CenterClient>();

            Log.Entitle("WvsCenter v.{0}.{1}", Application.MapleVersion, Application.PatchVersion);

            try
            {
                Settings.Initialize(Application.ExecutablePath + "WvsCenter.ini");

                Database.Test();
                Database.Analyze(false);

                CenterClient.SecurityCode = Settings.GetString("Server/SecurityCode");
                Log.Inform("Cross-servers code '{0}' assigned.", Log.MaskString(CenterClient.SecurityCode));

                IPAddress ip = IPAddress.Parse("127.0.0.1");
                Listener = new TcpListener(ip, Settings.GetInt("Server/Port"));
                Listener.Start();
                Log.Inform("Initialized clients listener on {0}.", Listener.LocalEndpoint);

                IsAlive = true;
            }
            catch (Exception e)
            {
                Log.SkipLine();
                Log.Error(e);
                Log.SkipLine();
            }

            if (IsAlive)
            {
                Log.SkipLine();
                Log.Success("WvsCenter started on thread {0}.", Thread.CurrentThread.ManagedThreadId);
                Log.SkipLine();
            }
            else
            {
                Log.Inform("Could not start server because of errors.");
            }

            while (IsAlive)
            {
                AcceptDone.Reset();

                Listener.BeginAcceptSocket(new AsyncCallback(OnAcceptSocket), null);

                AcceptDone.WaitOne();
            }

            CenterClient[] remainingServers = Clients.ToArray();

            foreach (CenterClient server in remainingServers)
            {
                server.Stop();
            }

            Dispose();

            Log.SkipLine();
            Log.Warn("Server stopped.");

            Console.Read();
        }

        private static void OnAcceptSocket(IAsyncResult ar)
        {
            AcceptDone.Set();

            var centerClient = new CenterClient(Listener.EndAcceptSocket(ar));
        }

        public static void Stop()
        {
            IsAlive = false;
        }

        private static void Dispose()
        {
            Listener?.Stop();

            Log.SkipLine();
            Log.Inform("Server disposed from thread {0}.", Thread.CurrentThread.ManagedThreadId);
        }
    }
}

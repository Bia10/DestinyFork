using System.Diagnostics;

using Destiny.Data;
using Destiny.IO;
using Destiny.Maple.Commands;

namespace Destiny.Maple.Data
{
    public static class DataProvider
    {
        public static bool IsInitialized { get; private set; }

        public static AvailableStyles Styles { get; private set; }
        public static CachedItems Items { get; private set; }
        public static CachedSkills Skills { get; private set; }
        public static CachedMobs Mobs { get; private set; }
        public static CachedReactors Reactors { get; private set; }
        public static CachedQuests Quests { get; private set; }
        public static CreationData CreationData { get; private set; }
        public static CachedMaps Maps { get; private set; }

        public static void Initialize()
        {
            using (Database.TemporarySchema(Database.SchemaMCDB))
            {
                IsInitialized = false;

                Styles?.Skins.Clear();
                Styles?.MaleHairs.Clear();
                Styles?.MaleFaces.Clear();
                Styles?.FemaleHairs.Clear();
                Styles?.FemaleFaces.Clear();

                Items?.Clear();

                Skills?.Clear();

                Mobs?.Clear();

                Maps?.Clear();

                Quests?.Clear();

                Database.Test();

                Stopwatch sw = new Stopwatch();

                sw.Start();

                Log.Inform("Loading data...");

                Styles = new AvailableStyles();
                Items = new CachedItems();
                Skills = new CachedSkills();
                Mobs = new CachedMobs();
                Reactors = new CachedReactors();
                Quests = new CachedQuests();
                CreationData = new CreationData();
                Maps = new CachedMaps();

                CommandFactory.Initialize();

                sw.Stop();
                
                Log.SkipLine();
                Log.Success("Maple data loaded in {0}ms.", sw.ElapsedMilliseconds);
                Log.SkipLine();

                IsInitialized = true;
            }
        }
    }
}

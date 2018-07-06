using System.Collections.ObjectModel;

using Destiny.IO;
using Destiny.Constants;

namespace Destiny.Maple
{
    public sealed class World : KeyedCollection<byte, Channel>
    {
        public byte ID { get; private set; }
        public string Name { get; private set; }
        public ushort Port { get; private set; }
        public ushort ShopPort { get; private set; }
        public byte Channels { get; private set; }
        public WorldConstants.WorldFlag Flag { get; private set; }
        public string EventMessage { get; private set; }
        public string TickerMessage { get; private set; }
        public bool AllowMultiLeveling { get; private set; }
        public int DefaultCreationSlots { get; private set; }
        public bool DisableCharacterCreation { get; private set; }
        public int ExperienceRate { get; private set; }
        public int QuestExperienceRate { get; set; }
        public int PartyQuestExperienceRate { get; set; }
        public int MesoRate { get; set; }
        public int DropRate { get; set; }

        // NOTE: Unless there's a max amount of users set, this is useless.
        public WorldConstants.WorldStatus Status
        {
            get
            {
                return WorldConstants.WorldStatus.Normal;
            }
        }

        public int Population
        {
            get
            {
                int population = 0;

                foreach (Channel loopChannel in this)
                {
                    population += loopChannel.Population;
                }

                return population;
            }
        }


        public Channel RandomChannel
        {
            get
            {
                return this[Application.Random.Next(Count)];
            }
        }

        public World(byte id)
        {
            ID = id;

            string configSection = "World" + ID.ToString();

            // TODO: Get the hardcoded values from config settings

            Name = Settings.GetString(configSection + "/Name");
            Port = (ushort)(8585 + 100 * ID);
            ShopPort = 9000;
            Channels = Settings.GetByte(configSection + "/Channels");
            Flag = Settings.GetEnum<WorldConstants.WorldFlag>(configSection + "/Flag");
            EventMessage = Settings.GetString(configSection + "/EventMessage");
            TickerMessage = Settings.GetString(configSection + "/TickerMessage");
            AllowMultiLeveling = true;
            DefaultCreationSlots = 3;
            DisableCharacterCreation = false;
            ExperienceRate = Settings.GetInt(configSection + "/ExperienceRate");
            QuestExperienceRate = Settings.GetInt(configSection + "/QuestExperienceRate");
            PartyQuestExperienceRate = Settings.GetInt(configSection + "/PartyQuestExperienceRate");
            MesoRate = Settings.GetInt(configSection + "/MesoDropRate");
            DropRate = Settings.GetInt(configSection + "/ItemDropRate");
        }

        protected override void InsertItem(int index, Channel item)
        {
            base.InsertItem(index, item);

            Log.SkipLine();
            Log.Success("Registered Channel ({0}-{1}).", Name, item.ID + 1);
            Log.SkipLine();
        }

        protected override void RemoveItem(int index)
        {
            Channel item = Items[index];

            base.RemoveItem(index);

            foreach (Channel loopChannel in this)
            {
                if (loopChannel.ID > index)
                {
                    loopChannel.ID--;
                }
            }

            Log.SkipLine();
            Log.Warn("Unregistered Channel {0}-{1}.", Name, item.ID);
            Log.SkipLine();
        }

        protected override byte GetKeyForItem(Channel item)
        {
            return item.ID;
        }
    }
}

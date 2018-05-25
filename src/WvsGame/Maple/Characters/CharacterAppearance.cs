using System.Collections.ObjectModel;

using Destiny.Constants;
using Destiny.Maple.Data;
using Destiny.Network.PacketFactory;

namespace Destiny.Maple.Characters
{
    public class CharacterAppearance : KeyedCollection<int, CharacterAppearance>
    {
        public Character Parent { get; }

        public CharacterAppearance(Character parent) : base()
        {
            Parent = parent;
        }

        #region Gender
        private CharacterConstants.Gender gender;
        private void SetGenderTo(CharacterConstants.Gender value)
        {
            gender = value;

            if (!Parent.IsInitialized) return;

            Parent.Client.Send(CharacterPackets.SetGenderPacket(gender));
        }

        public CharacterConstants.Gender Gender
        {
            get { return gender; }
            set { SetGenderTo(value);  }
        }

        public static void ChangeGender(Character character, CharacterConstants.Gender newGender)
        {
            character?.Appearance.SetGenderTo(newGender);
        }
        #endregion

        #region Skin
        private byte skin;
        private void SetSkinTo(byte value)
        {
            if (!DataProvider.Styles.Skins.Contains(value))
            {
                throw new StyleUnavailableException();
            }

            skin = value;

            if (!Parent.IsInitialized) return;

            CharacterStats.Update(Parent, CharacterConstants.StatisticType.Skin);
            Parent.Map.Broadcast(CharacterPackets.UpdateApperancePacket(Parent));
        }

        public byte Skin
        {
            get { return skin; }
            set { SetSkinTo(value); }
        }

        public static void ChangeSkin(Character character, byte newSkin)
        {
            character?.Appearance.SetSkinTo(newSkin);
        }
        #endregion

        #region Face
        private int face;
        private void SetFaceTo(int value)
        {
            if (Gender == CharacterConstants.Gender.Male
                && !DataProvider.Styles.MaleFaces.Contains(value) || Gender == CharacterConstants.Gender.Female && !DataProvider.Styles.FemaleFaces.Contains(value))
            {
                throw new StyleUnavailableException();
            }

            face = value;

            if (!Parent.IsInitialized) return;

            CharacterStats.Update(Parent, CharacterConstants.StatisticType.Face);
            Parent.Map.Broadcast(CharacterPackets.UpdateApperancePacket(Parent));
        }

        public int Face
        {
            get { return face; }
            set { SetFaceTo(value); }
        }

        public int FaceStyleOffset
        {
            get
            {
                return (Face - (10 * (Face / 10))) + (Gender == CharacterConstants.Gender.Male ? 20000 : 21000);
            }
        }

        public int FaceColorOffset
        {
            get { return ((Face / 100) - (10 * (Face / 1000))) * 100; }
        }

        public static void ChangeFace(Character character, int newFace)
        {
            character?.Appearance.SetFaceTo(newFace);
        }
        #endregion

        #region Hair
        private int hair;
        private void SetHairTo(int value)
        {
            if (Gender == CharacterConstants.Gender.Male
                && !DataProvider.Styles.MaleHairs.Contains(value) || Gender == CharacterConstants.Gender.Female && !DataProvider.Styles.FemaleHairs.Contains(value))
            {
                throw new StyleUnavailableException();
            }

            hair = value;

            if (!Parent.IsInitialized) return;

            CharacterStats.Update(Parent, CharacterConstants.StatisticType.Hair);
            Parent.Map.Broadcast(CharacterPackets.UpdateApperancePacket(Parent));
        }

        public int Hair
        {
            get { return hair; }
            set { SetHairTo(value); }
        }

        public int HairStyleOffset
        {
            get { return (Hair / 10) * 10; }
        }

        public int HairColorOffset
        {
            get { return Hair - (10 * (Hair / 10)); }
        }

        public static void ChangeHair(Character character, int newHair)
        {
            character?.Appearance.SetHairTo(newHair);
        }
        #endregion

        public static void UpdateApperance(Character character)
        {
            character.Map.Broadcast(CharacterPackets.UpdateApperancePacket(character), character);
        }

        protected override int GetKeyForItem(CharacterAppearance item)
        {
            throw new System.NotImplementedException();
        }
    }
}
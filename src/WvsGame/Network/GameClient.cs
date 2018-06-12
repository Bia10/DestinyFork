using System.Net.Sockets;

using Destiny.Maple;
using Destiny.Maple.Characters;
using Destiny.Constants;
using Destiny.IO;
using Destiny.Maple.Maps;
using Destiny.Network.ClientHandler;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Network
{
    public sealed class GameClient : MapleClientHandler.MapleClientHandler
    {
        public long ID { get; }

        public GameAccount Account { get; set; }
        public Character Character { get; set; }

        public GameClient(Socket socket) : base(socket)
        {
            ID = Application.Random.Next();
        }

        protected override bool IsServerAlive
        {
            get
            {
                return WvsGame.IsAlive;
            }
        }

        private void Initialize(Packet inPacket)
        {
            int accountID;
            int characterID = inPacket.ReadInt();

            if ((accountID = WvsGame.CenterConnection.ValidateMigration(RemoteEndPoint.Address.ToString(), characterID)) == 0)
            {
                Stop();

                return;
            }

            Character = new Character(characterID, this);
            Character.Load();
            Character.InitializeCharacter(Character);

            Title = Character.Name;
        }

        protected override void Register()
        {
            WvsGame.Clients.Add(this);
        }

        protected override void Terminate()
        {
            if (Character == null) return;

            Character.Save();
            Character.LastNpc = null;
            Character.Map.Characters.Remove(Character);
        }

        protected override void Unregister()
        {
            WvsGame.Clients.Remove(this);
        }

        protected override void Dispatch(Packet inPacket)
        {
            switch ((ClientOperationCode)inPacket.OperationCode)
            {
                case ClientOperationCode.CharacterLoad:
                    Initialize(inPacket);
                    break;

                case ClientOperationCode.MapChange:
                    Character.ChangeMapHandler(inPacket);
                    break;

                case ClientOperationCode.ChannelChange:
                    ChangeChannel(inPacket);
                    break;

                case ClientOperationCode.PlayerMovement:
                    Character.CharMoveHandler(inPacket);
                    break;

                case ClientOperationCode.Sit:
                    Character.CharSitHandler(inPacket);
                    break;

                case ClientOperationCode.UseChair:
                    Character.CharSitOnChairHandler(inPacket);
                    break;

                case ClientOperationCode.CloseRangeAttack:
                    Character.AttackHandler(inPacket, CharacterConstants.AttackType.Melee);
                    break;

                case ClientOperationCode.RangedAttack:
                    Character.AttackHandler(inPacket, CharacterConstants.AttackType.Range);
                    break;

                case ClientOperationCode.MagicAttack:
                    Character.AttackHandler(inPacket, CharacterConstants.AttackType.Magic);
                    break;

                case ClientOperationCode.TakeDamage:
                    Character.DamageHandler(inPacket);
                    break;

                case ClientOperationCode.PlayerChat:
                    Character.CharTalkHandler(inPacket);
                    break;

                case ClientOperationCode.CloseChalkboard:
                    Character.Chalkboard = string.Empty;
                    break;

                case ClientOperationCode.FaceExpression:
                    Character.CharExpressionHandler(inPacket);
                    break;

                case ClientOperationCode.NpcConverse:
                    Character.Converse(inPacket);
                    break;

                case ClientOperationCode.NpcResult:
                    Character.LastNpc.Handle(Character, inPacket);
                    break;

                case ClientOperationCode.NpcShop:
                    Character.LastNpc.Shop.Handle(Character, inPacket);
                    break;

                case ClientOperationCode.Storage:
                    Character.Storage.CharStorageHandler(inPacket);
                    break;

                case ClientOperationCode.InventorySort:
                    Character.Items.SortItemsHandler(inPacket);
                    break;

                case ClientOperationCode.InventoryGather:
                    Character.Items.GatherItemsHandler(inPacket);
                    break;

                case ClientOperationCode.InventoryAction:
                    Character.Items.CharItemsHandler(inPacket);
                    break;

                case ClientOperationCode.UseItem:
                    Character.Items.UseItemHandler(inPacket);
                    break;

                case ClientOperationCode.UseSummonBag:
                    Character.Items.UseSummonBagHandler(inPacket);
                    break;

                case ClientOperationCode.UseCashItem:
                    Character.Items.UseCashItemHandler(inPacket);
                    break;

                case ClientOperationCode.UseTeleportRock: // NOTE: Only occurs with the special Teleport Rock in the usable inventory.
                    Character.Trocks.UseTrockHandler(2320000, inPacket);
                    break;

                case ClientOperationCode.UseReturnScroll:
                    Character.Items.UseReturnScrollHandler(inPacket);
                    break;

                case ClientOperationCode.DistributeAP:
                    Character.Stats.CharDistributeAPHandler(inPacket);
                    break;

                case ClientOperationCode.AutoDistributeAP:
                    Character.Stats.AutoDistributeAP(inPacket);
                    break;

                case ClientOperationCode.HealOverTime:
                    Character.Stats.HealOverTime(inPacket);
                    break;

                case ClientOperationCode.DistributeSP:
                    Character.Stats.DistributeSPHandler(inPacket);
                    break;

                case ClientOperationCode.UseSkill:
                    Character.Skills.CastSkillHandler(inPacket);
                    break;

                case ClientOperationCode.CancelBuff:
                    Character.Buffs.CancelBuffHandler(inPacket);
                    break;

                case ClientOperationCode.MesoDrop:
                    Character.Stats.DropMesoHandler(inPacket);
                    break;

                case ClientOperationCode.PlayerInformation:
                    Character.InformOnCharacter(inPacket);
                    break;

                case ClientOperationCode.ChangeMapSpecial:
                    Character.EnterPortal(inPacket);
                    break;

                case ClientOperationCode.TrockAction:
                    Character.Trocks.UpdateTrockHandler(inPacket);
                    break;

                case ClientOperationCode.Report:
                    Character.Report(inPacket);
                    break;

                case ClientOperationCode.QuestAction:
                    Character.Quests.Handle(inPacket);
                    break;

                case ClientOperationCode.MultiChat:
                    Character.MultiTalkHandler(inPacket);
                    break;

                case ClientOperationCode.Command:
                    Character.UseCommand(inPacket);
                    break;

                case ClientOperationCode.PlayerInteraction:
                    Character.Interact(inPacket);
                    break;

                case ClientOperationCode.AdminCommand:
                    Character.UseAdminCommandHandler(inPacket);
                    break;

                case ClientOperationCode.NoteAction:
                    Character.Memos.Handle(inPacket);
                    break;

                case ClientOperationCode.ChangeKeymap:
                    Character.Keymap.Change(inPacket);
                    break;

                case ClientOperationCode.MovePet:
                    //Character.Pets.Move(inPacket);
                    break;

                case ClientOperationCode.MobMovement:
                    Character.ControlledMobs.MoveHandler(inPacket);
                    break;

                case ClientOperationCode.DropPickup:
                    Character.Items.PickupItemHandler(inPacket);
                    break;

                case ClientOperationCode.NpcMovement:
                    Character.ControlledNpcs.Move(inPacket);
                    break;

                case ClientOperationCode.HitReactor:
                    Character.Map.Reactors.Hit(inPacket, Character);
                    break;

                case ClientOperationCode.TouchReactor:
                    MapReactors.Touch(inPacket, Character);
                    break;

                default:
                    Log.SkipLine();
                    Log.Warn(" Unhandled ClientOperationCode encountered!" +
                             " \n Argument: {0}", inPacket.OperationCode);
                    Log.SkipLine();
                    break;
            }
        }

        private void ChangeChannel(Packet inPacket)
        {
            byte channelID = inPacket.ReadByte();

            using (Packet outPacket = new Packet(ServerOperationCode.MigrateCommand))
            {
                outPacket.WriteBool(true);
                outPacket.WriteBytes(127, 0, 0, 1);
                outPacket.WriteUShort(WvsGame.CenterConnection.GetChannelPort(channelID));

                Send(outPacket);
            }
        }
    }
}

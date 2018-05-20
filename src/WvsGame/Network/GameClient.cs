﻿using System.Net.Sockets;

using Destiny.Maple;
using Destiny.Maple.Characters;
using Destiny.Constants;
using Destiny.Network.ClientHandler;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Network
{
    public sealed class GameClient : MapleClientHandler.MapleClientHandler
    {
        public long ID { get; private set; }

        public Account Account { get; set; }
        public Character Character { get; set; }

        public GameClient(Socket socket)
            : base(socket)
        {
            this.ID = Application.Random.Next();
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

            if ((accountID = WvsGame.CenterConnection.ValidateMigration(this.RemoteEndPoint.Address.ToString(), characterID)) == 0)
            {
                this.Stop();

                return;
            }

            this.Character = new Character(characterID, this);
            this.Character.Load();
            Character.InitializeCharacter(this.Character);

            this.Title = this.Character.Name;
        }

        protected override void Register()
        {
            WvsGame.Clients.Add(this);
        }

        protected override void Terminate()
        {
            if (this.Character != null)
            {
                this.Character.Save();
                this.Character.LastNpc = null;
                this.Character.Map.Characters.Remove(this.Character);
            }
        }

        protected override void Unregister()
        {
            WvsGame.Clients.Remove(this);
        }

        protected override void Dispatch(Packet iPacket)
        {
            switch ((ClientOperationCode)iPacket.OperationCode)
            {
                case ClientOperationCode.CharacterLoad:
                    this.Initialize(iPacket);
                    break;

                case ClientOperationCode.MapChange:
                    this.Character.ChangeMapHandler(iPacket);
                    break;

                case ClientOperationCode.ChannelChange:
                    this.ChangeChannel(iPacket);
                    break;

                case ClientOperationCode.PlayerMovement:
                    this.Character.CharMoveHandler(iPacket);
                    break;

                case ClientOperationCode.Sit:
                    this.Character.CharSitHandler(iPacket);
                    break;

                case ClientOperationCode.UseChair:
                    this.Character.CharSitOnChairHandler(iPacket);
                    break;

                case ClientOperationCode.CloseRangeAttack:
                    this.Character.Attack(iPacket, CharacterConstants.AttackType.Melee);
                    break;

                case ClientOperationCode.RangedAttack:
                    this.Character.Attack(iPacket, CharacterConstants.AttackType.Range);
                    break;

                case ClientOperationCode.MagicAttack:
                    this.Character.Attack(iPacket, CharacterConstants.AttackType.Magic);
                    break;

                case ClientOperationCode.TakeDamage:
                    this.Character.DamageHandler(iPacket);
                    break;

                case ClientOperationCode.PlayerChat:
                    this.Character.CharTalkHandler(iPacket);
                    break;

                case ClientOperationCode.CloseChalkboard:
                    this.Character.Chalkboard = string.Empty;
                    break;

                case ClientOperationCode.FaceExpression:
                    this.Character.CharExpressionHandler(iPacket);
                    break;

                case ClientOperationCode.NpcConverse:
                    this.Character.Converse(iPacket);
                    break;

                case ClientOperationCode.NpcResult:
                    this.Character.LastNpc.Handle(this.Character, iPacket);
                    break;

                case ClientOperationCode.NpcShop:
                    this.Character.LastNpc.Shop.Handle(this.Character, iPacket);
                    break;

                case ClientOperationCode.Storage:
                    this.Character.Storage.CharStorageHandler(iPacket);
                    break;

                case ClientOperationCode.InventorySort:
                    this.Character.Items.SortItemsHandler(iPacket);
                    break;

                case ClientOperationCode.InventoryGather:
                    this.Character.Items.GatherItemsHandler(iPacket);
                    break;

                case ClientOperationCode.InventoryAction:
                    this.Character.Items.CharItemsHandler(iPacket);
                    break;

                case ClientOperationCode.UseItem:
                    this.Character.Items.UseItemHandler(iPacket);
                    break;

                case ClientOperationCode.UseSummonBag:
                    this.Character.Items.UseSummonBagHandler(iPacket);
                    break;

                case ClientOperationCode.UseCashItem:
                    this.Character.Items.UseCashItemHandler(iPacket);
                    break;

                case ClientOperationCode.UseTeleportRock: // NOTE: Only occurs with the special Teleport Rock in the usable inventory.
                    this.Character.Trocks.UseTrockHandler(2320000, iPacket);
                    break;

                case ClientOperationCode.UseReturnScroll:
                    this.Character.Items.UseReturnScrollHandler(iPacket);
                    break;

                case ClientOperationCode.DistributeAP:
                    this.Character.CharDistributeAPHandler(iPacket);
                    break;

                case ClientOperationCode.AutoDistributeAP:
                    this.Character.AutoDistributeAP(iPacket);
                    break;

                case ClientOperationCode.HealOverTime:
                    this.Character.HealOverTime(iPacket);
                    break;

                case ClientOperationCode.DistributeSP:
                    this.Character.DistributeSPHandler(iPacket);
                    break;

                case ClientOperationCode.UseSkill:
                    this.Character.Skills.CastSkillHandler(iPacket);
                    break;

                case ClientOperationCode.CancelBuff:
                    this.Character.Buffs.CancelBuffHandler(iPacket);
                    break;

                case ClientOperationCode.MesoDrop:
                    this.Character.DropMesoHandler(iPacket);
                    break;

                case ClientOperationCode.PlayerInformation:
                    this.Character.InformOnCharacter(iPacket);
                    break;

                case ClientOperationCode.ChangeMapSpecial:
                    this.Character.EnterPortal(iPacket);
                    break;

                case ClientOperationCode.TrockAction:
                    this.Character.Trocks.Update(iPacket);
                    break;

                case ClientOperationCode.Report:
                    this.Character.Report(iPacket);
                    break;

                case ClientOperationCode.QuestAction:
                    this.Character.Quests.Handle(iPacket);
                    break;

                case ClientOperationCode.MultiChat:
                    this.Character.MultiTalkHandler(iPacket);
                    break;

                case ClientOperationCode.Command:
                    this.Character.UseCommand(iPacket);
                    break;

                case ClientOperationCode.PlayerInteraction:
                    this.Character.Interact(iPacket);
                    break;

                case ClientOperationCode.AdminCommand:
                    this.Character.UseAdminCommand(iPacket);
                    break;

                case ClientOperationCode.NoteAction:
                    this.Character.Memos.Handle(iPacket);
                    break;

                case ClientOperationCode.ChangeKeymap:
                    this.Character.Keymap.Change(iPacket);
                    break;

                case ClientOperationCode.MovePet:
                    //this.Character.Pets.Move(iPacket);
                    break;

                case ClientOperationCode.MobMovement:
                    this.Character.ControlledMobs.Move(iPacket);
                    break;

                case ClientOperationCode.DropPickup:
                    this.Character.Items.PickupItemHandler(iPacket);
                    break;

                case ClientOperationCode.NpcMovement:
                    this.Character.ControlledNpcs.Move(iPacket);
                    break;

                case ClientOperationCode.HitReactor:
                    this.Character.Map.Reactors.Hit(iPacket, this.Character);
                    break;

                case ClientOperationCode.TouchReactor:
                    this.Character.Map.Reactors.Touch(iPacket, this.Character);
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

                this.Send(outPacket);
            }
        }
    }
}

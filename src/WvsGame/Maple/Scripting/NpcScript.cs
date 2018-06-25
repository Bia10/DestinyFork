using System;

using Destiny.Maple.Characters;
using Destiny.Maple.Life;
using Destiny.Threading;
using Destiny.Constants;
using Destiny.Network.Common;

namespace Destiny.Maple.Scripting
{
    public sealed class NpcScript : ScriptBase
    {
        private readonly Npc mNpc;
        private string mText;
        private WaitableResult<int> mResult;

        public NpcScript(Npc npc, Character character)
            : base(ServerConstants.ScriptType.Npc, npc.MapleID.ToString(), character, true) // TODO: Use actual npc script instead of ID.
        {
            mNpc = npc;

            Expose("answer_no", 0);
            Expose("answer_yes", 1);

            Expose("answer_decline", 0);
            Expose("answer_accept", 1);

            Expose("quiz_npc", 0);
            Expose("quiz_mob", 1);
            Expose("quiz_item", 2);

            Expose("addText", new Action<string>(AddText));
            Expose("sendOk", new Func<int>(SendOk));
            Expose("sendNext", new Func<int>(SendNext));
            Expose("sendBackNext", new Func<int>(SendBackNext));
            Expose("sendBackOk", new Func<int>(SendBackOk));
            Expose("askYesNo", new Func<int>(AskYesNo));
            Expose("askAcceptDecline", new Func<int>(AskAcceptDecline));
        }

        public void SetResult(int value)
        {
            mResult.Set(value);
        }

        private void AddText(string text)
        {
            mText += text;
        }

        private int SendOk()
        {
            mResult = new WaitableResult<int>();

            using (Packet oPacket = mNpc.GetDialogPacket(mText, NPCsConstants.NpcMessageType.Standard, 0, 0))
            {
                mCharacter.Client.Send(oPacket);
            }

            mText = string.Empty;

            mResult.Wait();

            return mResult.Value;
        }

        private int SendNext()
        {
            mResult = new WaitableResult<int>();

            using (Packet oPacket = mNpc.GetDialogPacket(mText, NPCsConstants.NpcMessageType.Standard, 0, 1))
            {
                mCharacter.Client.Send(oPacket);
            }

            mText = string.Empty;

            mResult.Wait();

            return mResult.Value;
        }

        private int SendBackOk()
        {
            mResult = new WaitableResult<int>();

            using (Packet oPacket = mNpc.GetDialogPacket(mText, NPCsConstants.NpcMessageType.Standard, 1, 0))
            {
                mCharacter.Client.Send(oPacket);
            }

            mText = string.Empty;

            mResult.Wait();

            return mResult.Value;
        }

        private int SendBackNext()
        {
            mResult = new WaitableResult<int>();

            using (Packet oPacket = mNpc.GetDialogPacket(mText, NPCsConstants.NpcMessageType.Standard, 1, 1))
            {
                mCharacter.Client.Send(oPacket);
            }

            mText = string.Empty;

            mResult.Wait();

            return mResult.Value;
        }

        private int AskYesNo()
        {
            mResult = new WaitableResult<int>();

            using (Packet oPacket = mNpc.GetDialogPacket(mText, NPCsConstants.NpcMessageType.YesNo))
            {
                mCharacter.Client.Send(oPacket);
            }

            mText = string.Empty;

            mResult.Wait();

            return mResult.Value;
        }

        private int AskAcceptDecline()
        {
            mResult = new WaitableResult<int>();

            using (Packet oPacket = mNpc.GetDialogPacket(mText, NPCsConstants.NpcMessageType.AcceptDecline))
            {
                mCharacter.Client.Send(oPacket);
            }

            mText = string.Empty;

            mResult.Wait();

            return mResult.Value;
        }

        private void AskChoice()
        {

        }
    }
}

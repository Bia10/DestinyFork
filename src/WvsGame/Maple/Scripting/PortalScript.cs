using System;

using Destiny.Maple.Characters;
using Destiny.Maple.Maps;
using Destiny.Constants;

namespace Destiny.Maple.Scripting
{
    public sealed class PortalScript : ScriptBase
    {
        private Portal mPortal;

        public PortalScript(Portal portal, Character character) : base(ServerConstants.ScriptType.Portal, portal.Script, character, false)
        {
            mPortal = portal;

            Expose("playPortalSe", new Action(PlayPortalSoundEffect));
        }

        private void PlayPortalSoundEffect()
        {
            CharacterBuffs.ShowLocalUserEffect(mCharacter, CharacterConstants.UserEffect.PlayPortalSE);
        }
    }
}
﻿using Destiny.Maple.Characters;
using Destiny.Maple.Maps;
using System;
using Destiny.Constants;

namespace Destiny.Maple.Scripting
{
    public sealed class PortalScript : ScriptBase
    {
        private Portal mPortal;

        public PortalScript(Portal portal, Character character)
            : base(ServerConstants.ScriptType.Portal, portal.Script, character, false)
        {
            mPortal = portal;

            this.Expose("playPortalSe", new Action(this.PlayPortalSoundEffect));
        }

        private void PlayPortalSoundEffect()
        {
            CharacterBuffs.ShowLocalUserEffect(mCharacter, CharacterConstants.UserEffect.PlayPortalSE);
        }
    }
}
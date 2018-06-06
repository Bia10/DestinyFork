﻿using Destiny.Constants;
using Destiny.Maple.Characters;
using Destiny.Scripting;

namespace Destiny.Maple.Scripting
{
    public abstract class ScriptBase : LuaScriptable
    {
        protected readonly Character mCharacter;

        protected ScriptBase(ServerConstants.ScriptType type, string name, Character character, bool useThread)
            : base(string.Format(Application.ExecutablePath + @"..\..\scripts\{0}\{1}.lua", type.ToString().ToLower(), name), useThread)
        {
            mCharacter = character;
        }
    }
}

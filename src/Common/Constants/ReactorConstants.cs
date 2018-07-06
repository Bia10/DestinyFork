using System;

namespace Destiny.Constants
{
    public static class ReactorConstants
    {
        #region Reactors
        public enum ReactorEventType
        {
            PlainAdvanceState,
            HitFromLeft,
            HitFromRight,
            HitBySkill,
            NoClue, //NOTE: Applies to activate_by_touch reactors
            NoClue2, //NOTE: Applies to activate_by_touch reactors
            HitByItem,
            Timeout = 101
        }

        [Flags]
        public enum ReactorFlags : byte
        {
            //TODO: Test this; I'm just guessing
            FacesLeft = 0x01,
            ActivateByTouch = 0x02,
            RemoveInFieldSet = 0x04
        }
        #endregion
    }
}

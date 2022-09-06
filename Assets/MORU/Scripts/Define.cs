using System.Collections;
using System.Collections.Generic;

namespace Moru
{
    public static class Define
    {
        public enum Slot { Q = 0, W, E, A, S, D, MAX }
        public const string Path = "";


        public enum UnitViewMode { ILLUSTMODE = 0, SPINEMODE }
        public static UnitViewMode unitViewMode = UnitViewMode.SPINEMODE;

        public enum MasteryLevel { L0,L1, L2, L3, L4 ,MAX}
        public enum MasteryType { Character, Utillity}
    }


}
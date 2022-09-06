using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            protected abstract class PFreezing : PassiveSkill
            {
                public PFreezing(PFreezingData data, Unit skillOwner) : base(data, skillOwner)
                {
                    restrictionFlag = RESTRICTION_FLAG.MOVE | RESTRICTION_FLAG.ATTACK;
                }
                protected override int GetTotalDamage(Unit victim) { throw new System.NotImplementedException(); }
            }

            protected abstract class PFreezingData : PassiveSkillData
            {
                protected PFreezingData()
                {
#if UNITY_EDITOR
                    ShowPPSetting = false;
                    PP_UseAreaStack = false;
                    PP_UseDefaultTarget = false;
                    PP_UseInRangeHitboxes = false;
                    PP_UseRange = false;
                    PP_UseRemainTimeType = true;
                    PP_UseRestrictionFlag = false;
                    PP_UseTick = false;
#endif
                }
            }
        }
    }
}
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            protected abstract class PBurn : PassiveSkill
            {
                protected int dmgPerTick;

                public PBurn(PBurnData data, Unit skillOwner) : base(data, skillOwner)
                {
                    dmgPerTick = data.dmgPerTick;
                }
                protected override void OnTickPassiveEffect(Unit unit, int tickStack)
                {
                    base.OnTickPassiveEffect(unit, tickStack);
                    Attack(unit, DAMAGE_TYPE.DOT, unit.transform.position);
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return dmgPerTick;
                }
            }

            protected abstract class PBurnData : PassiveSkillData
            {
                [BoxGroup("화상", true, true)]
                [LabelText("틱 당 피해량")]
                public int dmgPerTick;

                protected PBurnData()
                {
#if UNITY_EDITOR
                    ShowPPSetting = false;
                    PP_UseAreaStack = false;
                    PP_UseDefaultTarget = false;
                    PP_UseInRangeHitboxes = false;
                    PP_UseRange = false;
                    PP_UseRemainTimeType = true;
                    PP_UseRestrictionFlag = false;
                    PP_UseTick = true;
#endif
                }
            }
        }
    }
}
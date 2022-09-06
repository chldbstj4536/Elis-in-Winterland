using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            protected abstract class PSlow : PassiveSkill
            {
                protected float slowRate;

                public PSlow(PSlowData data, Unit skillOwner) : base(data, skillOwner)
                {
                    slowRate = data.slowRate;
                }

                protected override void OnBeginPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnBeginPassiveEffect(unit, areaStack);

                    unit.mc.MoveSpeedCoef *= slowRate;
                }
                protected override void OnEndPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnEndPassiveEffect(unit, areaStack);

                    unit.mc.MoveSpeedCoef /= slowRate;
                }
                protected override int GetTotalDamage(Unit victim) { throw new System.NotImplementedException(); }
            }

            protected abstract class PSlowData : PassiveSkillData
            {
                [BoxGroup("느려짐", true, true)]
                [LabelText("이동속도 감소량")]
                public float slowRate;

                protected PSlowData()
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
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            protected abstract class PExhausted : PassiveSkill
            {
                protected float moveSpeedSlowAmount;
                protected float atkSpeedSlowAmount;

                public PExhausted(PExhaustedData data, Unit skillOwner) : base(data, skillOwner)
                {
                    moveSpeedSlowAmount = data.moveSpeedSlowAmount;
                    atkSpeedSlowAmount = data.atkSpeedSlowAmount;
                }

                protected override void OnBeginPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnBeginPassiveEffect(unit, areaStack);

                    unit.mc.MoveSpeedCoef -= moveSpeedSlowAmount;
                    unit.AttackSpeedCoef -= atkSpeedSlowAmount;
                }
                protected override void OnEndPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnEndPassiveEffect(unit, areaStack);

                    unit.mc.MoveSpeedCoef += moveSpeedSlowAmount;
                    unit.AttackSpeedCoef += atkSpeedSlowAmount;
                }
            }

            protected abstract class PExhaustedData : PassiveSkillData
            {
                [BoxGroup("Ż��", true, true)]
                [LabelText("�̵��ӵ� ���ҷ�")]
                public float moveSpeedSlowAmount;
                [BoxGroup("Ż��")]
                [LabelText("���ݼӵ� ���ҷ�")]
                public float atkSpeedSlowAmount;

                protected PExhaustedData()
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
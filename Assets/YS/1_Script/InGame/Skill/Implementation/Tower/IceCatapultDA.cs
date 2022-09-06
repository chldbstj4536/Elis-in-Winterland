using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            /// <summary>
            /// 스킬에 필요한 변수 및 작동 방식만 정의
            /// (초기화는 ASKILL_NAME_DATA에서 처리)
            /// </summary>
            private class AIceCatapultDA : VectorSkill
            {
                #region Field
                private readonly BezierProjectile projectilePrefab;
                private readonly int physxDmg;
                #endregion

                public AIceCatapultDA(AIceCatapultDAData data, Unit skillOwner) : base(data, skillOwner)
                {
                    projectilePrefab = data.projectilePrefab.Instantiate(false) as BezierProjectile;
                    projectilePrefab.Speed = data.projectileSpeed;
                    physxDmg = data.physxDmg;
                }

                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    Trigger();

                    CooltimeStart();
                }
                protected override void Trigger()
                {
                    base.Trigger();

                    var p = projectilePrefab.Instantiate(true);
                    p.SetProjectile(this, caster.transform.position, dest, TargetLayerMask, TargetType);
                    p.SetAttackOnArrive(GetTotalDamage, DAMAGE_TYPE.NORMAL, TargetLayerMask, TargetType);
                }
                protected override void SetRange()
                {
                    base.SetRange();
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return PhysicsDamageCalc(physxDmg, caster.TotalArmorPnt, victim.TotalArmor);
                }
            }
            protected class AIceCatapultDAData : VectorSkillData
            {
                [BoxGroup("얼음투석기 기본공격", true, true)]
                [LabelText("곡사형 투사체 프리팹"), Tooltip("공격 시 생성할 투사체 프리팹")]
                public BezierProjectile projectilePrefab;
                [BoxGroup("얼음투석기 기본공격"), Min(0.0001f)]
                [LabelText("곡사형 투사체 속도"), Tooltip("투사체의 속도")]
                public float projectileSpeed = 1.0f;
                [BoxGroup("얼음투석기 기본공격"), Min(0.0001f)]
                [LabelText("물리피해량"), Tooltip("투사체가 입히는 물리피해량")]
                public int physxDmg;

                public AIceCatapultDAData()
                {
                    // 액티브 스킬의 캐스팅 타입
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // 기본공격여부
                    isDefaultAttack = true;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new AIceCatapultDA(this, owner);
                }
            }
        }
    }
}
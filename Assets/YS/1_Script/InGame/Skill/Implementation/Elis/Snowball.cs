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
            private class ASnowball : VectorSkill
            {
                #region Field
                private readonly int baseDmg;
                private readonly float baseDmgMagicPowerRate;
                private readonly int maxDmg;
                private readonly float veloDmg;
                private readonly float veloScale;
                private readonly float maxScale;
                private readonly float ballSpeed;
                private readonly int maxHitCount;

                public SnowballProjectile snowballPrefab;
                public bool isEnhanced;
                #endregion

                public ASnowball(ASnowballData data, Unit skillOwner) : base(data, skillOwner)
                {
                    baseDmg = data.baseDmg;
                    baseDmgMagicPowerRate = data.baseDmgMagicPowerRate;
                    maxDmg = data.maxDmg;
                    veloDmg = data.veloDmg;
                    veloScale = data.veloScale;
                    maxScale = data.maxScale;
                    ballSpeed = data.ballSpeed;
                    maxHitCount = data.maxHitCount;

                    snowballPrefab = data.projectilePrefab.Instantiate(false) as SnowballProjectile;
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

                    var snowball = PrefabPool.GetObject(snowballPrefab).GetComponent<SnowballProjectile>();

                    snowball.SetProjectile(this, caster.transform.position, dest, TargetLayerMask, TargetType);
                    snowball.Speed = ballSpeed;
                    snowball.SetAttackOnHit(null, DAMAGE_TYPE.NORMAL, TargetLayerMask, TargetType);
                    if (isEnhanced)
                    {
                        snowball.SetAttackOnArrive(null, DAMAGE_TYPE.NORMAL, TargetLayerMask, TargetType);
                        snowball.SetSnowballProjectile(maxDmg, 1.0f, 0.0f, maxDmg, 1.0f, 1.0f, 1);
                    }
                    else
                        snowball.SetSnowballProjectile(baseDmg, baseDmgMagicPowerRate, veloDmg, maxDmg, veloScale, maxScale, maxHitCount);
                }
                protected override void SetRange()
                {
                    base.SetRange();
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
            }
            protected class ASnowballData : VectorSkillData
            {
                [FoldoutGroup("스노우볼", true), Min(0)]
                [LabelText("기본 피해량"), Tooltip("스킬의 기본 피해량")]
                public int baseDmg;
                [FoldoutGroup("스노우볼"), Min(0.0f)]
                [LabelText("주문력 계수"), Tooltip("기본 피해량의 주문력 계수")]
                public float baseDmgMagicPowerRate;
                [FoldoutGroup("스노우볼"), Min(0)]
                [LabelText("최대 피해량"), Tooltip("스킬의 최대 피해량, 해당값을 초과할 수 없습니다.")]
                public int maxDmg;
                [FoldoutGroup("스노우볼"), Min(0.0f)]
                [LabelText("초당 피해 증가량"), Tooltip("매 초마다 증가되는 피해량의 곱의 값")]
                public float veloDmg;
                [FoldoutGroup("스노우볼"), Min(0.0f)]
                [LabelText("초당 크기 증가량"), Tooltip("매 초마다 증가되는 크기의 곱의 값")]
                public float veloScale;
                [FoldoutGroup("스노우볼"), Min(0.0f)]
                [LabelText("눈덩이의 최대 크기"), Tooltip("눈덩이가 최대로 커질 수 있는 한계값")]
                public float maxScale;
                [FoldoutGroup("스노우볼"), Min(0.0f)]
                [LabelText("눈덩이의 속도"), Tooltip("눈덩이 투사체의 속도와 회전속도")]
                public float ballSpeed;
                [FoldoutGroup("스노우볼"), Min(0)]
                [LabelText("피격 최대 횟수"), Tooltip("피격가능한 최대 대상 수")]
                public int maxHitCount;

                [FoldoutGroup("스노우볼")]
                [LabelText("눈덩이 투사체 프리팹"), Tooltip("눈덩이 투사체 프리팹")]
                public SnowballProjectile projectilePrefab;

                public ASnowballData()
                {
                    // 액티브 스킬의 캐스팅 타입
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // 기본공격여부
                    isDefaultAttack = false;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new ASnowball(this, owner);
                }
            }
        }
    }
}
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class ABlizzard : VectorSkill
            {
                #region Field
                private readonly float duringTime;
                private readonly float basicPower;
                private readonly float magicPowerRate;
                private readonly float tick;
                private PBlizzardSlow pSlow;
                private BoxHitcast hitbox;
                private static readonly Vector3 HitboxOffset = new Vector3(1.0f, 0.0f, 0.0f);

                private ParticleSystem areaFX;
                private readonly AutoReleaseParticlePrefab hitFXPrefab;
                #endregion

                public ABlizzard(ABlizzardData data, Unit skillOwner) : base(data, skillOwner)
                {
                    duringTime = data.duringTime;
                    basicPower = data.basicPower;
                    magicPowerRate = data.magicPowerRate;
                    tick = data.tick;
                    pSlow = data.pSlowData.Instantiate(caster) as PBlizzardSlow;
                    hitbox = new BoxHitcast(HitboxOffset, Vector3.right, TotalRange, new Vector3(0.0f, 1.0f, GameManager.HalfSizeField_Z), Quaternion.identity);

                    areaFX = PrefabPool.GetObject(data.areaFXPrefab, false).GetComponent<ParticleSystem>();
                    hitFXPrefab = data.hitFXPrefab;
                }

                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    Trigger();
                }
                protected override void Trigger()
                {
                    base.Trigger();

                    areaFX.gameObject.SetActive(true);
                    caster.StartCoroutine(TickAttack());
                }
                protected override void SetRange()
                {
                    base.SetRange();

                    hitbox.SetRange(TotalRange);
                }
                private IEnumerator TickAttack()
                {
                    float remainDuration = duringTime;
                    Vector3 origin = caster.transform.position;
                    Quaternion rot = dest.x - caster.transform.position.x > 0.0f ? Quaternion.identity : Quaternion.AngleAxis(180.0f, Vector3.up);

                    while (remainDuration > 0.0f)
                    {
                        var hits = Utility.SweepUnit(hitbox, origin, rot, true, TargetLayerMask, TargetType);

                        foreach (var hit in hits)
                        {
                            var victim = hit.transform.GetComponent<Unit>();
                            Attack(victim, DAMAGE_TYPE.NORMAL, victim.transform.position);
                            pSlow.AddPassiveSkillToUnit(victim);
                            PrefabPool.GetObject(hitFXPrefab).transform.position = victim.transform.position;
                        }

                        yield return CachedWaitForSeconds.Get(tick);

                        remainDuration -= tick;
                    }
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return (int)(basicPower + magicPowerRate * caster.TotalMagicPower);
                }
            }
            protected class ABlizzardData : VectorSkillData
            {
                [BoxGroup("블리자드", true, true)]
                [LabelText("지속시간"), Tooltip("스킬의 사용시의 지속시간입니다")]
                public float duringTime;
                [BoxGroup("블리자드")]
                [LabelText("기본 주문공격력"), Tooltip("스킬의 기본 주문공격력입니다")]
                public int basicPower;
                [BoxGroup("블리자드")]
                [LabelText("주문력 계수"), Tooltip("스킬의 주문력 계수입니다")]
                public float magicPowerRate;
                [BoxGroup("블리자드")]
                [LabelText("피해 주기(cycle) 시간"), Tooltip("피격판정 시간단위입니다")]
                public float tick;
                [BoxGroup("블리자드/느려짐", false)]
                [HideLabel]
                public PBlizzardSlowData pSlowData;

                [BoxGroup("블리자드/이펙트", true, true)]
                [LabelText("범위 이펙트"), Tooltip("피격범위 이펙트")]
                public PoolingComponent areaFXPrefab;
                [BoxGroup("블리자드/이펙트")]
                [LabelText("피격 이펙트"), Tooltip("피격 시 이펙트")]
                public AutoReleaseParticlePrefab hitFXPrefab;

                public ABlizzardData()
                {
                    // 액티브 스킬의 캐스팅 타입
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // 기본공격여부
                    isDefaultAttack = false;
                }
#if UNITY_EDITOR
                public override void DrawGizmos(Vector3 origin, Quaternion rot)
                {
                    base.DrawGizmos(origin, rot);

                    var hitbox = new BoxHitcast(new Vector3(1.0f, 0.0f, 0.0f), Vector3.right, range, new Vector3(0.0f, 1.0f, GameManager.HalfSizeField_Z), Quaternion.identity);
                    hitbox.SetRange(range);
                    hitbox.DrawGizmos(origin, rot);
                }
#endif
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new ABlizzard(this, owner);
                }
            }
            /// <summary>
            /// 스킬에 필요한 변수 및 작동 방식만 정의
            /// (초기화는 PSKILL_NAME_DATA에서 처리)
            /// </summary>
            private class PBlizzardSlow : PSlow
            {
                public PBlizzardSlow(PBlizzardSlowData data, Unit skillOwner) : base(data, skillOwner) { }
            }
            [System.Serializable]
            protected class PBlizzardSlowData : PSlowData
            {
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new PBlizzardSlow(this, owner);
                }
            }
        }
    }
}
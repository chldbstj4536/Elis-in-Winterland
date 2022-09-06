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
            /// (초기화는 ABlueFlameTowerDA_DATA에서 처리)
            /// </summary>
            private class ABlueFlameTowerDA : NoneSkill
            {
                #region Field
                private readonly float duration;
                private readonly float tickInterval;
                private readonly BoxHitcast hitbox;
                private readonly GameObject skillFX;
                private readonly AutoReleaseParticlePrefab hitFXPrefab;

                private float curTime;
                private float curMaxTime;
                #endregion

                public ABlueFlameTowerDA(ABlueFlameTowerDAData data, Unit skillOwner) : base(data, skillOwner)
                {
                    duration = data.duration;
                    tickInterval = data.tickInterval;
                    hitbox = data.hitbox.Instantiate() as BoxHitcast;
                    skillFX = PrefabPool.GetObject(data.skillFXPrefab, false);
                    hitFXPrefab = data.hitFXPrefab;

#if UNITY_EDITOR
                    caster.OnGizmosEvent += () =>
                    {
                        hitbox.DrawGizmos(caster.transform.position, caster.IsLookingLeft ? Quaternion.identity : Quaternion.AngleAxis(180.0f, Vector3.up));
                    };
#endif
                }
                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    Vector3 skillFXPos = caster.transform.position;
                    skillFXPos.y = 0.5f;
                    skillFX.transform.position = skillFXPos;
                    skillFX.transform.rotation = Quaternion.Euler(0.0f, caster.IsLookingLeft ? -90.0f : 90.0f, 0.0f);

                    curMaxTime = curTime = 0.0f;

                    skillFX.SetActive(true);
                }
                protected override void OnUpdateAttack()
                {
                    base.OnUpdateAttack();

                    if (curTime >= tickInterval)
                    {
                        curTime -= tickInterval;
                        Trigger();
                    }

                    curTime += Time.deltaTime;
                    curMaxTime += Time.deltaTime;
                    if (curMaxTime >= duration)
                        CompleteTickAttack();
                }
                protected override void OnEndAttack(FSM.State newState)
                {
                    base.OnEndAttack(newState);

                    CooltimeStart();
                    skillFX.SetActive(false);
                }

                protected override void Trigger()
                {
                    base.Trigger();

                    var hits = Utility.SweepUnit(hitbox, caster.transform.position, caster.IsLookingLeft ? Quaternion.identity : Quaternion.AngleAxis(180.0f, Vector3.up), true, TargetLayerMask, TargetType);

                    foreach (var hit in hits)
                    {
                        Unit victim = hit.transform.GetComponent<Unit>();
                        Attack(victim, DAMAGE_TYPE.NORMAL, victim.transform.position);
                        PrefabPool.GetObject(hitFXPrefab).transform.position = victim.transform.position;
                    }
                }
                protected override void SetRange()
                {
                    base.SetRange();

                    hitbox.SetRange(TotalRange);
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return PhysicsDamageCalc(caster.TotalPhysicsPower, caster.TotalArmorPnt, victim.TotalArmor);
                }
            }
            protected class ABlueFlameTowerDAData : NoneSkillData
            {
                [BoxGroup("얼음불꽃 타워", true, true)]
                [LabelText("지속시간"), Min(0.0f)]
                public float duration;
                [BoxGroup("얼음불꽃 타워")]
                [LabelText("피해 간격"), Min(0.0001f)]
                public float tickInterval = 0.2f;
                [BoxGroup("얼음불꽃 타워/피격 범위", true, true)]
                [HideLabel]
                public BoxHitcast hitbox;
                [BoxGroup("얼음불꽃 타워/이펙트", true, true)]
                [LabelText("스킬 효과")]
                public PoolingComponent skillFXPrefab;
                [BoxGroup("얼음불꽃 타워/이펙트")]
                [LabelText("피격 효과")]
                public AutoReleaseParticlePrefab hitFXPrefab;

                public ABlueFlameTowerDAData()
                {
                    // 액티브 스킬의 캐스팅 타입
                    skillCastingType = SKILL_CASTING_TYPE.TICK;
                    // 기본공격여부
                    isDefaultAttack = true;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new ABlueFlameTowerDA(this, owner);
                }
#if UNITY_EDITOR
                public override void DrawGizmos(Vector3 origin, Quaternion rot)
                {
                    base.DrawGizmos(origin, rot);

                    hitbox.SetRange(range);
                    hitbox.DrawGizmos(origin, rot);
                }
#endif
            }
        }
    }
}
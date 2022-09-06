using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class ABreathOfFrost : VectorSkill
            {
                #region Field
                private int normal_magicDamage;
                private float normal_magicPowerRate;
                private PBreathOfFrostSlow slowPSkill;
                private float max_duringTime;
                private int blow_magicDamage;
                private float blow_magicPowerRate;
                private float pushAmount;
                private float knockbackSpeed;
                private IHitBox hitbox;

                private PoolingComponent loopAttackFXPrefab;
                private AutoReleaseParticlePrefab attackFXPrefab;

                private GameObject loopAttackFX;

                private float curTime;
                private float curMaxTime;
                private bool isFinalAttack;
                private const float TICK_TIME = 0.2f;
                #endregion

                public ABreathOfFrost(ABreathOfFrostData data, Unit skillOwner) : base(data, skillOwner)
                {
                    normal_magicDamage = data.normal_magicDamage;
                    normal_magicPowerRate = data.normal_magicPowerRate;
                    slowPSkill = data.slowPSkillData.Instantiate(skillOwner) as PBreathOfFrostSlow;
                    max_duringTime = data.max_duringTime;
                    blow_magicDamage = data.blow_magicDamage;
                    blow_magicPowerRate = data.blow_magicPowerRate;
                    pushAmount = data.pushAmount;
                    knockbackSpeed = data.knockbackSpeed;
                    hitbox = data.hitbox.Instantiate();
                    hitbox.SetRange(TotalRange);
                    loopAttackFXPrefab = data.loopAttackFXPrefab;
                    attackFXPrefab = data.attackFXPrefab;

                    loopAttackFX = PrefabPool.GetObject(loopAttackFXPrefab, false);
#if UNITY_EDITOR
                    caster.OnGizmosEvent += () =>
                    {
                        hitbox.SetRange(TotalRange);
                        hitbox.DrawGizmos(caster.transform.position, loopAttackFX.transform.rotation);
                    };
#endif
                }

                //--------------------------------------//
                // 스킬에 필요한 이벤트 오버라이딩           //
                //--------------------------------------//
                // On(Begin/Update/End)SelectingTarget  //
                // On(Begin/Update/End)Casting          //
                // On(Begin/Update/End)Attack           //
                //--------------------------------------//
                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    caster.curMP -= ManaCost;
                    loopAttackFX.SetActive(true);
                    curMaxTime = max_duringTime;
                }
                protected override void OnUpdateAttack()
                {
                    SetDestinationForPlayer();

                    if (curTime >= TICK_TIME)
                    {
                        curTime -= TICK_TIME;
                        Trigger();
                    }

                    Vector3 pos = caster.transform.position;
                    pos.y = 0.5f;
                    loopAttackFX.transform.position = pos;
                    loopAttackFX.transform.rotation = Quaternion.FromToRotation(Vector3.forward, dest - caster.transform.position);

                    caster.Flip(pos.x > dest.x);

                    curTime += Time.deltaTime;
                    curMaxTime -= Time.deltaTime;
                    if (curMaxTime <= 0.0f)
                        caster.mainAnimPlayer.ExitLoopAllTrack();
                }
                protected override void OnEndAttack(FSM.State newState)
                {
                    loopAttackFX.SetActive(false);

                    GameObject attackFX = PrefabPool.GetObject(attackFXPrefab, false);
                    Vector3 pos = caster.transform.position;
                    pos.y = 0.5f;
                    attackFX.transform.position = pos;
                    attackFX.transform.rotation = Quaternion.FromToRotation(Vector3.forward, dest - caster.transform.position);
                    attackFX.SetActive(true);

                    isFinalAttack = true;
                    var hits = Utility.SweepUnit(hitbox, caster.transform.position, loopAttackFX.transform.rotation, true, TargetLayerMask, TargetType);

                    foreach (var hit in hits)
                    {
                        Unit victim = hit.transform.GetComponent<Unit>();
                        Attack(victim, DAMAGE_TYPE.NORMAL, Utility.GetHitPoint(caster.transform.position, hit));
                        Vector3 knockbackDir = (victim.transform.position - caster.transform.position).normalized;
                        Vector3 knockbackDest = victim.transform.position + knockbackDir * pushAmount;
                        victim.mc.KnockBack(knockbackDest, knockbackSpeed);
                    }
                    isFinalAttack = false;

                    CooltimeStart();

                    base.OnEndAttack(newState);
                }

                protected override void Trigger()
                {
                    base.Trigger();

                    var hits = Utility.SweepUnit(hitbox, caster.transform.position, loopAttackFX.transform.rotation, true, TargetLayerMask, TargetType);

                    foreach (var hit in hits)
                    {
                        Unit victim = hit.transform.GetComponent<Unit>();
                        Attack(victim, DAMAGE_TYPE.NORMAL, Utility.GetHitPoint(caster.transform.position, hit));
                        slowPSkill.AddPassiveSkillToUnit(victim);
                    }
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    if (isFinalAttack)
                        return (int)(blow_magicDamage + blow_magicPowerRate * caster.TotalMagicPower);
                    else
                        return (int)(normal_magicDamage + normal_magicPowerRate * caster.TotalMagicPower);
                }
                protected override void SetRange()
                {
                    base.SetRange();
                    hitbox.SetRange(TotalRange);
                }
            }
            protected class ABreathOfFrostData : VectorSkillData
            {
                #region Field
                [BoxGroup("냉기의 숨결", true, true)]
                [LabelText("기본 스킬 피해량"), Tooltip("스킬의 기본 피해량입니다.")]
                public int normal_magicDamage;
                [BoxGroup("냉기의 숨결")]
                [LabelText("스킬 피해량 계수"), Tooltip("플레이어의 마법공격력에 따라 피해량이 증가하는 비율입니다.")]
                public float normal_magicPowerRate;
                [BoxGroup("냉기의 숨결/냉기의 숨결 슬로우")]
                [HideLabel]
                public PBreathOfFrostSlowData slowPSkillData = new PBreathOfFrostSlowData();
                [BoxGroup("냉기의 숨결")]
                [LabelText("스킬 최대 지속시간"), Tooltip("스킬이 지속될 수 있는 최대시간입니다.")]
                public float max_duringTime;
                [BoxGroup("냉기의 숨결")]
                [LabelText("최후의 일격 피해량"), Tooltip("스킬이 끝날때 입히는 최후의 일격의 피해량입니다.")]
                public int blow_magicDamage;
                [BoxGroup("냉기의 숨결")]
                [LabelText("최후의 일격 피해량 계수"), Tooltip("플레이어의 마법공격력에 따라 최후의 일격 피해량이 증가하는 비율입니다.")]
                public float blow_magicPowerRate;
                [BoxGroup("냉기의 숨결")]
                [LabelText("넉백 수치"), Tooltip("최후의 일격에 피해를 입은 유닛들이 얼마나 밀려날지에 대한 값입니다.")]
                public float pushAmount;
                [BoxGroup("냉기의 숨결"), Min(0.1f)]
                [LabelText("넉백 속도"), Tooltip("밀려나는 속도에 대한 값입니다.")]
                public float knockbackSpeed;
                [BoxGroup("냉기의 숨결/스킬 범위", true, true)]
                [HideLabel, SerializeReference, Tooltip("냉기의 숨결 스킬 범위입니다.")]
                public IHitBox hitbox;
                [BoxGroup("냉기의 숨결")]
                [LabelText("지속효과 이펙트")]
                public PoolingComponent loopAttackFXPrefab;
                [BoxGroup("냉기의 숨결")]
                [LabelText("파이널 이펙트")]
                public AutoReleaseParticlePrefab attackFXPrefab;
                #endregion

                public ABreathOfFrostData()
                {
                    // 액티브 스킬의 캐스팅 타입
                    skillCastingType = SKILL_CASTING_TYPE.TICK;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new ABreathOfFrost(this, owner);
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

            private class PBreathOfFrostSlow : PSlow
            {
                public PBreathOfFrostSlow(PBreathOfFrostSlowData data, Unit skillOwner) : base(data, skillOwner) { }
            }

            [System.Serializable]
            protected class PBreathOfFrostSlowData : PSlowData
            {
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new PBreathOfFrostSlow(this, owner);
                }
            }
        }
    }
}
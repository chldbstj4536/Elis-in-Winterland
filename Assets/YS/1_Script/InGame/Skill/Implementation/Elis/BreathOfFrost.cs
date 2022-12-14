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
                // ?????? ?????? ?????? ??????????           //
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
                [BoxGroup("?????? ????", true, true)]
                [LabelText("???? ???? ??????"), Tooltip("?????? ???? ????????????.")]
                public int normal_magicDamage;
                [BoxGroup("?????? ????")]
                [LabelText("???? ?????? ????"), Tooltip("?????????? ???????????? ???? ???????? ???????? ??????????.")]
                public float normal_magicPowerRate;
                [BoxGroup("?????? ????/?????? ???? ??????")]
                [HideLabel]
                public PBreathOfFrostSlowData slowPSkillData = new PBreathOfFrostSlowData();
                [BoxGroup("?????? ????")]
                [LabelText("???? ???? ????????"), Tooltip("?????? ?????? ?? ???? ??????????????.")]
                public float max_duringTime;
                [BoxGroup("?????? ????")]
                [LabelText("?????? ???? ??????"), Tooltip("?????? ?????? ?????? ?????? ?????? ????????????.")]
                public int blow_magicDamage;
                [BoxGroup("?????? ????")]
                [LabelText("?????? ???? ?????? ????"), Tooltip("?????????? ???????????? ???? ?????? ???? ???????? ???????? ??????????.")]
                public float blow_magicPowerRate;
                [BoxGroup("?????? ????")]
                [LabelText("???? ????"), Tooltip("?????? ?????? ?????? ???? ???????? ?????? ?????????? ???? ????????.")]
                public float pushAmount;
                [BoxGroup("?????? ????"), Min(0.1f)]
                [LabelText("???? ????"), Tooltip("???????? ?????? ???? ????????.")]
                public float knockbackSpeed;
                [BoxGroup("?????? ????/???? ????", true, true)]
                [HideLabel, SerializeReference, Tooltip("?????? ???? ???? ??????????.")]
                public IHitBox hitbox;
                [BoxGroup("?????? ????")]
                [LabelText("???????? ??????")]
                public PoolingComponent loopAttackFXPrefab;
                [BoxGroup("?????? ????")]
                [LabelText("?????? ??????")]
                public AutoReleaseParticlePrefab attackFXPrefab;
                #endregion

                public ABreathOfFrostData()
                {
                    // ?????? ?????? ?????? ????
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
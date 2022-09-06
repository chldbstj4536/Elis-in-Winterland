using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class AUnstableLavaDA : NoneSkill
            {
                #region Field
                private IHitBox mainHitbox;
                private IHitBox splashHitbox;
                private UNIT_FLAG hitUnitType;

                private float limitTime;
                private bool isSubAttack;

                private PoolingComponent hitFXPrefab;
                private HashSet<Transform> hitSet = new HashSet<Transform>();
                #endregion

                public AUnstableLavaDA(AUnstableLavaDAData data, Unit skillOwner) : base(data, skillOwner)
                {
                    hitUnitType = data.hitUnitType;
                    hitFXPrefab = data.hitFXPrefab;

                    mainHitbox = data.mainHitbox.Instantiate();
                    splashHitbox = data.splashHitbox.Instantiate();
                    mainHitbox.SetRange(TotalRange);
                    splashHitbox.SetRange(TotalRange);
#if UNITY_EDITOR
                    caster.OnGizmosEvent += () =>
                    {
                        mainHitbox.DrawGizmos(caster.transform.position, Quaternion.identity);
                        splashHitbox.DrawGizmos(caster.transform.position, Quaternion.identity);
                    };
#endif
                }
                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    limitTime = 1 / caster.TotalAttackSpeed;
                }
                protected override void OnUpdateAttack()
                {
                    limitTime -= Time.deltaTime;

                    if (limitTime <= 0.0f)
                    {
                        Trigger();
                        caster.OnDie(caster);
                    }
                }

                protected override void Trigger()
                {
                    base.Trigger();

                    hitSet.Clear();

                    Unit victim;

                    // 메인 공격 먼저 진행
                    isSubAttack = false;
                    var hitList = Utility.SweepUnit(mainHitbox, caster.transform.position, caster.transform.rotation, true, TargetLayerMask, hitUnitType);
                    foreach (var hit in hitList)
                    {
                        victim = hit.transform.GetComponent<Unit>();
                        Attack(victim, DAMAGE_TYPE.NORMAL, victim.transform.position);
                        PrefabPool.GetObject(hitFXPrefab).transform.position = victim.transform.position;

                        // 피격된 유닛들은 따로 저장
                        hitSet.Add(hit.transform);
                    }

                    // 서브 공격 진행
                    isSubAttack = true;
                    hitList = Utility.SweepUnit(splashHitbox, caster.transform.position, caster.transform.rotation, true, TargetLayerMask, hitUnitType);
                    foreach (var hit in hitList)
                    {
                        // 이미 피격된 유닛들이 있다면 제외
                        if (hitSet.Add(hit.transform))
                        {
                            victim = hit.transform.GetComponent<Unit>();
                            Attack(victim, DAMAGE_TYPE.NORMAL, victim.transform.position);
                            PrefabPool.GetObject(hitFXPrefab).transform.position = victim.transform.position;
                        }
                    }
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    int totalDmg;

                    if (victim.UnitType == UNIT_TYPE.TOWER)
                        totalDmg = caster.TotalPhysicsPower;
                    else
                        totalDmg = PhysicsDamageCalc(caster.TotalPhysicsPower, caster.TotalArmorPnt, victim.TotalArmor);

                    return isSubAttack ? totalDmg / 2 : totalDmg;
                }
                protected override void SetRange()
                {
                    base.SetRange();
                    mainHitbox.SetRange(TotalRange);
                    splashHitbox.SetRange(TotalRange);
                }
            }
            protected class AUnstableLavaDAData : NoneSkillData
            {
                [BoxGroup("불안정한 용암 기본공격"), SerializeReference]
                [LabelText("메인 피격 효과 범위"), Tooltip("온전한 피해를 입히는 범위를 결정합니다.")]
                public IHitBox mainHitbox;
                [BoxGroup("불안정한 용암 기본공격"), SerializeReference]
                [LabelText("서브 피격 효과 범위"), Tooltip("절반의 피해를 입히는 범위를 결정합니다.")]
                public IHitBox splashHitbox;
                [BoxGroup("불안정한 용암 기본공격")]
                [EnumToggleButtons, LabelText("피격 효과 대상"), Tooltip("피해를 받는 유닛 타입을 결정합니다.")]
                public UNIT_FLAG hitUnitType;

                [BoxGroup("불안정한 용암 기본공격", true, true), LabelText("피격 효과 프리팹")]
                public PoolingComponent hitFXPrefab;

                public AUnstableLavaDAData()
                {
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    isDefaultAttack = true;
                }

                public override BaseSkill Instantiate(Unit owner)
                {
                    return new AUnstableLavaDA(this, owner);
                }
#if UNITY_EDITOR
                public override void DrawGizmos(Vector3 origin, Quaternion rot)
                {
                    base.DrawGizmos(origin, rot);
                    mainHitbox.SetRange(range);
                    splashHitbox.SetRange(range);
                    mainHitbox.DrawGizmos(origin, rot);
                    splashHitbox.DrawGizmos(origin, rot);
                }
#endif
            }
        }
    }
}
using System.Collections.Generic;
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
            private class AEverFrost : NoneSkill
            {
                #region Field
                private int magicHeal;
                private float magicHealRate;
                private int magicDamage;
                private float magicDamageRate;
                private IHitBox hitbox;

                private AutoReleaseParticlePrefab skillFXPrefab;
                private AutoReleaseParticlePrefab hitFXPrefab;
                private AutoReleaseParticlePrefab healFXPrefab;
                #endregion

                public AEverFrost(AEverFrostData data, Unit skillOwner) : base(data, skillOwner)
                {
                    magicHeal = data.magicHeal;
                    magicHealRate = data.magicHealRate;
                    magicDamage = data.magicDamage;
                    magicDamageRate = data.magicDamageRate;
                    hitbox = data.hitbox.Instantiate();
                    hitbox.SetRange(TotalRange);

                    skillFXPrefab = data.skillFXPrefab;
                    hitFXPrefab = data.hitFXPrefab;
                    healFXPrefab = data.healFXPrefab;
#if UNITY_EDITOR
                    caster.OnGizmosEvent += () =>
                    {
                        hitbox.SetRange(TotalRange);
                        hitbox.DrawGizmos(caster.transform.position, Quaternion.identity);
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

                    Trigger();
                    CooltimeStart();
                    caster.curMP -= ManaCost;
                }

                protected override void Trigger()
                {
                    base.Trigger();

                    var hits = Utility.SweepUnit(hitbox, caster.transform.position, Quaternion.identity, true, TargetLayerMask, TargetType);

                    foreach (var hit in hits)
                    {
                        Unit victim = hit.transform.GetComponent<Unit>();

                        if (victim.gameObject.layer == caster.gameObject.layer)
                        {
                            Attack(victim, DAMAGE_TYPE.HEAL, victim.transform.position);
                            PrefabPool.GetObject(healFXPrefab).transform.position = victim.transform.position;
                        }
                        else
                        {
                            Attack(victim, DAMAGE_TYPE.NORMAL, victim.transform.position);
                            PrefabPool.GetObject(hitFXPrefab).transform.position = victim.transform.position; 
                        }
                    }

                    var fx = PrefabPool.GetObject(skillFXPrefab, true);
                    Vector3 pos = caster.transform.position;
                    pos.y = 3.0f;
                    fx.transform.position = pos;
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    if (victim.gameObject.layer == caster.gameObject.layer)
                        return -(int)(magicHeal + magicHealRate * caster.TotalMagicPower);
                    else
                        return (int)(magicDamage + magicDamageRate * caster.TotalMagicPower);
                }
            }
            protected class AEverFrostData : NoneSkillData
            {
                [FoldoutGroup("만년서리", true)]
                [LabelText("회복량"), Tooltip("스킬 범위 내의 아군에게 적용되는 회복량")]
                public int magicHeal;
                [FoldoutGroup("만년서리")]
                [LabelText("회복량 계수"), Tooltip("플레이어의 마법공격력에 따라 회뵥량이 증가하는 비율")]
                public float magicHealRate;
                [FoldoutGroup("만년서리")]
                [LabelText("피해량"), Tooltip("스킬 범위 내의 적군에게 적용되는 피해량")]
                public int magicDamage;
                [FoldoutGroup("만년서리")]
                [LabelText("피해량 계수"), Tooltip("플레이어의 마법공격력에 따라 피해량이 증가하는 비율")]
                public float magicDamageRate;
                [FoldoutGroup("만년서리/공격 범위"), SerializeReference]
                [HideLabel, Tooltip("스킬 히트 범위를 결정")]
                public IHitBox hitbox;

                [FoldoutGroup("만년서리/이펙트")]
                [LabelText("스킬 효과 프리팹"), Tooltip("스킬의 효과 이펙트 프리팹")]
                public AutoReleaseParticlePrefab skillFXPrefab;
                [FoldoutGroup("만년서리/이펙트")]
                [LabelText("피격 효과"), Tooltip("피격 효과 프리팹")]
                public AutoReleaseParticlePrefab hitFXPrefab;
                [FoldoutGroup("만년서리/이펙트")]
                [LabelText("치유 효과"), Tooltip("치유 효과 프리팹")]
                public AutoReleaseParticlePrefab healFXPrefab;

                public AEverFrostData()
                {
                    // 액티브 스킬의 캐스팅 타입
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // 기본공격여부
                    isDefaultAttack = false;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new AEverFrost(this, owner);
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
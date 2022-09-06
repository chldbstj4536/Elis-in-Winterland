using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            public abstract partial class BaseSkill
            {
                public const float INFINITE_RANGE = 0.0f;

                #region Field
                // 스킬 시전자
                protected Unit caster;

                private bool bActive;
                protected bool criticalHit;

                // 선택가능한 스킬 대상, 위치 / 스킬 효과 적용 가능한 범위 (투사체의 사거리 X)
                // range == 0 이면 사거리 제한 X
                private readonly float range;
                private float rangeAdd;
                private float rangeCoef;

                // 적중 대상 레이어
                private readonly TARGET_LAYER_MASK targetLayerMask;
                private readonly UNIT_FLAG targetType;

                protected readonly bool isUsingCasterRotation;
                protected IHitBox[] inRangeHitboxes;

                public delegate void OnAttackUnit(BaseSkill attacker, Unit victim, int totalDmg, DAMAGE_TYPE dmgType, Vector3 hitPos);
                public delegate void OnCriticalHit(BaseSkill attacker, Unit victim);

                public event OnChangedValue OnChangedActive;
                public event OnAttackUnit OnBeforeAttackEvent;
                public event OnAttackUnit OnAfterAttackEvent;
                public event OnCriticalHit OnCriticalHitEvent;
                #endregion

                #region Properties
                public Unit Caster => caster;
                public bool Active
                {
                    get { return bActive; }
                    set
                    {
                        bActive = value;
                        OnChangedActive?.Invoke();
                    }
                }
                public Quaternion InRangeHitBoxesRot => isUsingCasterRotation && !caster.IsLookingLeft ? Quaternion.AngleAxis(180.0f, Vector3.up) : Quaternion.identity;
                public float RangeAdditional
                {
                    get { return rangeAdd; }
                    set
                    {
                        rangeAdd = value;
                        SetRange();
                    }
                }
                public float RangeCoefficient
                {
                    get { return rangeCoef; }
                    set
                    {
                        rangeCoef = value;
                        SetRange();
                    }
                }
                public float TotalRange => range * rangeCoef + rangeAdd;
                public float SqrTotalRange => TotalRange * TotalRange;
                public UNIT_FLAG TargetType => targetType;
                public int TargetLayerMask
                {
                    get
                    {
                        int result = 0;
                        LAYER_NUMBER teamNumber = (LAYER_NUMBER)caster.gameObject.layer;

                        if (Utility.HasFlag((int)targetLayerMask, (int)TARGET_LAYER_MASK.ALLY))
                            result |= (int)(teamNumber == LAYER_NUMBER.TEAM1 ? LAYER_MASK.TEAM1 : LAYER_MASK.TEAM2);
                        if (Utility.HasFlag((int)targetLayerMask, (int)TARGET_LAYER_MASK.ENEMY))
                            result |= (int)(teamNumber == LAYER_NUMBER.TEAM1 ? LAYER_MASK.TEAM2 : LAYER_MASK.TEAM1);

                        return result;
                    }
                }
                public abstract bool IsUsingCritical { get; }
                #endregion

                #region Methods
                protected BaseSkill(BaseSkillData data, Unit skillOwner)
                {
                    caster = skillOwner;

                    isUsingCasterRotation = data.isUsingCasterRotation;
                    inRangeHitboxes = new IHitBox[data.inRangeHitboxes.Length];
                    for (int i = 0; i < inRangeHitboxes.Length; ++i)
                        inRangeHitboxes[i] = data.inRangeHitboxes[i].Instantiate();

                    range = data.range;
                    rangeCoef = 1.0f;
                    rangeAdd = 0.0f;

                    foreach (var hitbox in inRangeHitboxes)
                        hitbox.SetRange(TotalRange);

                    targetLayerMask = data.targetLayerMask;
                    targetType = data.targetType;
#if UNITY_EDITOR
                    caster.OnGizmosEvent += () =>
                    {
                        foreach (var inRangeHitbox in inRangeHitboxes)
                            inRangeHitbox.DrawGizmos(caster.transform.position, InRangeHitBoxesRot);
                    };
#endif
                }
                /// <summary>
                /// 히트박스 중 사거리에 영향 받는 히트박스가 있다면 이 함수에서 사거리 적용
                /// </summary>
                protected virtual void SetRange()
                {
                    foreach (var hitbox in inRangeHitboxes)
                        hitbox.SetRange(TotalRange);
                }
                protected abstract int GetTotalDamage(Unit victim);
                protected void Attack(Unit victim, DAMAGE_TYPE dmgType, Vector3 hitPos, bool critical = false, TotalDamageCalcEvent customDmgCalcEvent = null)
                {
                    int totalDmg;
                    if (customDmgCalcEvent == null)
                        totalDmg = GetTotalDamage(victim);
                    else
                        totalDmg = customDmgCalcEvent(victim);

                    if (IsUsingCritical && (criticalHit || critical || Random.Range(0, 100) < caster.TotalCriticalRate))
                    {
                        totalDmg *= 2;
                        OnCriticalHitEvent?.Invoke(this, victim);
                        criticalHit = false;
                    }

                    OnBeforeAttackEvent?.Invoke(this, victim, totalDmg, dmgType, hitPos);
                    victim.OnBeforeHitEvent?.Invoke(this, victim, totalDmg, dmgType, hitPos);

                    foreach (var shield in victim.curShields.Values)
                    {
                        if (shield.CurrentShieldAmount < totalDmg)
                        {
                            totalDmg -= shield.CurrentShieldAmount;
                            shield.CurrentShieldAmount = 0;
                        }
                        else
                        {
                            shield.CurrentShieldAmount -= totalDmg;
                            totalDmg = 0;
                            return;
                        }
                    }
                    victim.CurrentHP -= totalDmg;

                    switch (dmgType)
                    {
                        case DAMAGE_TYPE.NORMAL:
                            caster.gm.dmgFont.Spawn(hitPos, totalDmg);
                            break;
                        case DAMAGE_TYPE.HEAL:
                            caster.gm.healFont.Spawn(hitPos, -totalDmg);
                            break;
                        case DAMAGE_TYPE.DOT:
                            caster.gm.dotFont.Spawn(hitPos, totalDmg);
                            break;
                    }

                    if (victim.CurrentHP <= 0)
                        victim.OnDie(caster);
                    else if (victim.CurrentHP > victim.MaxHP)
                        victim.CurrentHP = victim.MaxHP;

                    OnAfterAttackEvent?.Invoke(this, victim, totalDmg, dmgType, hitPos);
                    victim.OnAfterHitEvent?.Invoke(this, victim, totalDmg, dmgType, hitPos);
                }
                static public int PhysicsDamageCalc(int physxDmg, float totalArmorPnt, float totalArmor)
                {
                    return (int)(physxDmg * (1 - Mathf.Clamp(totalArmorPnt - totalArmor, 0.0f, 1.0f)));
                }
                #endregion
            }

            [System.Serializable]
            public abstract class BaseSkillData
            {
                [HideIf("@true")]
                public bool ShowPPSetting = true;
                [BoxGroup("프로퍼티 세팅", true, true, -100), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("사거리 사용")]
                protected bool PP_UseRange = true;
                [BoxGroup("프로퍼티 세팅"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("기본 타겟지정 사용")]
                protected bool PP_UseDefaultTarget = true;
                [BoxGroup("프로퍼티 세팅"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("기본 히트박스 사용")]
                protected bool PP_UseInRangeHitboxes = true;
                [BoxGroup("프로퍼티 세팅"), ShowIf("ShowPPSetting"), SerializeField]
                [Button("프로퍼티 세팅 종료")]
                private void PPSettingEnd() { ShowPPSetting = false; }
#if UNITY_EDITOR
                [ShowIf("PP_UseRange")]
#endif
                [FoldoutGroup("Skill", 5), Min(0.0f)]
                [LabelText("스킬 사거리"), Tooltip("히트박스에 적용되는 사거리 (투사체의 사거리 X)\nrange == 0 이면 사거리 제한 X")]
                public float range;
#if UNITY_EDITOR
                [ShowIf("PP_UseDefaultTarget")]
#endif
                [FoldoutGroup("Skill"), EnumToggleButtons]
                [LabelText("타겟 지정 가능 대상"), Tooltip("실제 피해 적용 대상은 각 스킬마다 따로 입력받을수도 있습니다.\nAI의 경우 공격 가능 여부(공격 가능한 대상 존재 여부)를 이 설정을 통해 정해집니다.")]
                public TARGET_LAYER_MASK targetLayerMask;
#if UNITY_EDITOR
                [ShowIf("PP_UseDefaultTarget")]
#endif
                [FoldoutGroup("Skill"), EnumToggleButtons]
                [LabelText("타겟 지정 가능 타입"), Tooltip("실제 피해 적용 타입은 각 스킬마다 따로 입력받을수도 있습니다.\nAI의 경우 공격 가능 여부(공격 가능한 대상 존재 여부)를 이 설정을 통해 정해집니다.")]
                public UNIT_FLAG targetType;

#if UNITY_EDITOR
                [ShowIf("PP_UseInRangeHitboxes")]
#endif
                [BoxGroup("Skill/AI 전용", true, true)]
                [LabelText("공격 감지 범위에 캐스터 회전 적용 여부"), LabelWidth(250.0f), Tooltip("공격 감지 범위 히트박스에 캐스터의 방향에 따라 히트박스의 회전이 이루어질지에 대한 여부입니다.\n왼쪽을 바라보는것이 기본값입니다.")]
                public bool isUsingCasterRotation;
#if UNITY_EDITOR
                [ShowIf("PP_UseInRangeHitboxes")]
#endif
                [BoxGroup("Skill/AI 전용"), SerializeReference]
                [LabelText("공격 감지 범위"), Tooltip("AI의 공격 가능 여부를 판단하기 위한 히트박스입니다.\n기본적으로 해당 히트박스에 감지된 대상을 목적지(or 타겟)으로 정합니다.\n감지와 별도로 대상을 정하고 싶다면 별도의 구현이 필요합니다.")]
                public IHitBox[] inRangeHitboxes = new IHitBox[0];

                public abstract BaseSkill Instantiate(Unit owner);
#if UNITY_EDITOR
                public virtual void DrawGizmos(Vector3 origin, Quaternion rot)
                {
                    foreach (var inRangeHitbox in inRangeHitboxes)
                        inRangeHitbox.SetRange(range);
                    foreach (var inRangeHitbox in inRangeHitboxes)
                        inRangeHitbox.DrawGizmos(origin, rot);
                }
#endif
            }
        }
    }
}
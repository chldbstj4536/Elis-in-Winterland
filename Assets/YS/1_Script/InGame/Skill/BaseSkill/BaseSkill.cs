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
                // ��ų ������
                protected Unit caster;

                private bool bActive;
                protected bool criticalHit;

                // ���ð����� ��ų ���, ��ġ / ��ų ȿ�� ���� ������ ���� (����ü�� ��Ÿ� X)
                // range == 0 �̸� ��Ÿ� ���� X
                private readonly float range;
                private float rangeAdd;
                private float rangeCoef;

                // ���� ��� ���̾�
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
                /// ��Ʈ�ڽ� �� ��Ÿ��� ���� �޴� ��Ʈ�ڽ��� �ִٸ� �� �Լ����� ��Ÿ� ����
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
                [BoxGroup("������Ƽ ����", true, true, -100), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("��Ÿ� ���")]
                protected bool PP_UseRange = true;
                [BoxGroup("������Ƽ ����"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("�⺻ Ÿ������ ���")]
                protected bool PP_UseDefaultTarget = true;
                [BoxGroup("������Ƽ ����"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("�⺻ ��Ʈ�ڽ� ���")]
                protected bool PP_UseInRangeHitboxes = true;
                [BoxGroup("������Ƽ ����"), ShowIf("ShowPPSetting"), SerializeField]
                [Button("������Ƽ ���� ����")]
                private void PPSettingEnd() { ShowPPSetting = false; }
#if UNITY_EDITOR
                [ShowIf("PP_UseRange")]
#endif
                [FoldoutGroup("Skill", 5), Min(0.0f)]
                [LabelText("��ų ��Ÿ�"), Tooltip("��Ʈ�ڽ��� ����Ǵ� ��Ÿ� (����ü�� ��Ÿ� X)\nrange == 0 �̸� ��Ÿ� ���� X")]
                public float range;
#if UNITY_EDITOR
                [ShowIf("PP_UseDefaultTarget")]
#endif
                [FoldoutGroup("Skill"), EnumToggleButtons]
                [LabelText("Ÿ�� ���� ���� ���"), Tooltip("���� ���� ���� ����� �� ��ų���� ���� �Է¹������� �ֽ��ϴ�.\nAI�� ��� ���� ���� ����(���� ������ ��� ���� ����)�� �� ������ ���� �������ϴ�.")]
                public TARGET_LAYER_MASK targetLayerMask;
#if UNITY_EDITOR
                [ShowIf("PP_UseDefaultTarget")]
#endif
                [FoldoutGroup("Skill"), EnumToggleButtons]
                [LabelText("Ÿ�� ���� ���� Ÿ��"), Tooltip("���� ���� ���� Ÿ���� �� ��ų���� ���� �Է¹������� �ֽ��ϴ�.\nAI�� ��� ���� ���� ����(���� ������ ��� ���� ����)�� �� ������ ���� �������ϴ�.")]
                public UNIT_FLAG targetType;

#if UNITY_EDITOR
                [ShowIf("PP_UseInRangeHitboxes")]
#endif
                [BoxGroup("Skill/AI ����", true, true)]
                [LabelText("���� ���� ������ ĳ���� ȸ�� ���� ����"), LabelWidth(250.0f), Tooltip("���� ���� ���� ��Ʈ�ڽ��� ĳ������ ���⿡ ���� ��Ʈ�ڽ��� ȸ���� �̷�������� ���� �����Դϴ�.\n������ �ٶ󺸴°��� �⺻���Դϴ�.")]
                public bool isUsingCasterRotation;
#if UNITY_EDITOR
                [ShowIf("PP_UseInRangeHitboxes")]
#endif
                [BoxGroup("Skill/AI ����"), SerializeReference]
                [LabelText("���� ���� ����"), Tooltip("AI�� ���� ���� ���θ� �Ǵ��ϱ� ���� ��Ʈ�ڽ��Դϴ�.\n�⺻������ �ش� ��Ʈ�ڽ��� ������ ����� ������(or Ÿ��)���� ���մϴ�.\n������ ������ ����� ���ϰ� �ʹٸ� ������ ������ �ʿ��մϴ�.")]
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
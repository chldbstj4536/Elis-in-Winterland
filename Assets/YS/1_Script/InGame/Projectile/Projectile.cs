using System.Collections.Generic;
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

                public abstract class Projectile : PoolingComponent
                {
                    #region Field
                    [FoldoutGroup("����ü")]
                    [BoxGroup("����ü/����ü ����"), SerializeField]
                    [LabelText("�浹 �̺�Ʈ ���"), Tooltip("����ü�� ���� �浹 �� �̺�Ʈ�� �߻���ų���� ���մϴ�.\n������� �ʴ´ٸ� OnHit�̺�Ʈ�� �߻����� �ʽ��ϴ�.")]
                    private bool isUsingOnHit;
                    [BoxGroup("����ü/����ü ����"), SerializeField]
                    [LabelText("���� �̺�Ʈ ���"), Tooltip("����ü�� �������� ���� �� �̺�Ʈ�� �߻���ų���� ���մϴ�.\n������� �ʴ´ٸ� OnArrive�̺�Ʈ�� �߻����� �ʽ��ϴ�.")]
                    private bool isUsingOnArrive;
                    [BoxGroup("����ü/����ü ����"), SerializeField, Min(0.0f)]
                    [LabelText("��Ÿ�/����"), Tooltip("����ü�� ��Ʈ�ڽ��鿡 ����Ǵ� Range(����)���� �����մϴ�")]
                    private float range;
                    private float rangeAdd = 0.0f;
                    private float rangeCoef = 1.0f;

                    [FoldoutGroup("����ü/�浹 ����"), ShowIf("isUsingOnHit"), SerializeReference]
                    [LabelText("�浹 ����(��Ʈ �ڽ�)")]
                    protected IHitBox[] hitboxesCol = new IHitBox[0];
                    [FoldoutGroup("����ü/�浹 ����"), Min(0), ShowIf("isUsingOnHit"), SerializeField]
                    [LabelText("���� Ƚ��"), Tooltip("����ü�� ��� ����� ���������� ���� ����\n0�̸� �׻� ����")]
                    protected int countPiercing = 1;
                    [FoldoutGroup("����ü/�浹 ����"), ShowIf("isUsingOnHit"), SerializeField]
                    [LabelText("�浹 �� ����")]
                    private bool isUsingAttackOnHit;
                    [FoldoutGroup("����ü/�浹 ����/���� ����"), ShowIf("@isUsingOnHit && isUsingAttackOnHit"), SerializeField]
                    [LabelText("��Ʈ ȿ�� ������")]
                    private AutoReleaseParticlePrefab fxPrefabOnHitByColl;
                    [FoldoutGroup("����ü/�浹 ����/���� ����"), ShowIf("@isUsingOnHit && isUsingAttackOnHit"), SerializeField]
                    [LabelText("���� ����")]
                    private bool isUsingRAOnHit;
                    [FoldoutGroup("����ü/�浹 ����/���� ����"), ShowIf("@isUsingOnHit && isUsingAttackOnHit && isUsingRAOnHit"), SerializeReference]
                    [LabelText("���� ����")]
                    private IHitBox[] hitBoxesOnHit = new IHitBox[0];

                    [FoldoutGroup("����ü/���� ����"), ShowIf("isUsingOnArrive"), EnableIf("@isUsingOnArrive && isUsingOnHit"), SerializeField]
                    [LabelText("����ü �ı� �� ��������"), Tooltip("���� Ƚ���� ���� ����ü�� ������� ���� �̺�Ʈ�� �߻�����")]
                    private bool isLastHitArrival;
                    [FoldoutGroup("����ü/���� ����"), ShowIf("@isUsingOnArrive"), SerializeField]
                    [LabelText("���� �� ȿ�� ������")]
                    private AutoReleaseParticlePrefab fxPrefabOnArrive;
                    [FoldoutGroup("����ü/���� ����"), ShowIf("isUsingOnArrive"), SerializeField]
                    [LabelText("���� �� ����")]
                    private bool isUsingAttackOnArrive;
                    [FoldoutGroup("����ü/���� ����/���� ����"), ShowIf("@isUsingOnArrive && isUsingAttackOnArrive"), SerializeField]
                    [LabelText("��Ʈ ȿ�� ������")]
                    private AutoReleaseParticlePrefab fxPrefabOnHitByAriv;
                    [FoldoutGroup("����ü/���� ����/���� ����"), ShowIf("@isUsingOnArrive && isUsingAttackOnArrive"), SerializeField]
                    [LabelText("���� ����"), Tooltip("Ÿ���� ������ �ƴ϶�� ���� ���� ������ �ȵǾ������� ���ظ� ������ �ʽ��ϴ�.")]
                    private bool isUsingRAOnArrive;
                    [FoldoutGroup("����ü/���� ����/���� ����"), ShowIf("@isUsingOnArrive && isUsingAttackOnArrive && isUsingRAOnArrive"), SerializeReference]
                    [LabelText("���� ����")]
                    private IHitBox[] hitBoxesOnArrive = new IHitBox[0];

                    protected BaseSkill shooter;
                    [BoxGroup("����ü/�����(����X)", true, true, 10), ShowIf("isTargeting"), ShowInInspector]
                    protected Transform target;
                    [BoxGroup("����ü/�����(����X)", true, true, 10), HideIf("isTargeting"), ShowInInspector]
                    protected Vector3 dest;
                    [BoxGroup("����ü/�����(����X)", true, true, 10), ShowInInspector]
                    protected bool isTargeting;

                    // �浹�� ���� ���̾� �� �����÷���
                    private int layerMaskOnCol;
                    private UNIT_FLAG unitFlagOnCol;

                    // �浹 ���ݿ� ���� ���ط�, ���̾� �� �����÷���
                    private DAMAGE_TYPE dmgTypeOnHit;
                    private int layerMaskOnHit;
                    private UNIT_FLAG unitFlagOnHit;

                    // ���� ���ݿ� ���� ���ط�, ���̾� �� �����÷���
                    private DAMAGE_TYPE dmgTypeOnArrive;
                    private int layerMaskOnArrive;
                    private UNIT_FLAG unitFlagOnArrive;

                    protected Vector3 beforePos;
                    protected int curCountPiercing;
                    private HashSet<Transform> savedHitUnit = new HashSet<Transform>();
                    #endregion

                    #region Event
                    public delegate void OnHitEvent(Unit hit, Vector3 hitPos);
                    public delegate void OnArriveEvent(Vector3 arrivePos);

                    public event OnChangedValue OnChangedRange;

                    // ������(Ÿ��)�� ���������� ȣ��
                    public event OnArriveEvent OnArrive;
                    // ������(Ÿ��)�� ���� �� ���ش�� ȣ��
                    public event OnHitEvent OnArriveHit;
                    // ����ü�� �浹 �� ȣ��
                    public event OnHitEvent OnHit;
                    public event TotalDamageCalcEvent TotalDamageCalcOnHit;
                    public event TotalDamageCalcEvent TotalDamageCalcOnArriveHit;
                    #endregion

                    #region Properties
                    public virtual bool IsLookingLeft => transform.localScale.x > 0.0f;
                    public float TotalRange => (range * rangeCoef) + rangeAdd;
                    public float RangeAdd
                    {
                        get { return rangeAdd; }
                        set
                        {
                            rangeAdd = value;
                            OnChangedRange?.Invoke();
                        }
                    }
                    public float RangeCoef
                    {
                        get { return rangeCoef; }
                        set
                        {
                            rangeCoef = value;
                            OnChangedRange?.Invoke();
                        }
                    }
                    public abstract float Speed { get; set; }
                    public abstract float SpeedCoef { get; set; }
                    public float TotalSpeed => Speed * SpeedCoef;
                    public int LayerMaskOnCol => layerMaskOnCol;
                    public UNIT_FLAG UnitFlagOnCol => unitFlagOnCol;
                    public DAMAGE_TYPE DmgTypeOnHit => dmgTypeOnHit;
                    public int LayerMaskOnHit => layerMaskOnHit;
                    public UNIT_FLAG UnitFlagOnHit => unitFlagOnHit;
                    public DAMAGE_TYPE DmgTypeOnArrive => dmgTypeOnArrive;
                    public int LayerMaskOnArrive => layerMaskOnArrive;
                    public UNIT_FLAG UnitFlagOnArrive => unitFlagOnArrive;
                    public IHitBox[] HitboxesCol => hitboxesCol;
                    public IHitBox[] HitBoxesOnHit => hitBoxesOnHit;
                    public IHitBox[] HitBoxesOnArrive => hitBoxesOnArrive;
                    #endregion

                    #region Unity Methods
                    protected virtual void OnEnable()
                    {
                        savedHitUnit.Clear();
                        curCountPiercing = countPiercing == 0 ? -1 : countPiercing;
                    }
                    protected virtual void Update()
                    {
                        if (!isUsingOnHit || curCountPiercing == 0)
                            return;

                        foreach (IHitBox hitbox in hitboxesCol)
                        {
                            hitbox.direction = transform.position - beforePos;
                            hitbox.maxDistance = Vector3.Distance(transform.position, beforePos);
                        }

                        var hits = Utility.SweepUnit(hitboxesCol, beforePos, transform.rotation, countPiercing != 1, layerMaskOnCol, unitFlagOnCol);

                        foreach (var hit in hits)
                        {
                            // �浹���� ���� ������� Ȯ��
                            if (savedHitUnit.Add(hit.transform))
                            {
                                Unit victim = hit.transform.GetComponent<Unit>();
                                Vector3 hitPos = Utility.GetHitPoint(beforePos, hit);
                                OnHit?.Invoke(victim, hitPos);

                                // �浹�� ������ ����ϴ°�
                                if (isUsingAttackOnHit)
                                {
                                    // ���������ΰ�?
                                    if (isUsingRAOnHit)
                                    {
                                        var hits2 = Utility.SweepUnit(hitBoxesOnHit, transform.position, transform.rotation, true, layerMaskOnHit, unitFlagOnHit);
                                        foreach (var hit2 in hits2)
                                        {
                                            victim = hit2.transform.GetComponent<Unit>();

                                            shooter.Attack(victim, dmgTypeOnHit, victim.transform.position, false, TotalDamageCalcOnHit);

                                            if (fxPrefabOnHitByColl != null)
                                                PrefabPool.GetObject(fxPrefabOnHitByColl).transform.position = victim.transform.position;
                                        }
                                    }
                                    else
                                    {
                                        shooter.Attack(victim, dmgTypeOnHit, hitPos, false, TotalDamageCalcOnHit);

                                        if (fxPrefabOnHitByColl != null)
                                            PrefabPool.GetObject(fxPrefabOnHitByColl).transform.position = hitPos;
                                    }
                                }
                                // ����Ƚ���� �����°�
                                if (--curCountPiercing == 0)
                                {
                                    OnHitCountOver();
                                    return;
                                }
                            }
                        }

                        beforePos = transform.position;
                    }
#if UNITY_EDITOR
                    protected virtual void OnDrawGizmos()
                    {
                        foreach (var hitbox in hitboxesCol)
                        {
                            hitbox.SetRange(TotalRange);
                            hitbox.DrawGizmos(transform.position, transform.rotation);
                        }
                        foreach (var hitbox in hitBoxesOnHit)
                        {
                            hitbox.SetRange(TotalRange);
                            hitbox.DrawGizmos(transform.position, transform.rotation);
                        }
                        foreach (var hitbox in hitBoxesOnArrive)
                        {
                            hitbox.SetRange(TotalRange);
                            hitbox.DrawGizmos(transform.position, transform.rotation);
                        }
                    }
#endif
                    #endregion

                    #region Methods
                    public void SetAttackOnHit(TotalDamageCalcEvent totalDmgCalc, DAMAGE_TYPE dmgTypeOnHit, int layerMaskOnHit, UNIT_FLAG unitFlagOnHit)
                    {
                        TotalDamageCalcOnHit = totalDmgCalc;
                        this.dmgTypeOnHit = dmgTypeOnHit;
                        this.layerMaskOnHit = layerMaskOnHit;
                        this.unitFlagOnHit = unitFlagOnHit;
                    }
                    public void SetAttackOnArrive(TotalDamageCalcEvent totalDmgCalc, DAMAGE_TYPE dmgTypeOnArrive, int layerMaskOnArrive, UNIT_FLAG unitFlagOnArrive)
                    {
                        TotalDamageCalcOnArriveHit = totalDmgCalc;
                        this.dmgTypeOnArrive = dmgTypeOnArrive;
                        this.layerMaskOnArrive = layerMaskOnArrive;
                        this.unitFlagOnArrive = unitFlagOnArrive;
                    }
                    public virtual void SetProjectile(BaseSkill shooter, Vector3 startPos, Transform target, int layerMaskOnCol, UNIT_FLAG unitFlagOnCol)
                    {
                        SetBaseProjectile(shooter, startPos, layerMaskOnCol, unitFlagOnCol);

                        this.target = target;
                        isTargeting = true;
                    }
                    public virtual void SetProjectile(BaseSkill shooter, Vector3 startPos, Vector3 dest, int layerMaskOnCol, UNIT_FLAG unitFlagOnCol)
                    {
                        SetBaseProjectile(shooter, startPos, layerMaskOnCol, unitFlagOnCol);

                        this.dest = dest;
                        isTargeting = false;
                    }
                    protected void SetBaseProjectile(BaseSkill shooter, Vector3 startPos, int layerMaskOnCol, UNIT_FLAG unitFlagOnCol)
                    {
                        this.shooter = shooter;
                        this.layerMaskOnCol = layerMaskOnCol;
                        this.unitFlagOnCol = unitFlagOnCol;

                        beforePos = transform.position = startPos;

                        ClearEvent();
                    }
                    protected void Arrive()
                    {
                        if (isUsingOnArrive)
                        {
                            if (fxPrefabOnArrive != null)
                                PrefabPool.GetObject(fxPrefabOnArrive).transform.position = transform.position;

                            if (isUsingAttackOnArrive)
                            {
                                Unit victim;
                                if (isUsingRAOnArrive)
                                {
                                    var hits = Utility.SweepUnit(hitBoxesOnArrive, transform.position, transform.rotation, true, layerMaskOnArrive, unitFlagOnArrive);
                                    foreach (var hit in hits)
                                    {
                                        victim = hit.transform.GetComponent<Unit>();
                                        shooter.Attack(victim, dmgTypeOnArrive, victim.transform.position, false, TotalDamageCalcOnArriveHit);
                                        if (fxPrefabOnHitByAriv != null)
                                            PrefabPool.GetObject(fxPrefabOnHitByAriv).transform.position = victim.transform.position;
                                        OnArriveHit?.Invoke(victim, victim.transform.position);
                                    }
                                }
                                else if (isTargeting)
                                {
                                    victim = target.GetComponent<Unit>();
                                    shooter.Attack(victim, dmgTypeOnArrive, victim.transform.position, false, TotalDamageCalcOnArriveHit);
                                    if (fxPrefabOnHitByAriv != null)
                                        PrefabPool.GetObject(fxPrefabOnHitByAriv).transform.position = victim.transform.position;
                                    OnArriveHit?.Invoke(victim, victim.transform.position);
                                }
                            }

                            if (isTargeting)
                                OnArrive?.Invoke(target.position);
                            else
                                OnArrive?.Invoke(dest);
                        }

                        Release();
                    }
                    protected virtual void OnHitCountOver()
                    {
                        if (isLastHitArrival)
                            Arrive();
                        else
                            Release();
                    }
                    public void SetArrive()
                    {
                        dest = transform.position;
                        Arrive();
                    }
                    public virtual void Flip(bool isLeft)
                    {
                        if (IsLookingLeft == isLeft)
                            return;

                        Vector3 scale = transform.localScale;
                        scale.x = -scale.x;
                        transform.localScale = scale;
                    }
                    public void ClearEvent()
                    {
                        OnChangedRange = null;
                        OnArrive = null;
                        OnArriveHit = null;
                        OnHit = null;
                    }
                    protected virtual void SetRange()
                    {
                        foreach (var hitbox in hitboxesCol)
                            hitbox.SetRange(range);
                        foreach (var hitbox in hitBoxesOnHit)
                            hitbox.SetRange(range);
                        foreach (var hitbox in hitBoxesOnArrive)
                            hitbox.SetRange(range);
                    }
                    public virtual Projectile Instantiate(bool active = true)
                    {
                        var p = PrefabPool.GetObject(this, active).GetComponent<Projectile>();

                        p.OnArrive = OnArrive;
                        p.OnArriveHit = OnArriveHit;
                        p.OnHit = OnHit;
                        p.TotalDamageCalcOnHit = TotalDamageCalcOnHit;
                        p.TotalDamageCalcOnArriveHit = TotalDamageCalcOnArriveHit;
                        
                        p.layerMaskOnCol = layerMaskOnCol;
                        p.unitFlagOnCol = unitFlagOnCol;
                        p.dmgTypeOnHit = dmgTypeOnHit;
                        p.layerMaskOnHit = layerMaskOnHit;
                        p.unitFlagOnHit = unitFlagOnHit;
                        p.dmgTypeOnArrive = dmgTypeOnArrive;
                        p.layerMaskOnArrive = layerMaskOnArrive;
                        p.unitFlagOnArrive = unitFlagOnArrive;

                        p.OnChangedRange = SetRange;

                        return p;
                    }
                    #endregion
                }
            }
        }
    }
}
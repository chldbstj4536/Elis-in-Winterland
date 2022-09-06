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
                    [FoldoutGroup("투사체")]
                    [BoxGroup("투사체/투사체 설정"), SerializeField]
                    [LabelText("충돌 이벤트 사용"), Tooltip("투사체가 대상과 충돌 시 이벤트를 발생시킬지를 정합니다.\n사용하지 않는다면 OnHit이벤트는 발생하지 않습니다.")]
                    private bool isUsingOnHit;
                    [BoxGroup("투사체/투사체 설정"), SerializeField]
                    [LabelText("도착 이벤트 사용"), Tooltip("투사체가 목적지에 도착 시 이벤트를 발생시킬지를 정합니다.\n사용하지 않는다면 OnArrive이벤트는 발생하지 않습니다.")]
                    private bool isUsingOnArrive;
                    [BoxGroup("투사체/투사체 설정"), SerializeField, Min(0.0f)]
                    [LabelText("사거리/범위"), Tooltip("투사체의 히트박스들에 적용되는 Range(범위)값을 설정합니다")]
                    private float range;
                    private float rangeAdd = 0.0f;
                    private float rangeCoef = 1.0f;

                    [FoldoutGroup("투사체/충돌 설정"), ShowIf("isUsingOnHit"), SerializeReference]
                    [LabelText("충돌 범위(히트 박스)")]
                    protected IHitBox[] hitboxesCol = new IHitBox[0];
                    [FoldoutGroup("투사체/충돌 설정"), Min(0), ShowIf("isUsingOnHit"), SerializeField]
                    [LabelText("관통 횟수"), Tooltip("투사체가 몇번 대상을 관통할지에 대한 설정\n0이면 항상 관통")]
                    protected int countPiercing = 1;
                    [FoldoutGroup("투사체/충돌 설정"), ShowIf("isUsingOnHit"), SerializeField]
                    [LabelText("충돌 시 공격")]
                    private bool isUsingAttackOnHit;
                    [FoldoutGroup("투사체/충돌 설정/공격 설정"), ShowIf("@isUsingOnHit && isUsingAttackOnHit"), SerializeField]
                    [LabelText("히트 효과 프리팹")]
                    private AutoReleaseParticlePrefab fxPrefabOnHitByColl;
                    [FoldoutGroup("투사체/충돌 설정/공격 설정"), ShowIf("@isUsingOnHit && isUsingAttackOnHit"), SerializeField]
                    [LabelText("범위 공격")]
                    private bool isUsingRAOnHit;
                    [FoldoutGroup("투사체/충돌 설정/공격 설정"), ShowIf("@isUsingOnHit && isUsingAttackOnHit && isUsingRAOnHit"), SerializeReference]
                    [LabelText("공격 범위")]
                    private IHitBox[] hitBoxesOnHit = new IHitBox[0];

                    [FoldoutGroup("투사체/도착 설정"), ShowIf("isUsingOnArrive"), EnableIf("@isUsingOnArrive && isUsingOnHit"), SerializeField]
                    [LabelText("투사체 파괴 시 도착판정"), Tooltip("관통 횟수가 끝나 투사체가 사라질때 도착 이벤트가 발생할지")]
                    private bool isLastHitArrival;
                    [FoldoutGroup("투사체/도착 설정"), ShowIf("@isUsingOnArrive"), SerializeField]
                    [LabelText("도착 시 효과 프리팹")]
                    private AutoReleaseParticlePrefab fxPrefabOnArrive;
                    [FoldoutGroup("투사체/도착 설정"), ShowIf("isUsingOnArrive"), SerializeField]
                    [LabelText("도착 시 공격")]
                    private bool isUsingAttackOnArrive;
                    [FoldoutGroup("투사체/도착 설정/공격 설정"), ShowIf("@isUsingOnArrive && isUsingAttackOnArrive"), SerializeField]
                    [LabelText("히트 효과 프리팹")]
                    private AutoReleaseParticlePrefab fxPrefabOnHitByAriv;
                    [FoldoutGroup("투사체/도착 설정/공격 설정"), ShowIf("@isUsingOnArrive && isUsingAttackOnArrive"), SerializeField]
                    [LabelText("범위 공격"), Tooltip("타겟팅 공격이 아니라면 범위 공격 설정이 안되어있을시 피해를 입히지 않습니다.")]
                    private bool isUsingRAOnArrive;
                    [FoldoutGroup("투사체/도착 설정/공격 설정"), ShowIf("@isUsingOnArrive && isUsingAttackOnArrive && isUsingRAOnArrive"), SerializeReference]
                    [LabelText("공격 범위")]
                    private IHitBox[] hitBoxesOnArrive = new IHitBox[0];

                    protected BaseSkill shooter;
                    [BoxGroup("투사체/디버깅(적용X)", true, true, 10), ShowIf("isTargeting"), ShowInInspector]
                    protected Transform target;
                    [BoxGroup("투사체/디버깅(적용X)", true, true, 10), HideIf("isTargeting"), ShowInInspector]
                    protected Vector3 dest;
                    [BoxGroup("투사체/디버깅(적용X)", true, true, 10), ShowInInspector]
                    protected bool isTargeting;

                    // 충돌에 대한 레이어 및 유닛플래그
                    private int layerMaskOnCol;
                    private UNIT_FLAG unitFlagOnCol;

                    // 충돌 공격에 대한 피해량, 레이어 및 유닛플래그
                    private DAMAGE_TYPE dmgTypeOnHit;
                    private int layerMaskOnHit;
                    private UNIT_FLAG unitFlagOnHit;

                    // 도착 공격에 대한 피해량, 레이어 및 유닛플래그
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

                    // 목적지(타겟)에 도착했을때 호출
                    public event OnArriveEvent OnArrive;
                    // 목적지(타겟)에 도착 후 피해대상 호출
                    public event OnHitEvent OnArriveHit;
                    // 투사체와 충돌 시 호출
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
                            // 충돌하지 않은 대상인지 확인
                            if (savedHitUnit.Add(hit.transform))
                            {
                                Unit victim = hit.transform.GetComponent<Unit>();
                                Vector3 hitPos = Utility.GetHitPoint(beforePos, hit);
                                OnHit?.Invoke(victim, hitPos);

                                // 충돌시 공격을 사용하는가
                                if (isUsingAttackOnHit)
                                {
                                    // 범위공격인가?
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
                                // 관통횟수가 끝났는가
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
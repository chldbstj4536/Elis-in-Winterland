using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Sirenix.OdinInspector;

namespace YS
{
    [RequireComponent(typeof(MoveComponent))]
    public abstract partial class Unit : BaseObject
    {
        #region Field
        private int                 level;
        protected UNIT_TYPE         type;

        #region 스탯
        private int                 baseMaxHP;                      // 기본 최대 체력
        private float               baseHPRegen;                    // 기본 초당 체력 재생
        private int                 baseMaxMP;                      // 기본 최대 마나
        private float               baseMPRegen;                    // 기본 초당 마나 재생
        private int                 physxPower;                     // 물리공격력
        private int                 magicPower;                     // 주문력
        private float               armor;                          // 방어력
        private float               armorPnt;                       // 물리관통력
        private float               atkSpd;                         // 공격 속도
        private int                 criticalRate;                   // 치명타율 (두배 피해량)
        private float               cdr;                            // 쿨타임 감소
        private int                 tempVar;                        // 온도증감계수
        #endregion
        #region 스탯 변화량
        private int                 hpAdd               = 0;        // 추가 체력
        private float               hpCoef              = 1.0f;     // 기본 체력 계수
        private float               hpRegenAdd          = 0.0f;     // 추가 초당 체력 재생
        private float               hpRegenCoef         = 1.0f;     // 초당 체력 재생 계수
        private int                 mpAdd               = 0;        // 추가 마나
        private float               mpCoef              = 1.0f;     // 기본 마나 계수
        private float               mpRegenAdd          = 0.0f;     // 추가 초당 마나 재생
        private float               mpRegenCoef         = 1.0f;     // 초당 체력 마나 계수
        private int                 physxPowerAdd       = 0;        // 추가 물리공격력
        private float               physxPowerCoef      = 1.0f;     // 물리공격력 계수
        private int                 magicPowerAdd       = 0;        // 추가 마법공격력
        private float               magicPowerCoef      = 1.0f;     // 마법공격력 계수
        private float               armorAdd            = 0.0f;     // 추가 방어력
        private float               armorPntAdd         = 0.0f;     // 추가 관통력
        private float               atkSpdCoef          = 1.0f;     // 공격 속도 계수
        private int                 criticalRateAdd     = 0;        // 치명타 증가량
        private float               cdrAdd              = 0.0f;     // 쿨타임 감소량
        private float               totalTakeDmgRate    = 1.0f;     // 받는 피해량 증감률
        private float               totalDmgRate        = 1.0f;     // 가하는 피해량 증감률
        #endregion
        private float               curHP;
        private float               curMP;
        #region 스탯 이벤트
        public event OnChangedValue OnChangedLevel;

        public event OnChangedValue OnChangedBaseMaxHP;
        public event OnChangedValue OnChangedBaseHPRegen;
        public event OnChangedValue OnChangedBaseMaxMP;
        public event OnChangedValue OnChangedBaseMPRegen;
        public event OnChangedValue OnChangedBasePhysicsPower;
        public event OnChangedValue OnChangedBaseMagicPower;
        public event OnChangedValue OnChangedBaseArmor;
        public event OnChangedValue OnChangedBaseArmorPnt;
        public event OnChangedValue OnChangedBaseAttackSpeed;
        public event OnChangedValue OnChangedBaseCriticalRate;
        public event OnChangedValue OnChangedBaseCDR;
        public event OnChangedValue OnChangedTemperatureVarious;

        public event OnChangedValue OnChangedHPAdd;
        public event OnChangedValue OnChangedHPCoef;
        public event OnChangedValue OnChangedHPRegenAdd;
        public event OnChangedValue OnChangedHPRegenCoef;
        public event OnChangedValue OnChangedMPAdd;
        public event OnChangedValue OnChangedMPCoef;
        public event OnChangedValue OnChangedMPRegenAdd;
        public event OnChangedValue OnChangedMPRegenCoef;
        public event OnChangedValue OnChangedPhysicsPowerAdd;
        public event OnChangedValue OnChangedPhysicsPowerCoef;
        public event OnChangedValue OnChangedMagicPowerAdd;
        public event OnChangedValue OnChangedMagicPowerCoef;
        public event OnChangedValue OnChangedArmorAdd;
        public event OnChangedValue OnChangedArmorPntAdd;
        public event OnChangedValue OnChangedAttackSpeedCoef;
        public event OnChangedValue OnChangedCriticalRateAdd;
        public event OnChangedValue OnChangedCDRAdd;
        public event OnChangedValue OnChangedTotalTakeDmgRate;
        public event OnChangedValue OnChangedTotalDmgRate;

        public event OnChangedValue OnChangedMaxHP;
        public event OnChangedValue OnChangedMaxMP;
        public event OnChangedValue OnChangedCurrentHP;
        public event OnChangedValue OnChangedCurrentMP;

        public event OnChangedValue OnChangedTotalHPRegen;
        public event OnChangedValue OnChangedTotalMPRegen;
        public event OnChangedValue OnChangedTotalPhysicsPower;
        public event OnChangedValue OnChangedTotalMagicPower;
        public event OnChangedValue OnChangedTotalArmor;
        public event OnChangedValue OnChangedTotalArmorPnt;
        public event OnChangedValue OnChangedTotalAttackSpeed;
        public event OnChangedValue OnChangedTotalCriticalRate;
        public event OnChangedValue OnChangedTotalCDR;
        #endregion

        [HideInInspector]
        public UnitInfoBar unitInfoBar;

        protected AnimationPlayer mainAnimPlayer;
        protected AnimationPlayer moveAnimPlayer;
        protected AnimationPlayer stopAnimPlayer;

        protected SkeletonAnimation anim;
        protected Spine.AnimationState animState;
        protected string skinName;

        private bool isInvincible; // 무적상태

        // 현재 걸린 상태 이상
        [ShowInInspector, DisableIf("@true"), BoxGroup("Debug", true, true, 1000)]
        protected RESTRICTION_FLAG restrictionFlag = RESTRICTION_FLAG.NONE;
        // 도발 대상
        protected Unit tauntUnit;
        // 현재 공격중인 스킬
        [ShowInInspector, DisableIf("@true"), BoxGroup("Debug")]
        protected Skill.ActiveSkill curSkill;
        // 현재 영향받고있는 패시브 효과들
        [ShowInInspector, DisableIf("@true"), BoxGroup("Debug")]
        protected Dictionary<int, PassiveSkillStateData> curPSkills = new Dictionary<int, PassiveSkillStateData>();
        // 현재 보호해주는 쉴드들
        [ShowInInspector, DisableIf("@true"), BoxGroup("Debug")]
        protected Dictionary<int, BaseShield> curShields = new Dictionary<int, BaseShield>();

        [SerializeField, LabelText("원거리 유닛인가?")]
        private bool isRangedUnit = false;
        [SerializeField, LabelText("기본공격 투사체 프리팹"), ShowIf("isRangedUnit")]
        private Skill.BaseSkill.Projectile projectileDAPrefab;
        [SerializeField, LabelText("기본공격 투사체 속도"), Min(0.0001f), ShowIf("@projectileDAPrefab != null")]
        private float projectileDASpeed = 1.0f;

        protected Skill defaultAttack;
        protected Skill dashSkill;
        protected Skill ultimateSkill;
        protected Skill[] skills;

        public event Delegate_NoRetVal_NoParam OnDisableEvent;
        public event Delegate_NoRetVal_NoParam UpdateEvent;
        public event Skill.BaseSkill.OnAttackUnit OnBeforeHitEvent;
        public event Skill.BaseSkill.OnAttackUnit OnAfterHitEvent;
#if UNITY_EDITOR
        public event Delegate_NoRetVal_NoParam OnGizmosEvent;
#endif
        [HideInInspector]
        public MoveComponent mc;
        protected BaseUnitAI fsm;

        protected GameManager gm;
        #endregion

        #region Properties
        #region 스탯
        public int BaseMaxHP
        {
            get { return baseMaxHP; }
            protected set
            {
                baseMaxHP = value;
                OnChangedBaseMaxHP?.Invoke();
                OnChangedMaxHP?.Invoke();
            }
        }
        public float BaseHPRegen
        {
            get { return baseHPRegen; }
            protected set
            {
                baseHPRegen = value;
                OnChangedBaseHPRegen?.Invoke();
                OnChangedTotalHPRegen?.Invoke();
            }
        }
        public int BaseMaxMP
        {
            get { return baseMaxMP; }
            protected set
            {
                baseMaxMP = value;
                OnChangedBaseMaxMP?.Invoke();
                OnChangedMaxMP?.Invoke();
            }
        }
        public float BaseMPRegen
        {
            get { return baseMPRegen; }
            protected set
            {
                baseMPRegen = value;
                OnChangedBaseMPRegen?.Invoke();
                OnChangedTotalMPRegen?.Invoke();
            }
        }
        public int BasePhysxPower
        {
            get { return physxPower; }
            protected set
            {
                physxPower = value;
                OnChangedBasePhysicsPower?.Invoke();
                OnChangedTotalPhysicsPower?.Invoke();
            }
        }
        public int BaseMagicPower
        {
            get { return magicPower; }
            protected set
            {
                magicPower = value;
                OnChangedBaseMagicPower?.Invoke();
                OnChangedTotalMagicPower?.Invoke();
            }
        }
        public float BaseArmor
        {
            get { return armor; }
            protected set
            {
                armor = value;
                OnChangedBaseArmor?.Invoke();
                OnChangedTotalArmor?.Invoke();
            }
        }
        public float BaseArmorPnt
        {
            get { return armorPnt; }
            protected set
            {
                armorPnt = value;
                OnChangedBaseArmorPnt?.Invoke();
                OnChangedTotalArmorPnt?.Invoke();
            }
        }
        public float BaseAttackSpeed
        {
            get { return atkSpd; }
            protected set
            {
                atkSpd = value;
                OnChangedBaseAttackSpeed?.Invoke();
                OnChangedTotalAttackSpeed?.Invoke();
            }
        }
        public int BaseCriticalRate
        {
            get { return criticalRate; }
            protected set
            {
                criticalRate = value;
                OnChangedBaseCriticalRate?.Invoke();
                OnChangedTotalCriticalRate?.Invoke();
            }
        }
        public float BaseCDR
        {
            get { return cdr; }
            protected set
            {
                cdr = value;
                OnChangedBaseCDR?.Invoke();
                OnChangedTotalCDR?.Invoke();
            }
        }
        public int TemperatureVarious
        {
            get { return tempVar; }
            protected set
            {
                gm.Temperature += value - tempVar;
                tempVar = value;
                OnChangedTemperatureVarious?.Invoke();
            }
        }
        #endregion
        #region 스탯 변화량
        public int HPAdd
        {
            get { return hpAdd; }
            protected set
            {
                // 변경 전 최대 HP
                int hp = MaxHP;
                hpAdd = value;
                // 변경 후 최대 HP를 빼서 변화량 구하기
                hp -= MaxHP;

                if (hp < 0)
                    CurrentHP -= hp;
                else if (CurrentHP > MaxHP)
                    CurrentHP = MaxHP;

                OnChangedHPAdd?.Invoke();
                OnChangedMaxHP?.Invoke();
            }
        }
        public float HPCoef
        {
            get { return hpCoef; }
            protected set
            {
                // 변경 전 최대 HP
                int hp = MaxHP;
                hpCoef = value;
                // 변경 후 최대 HP를 빼서 변화량 구하기
                hp -= MaxHP;

                if (hp < 0)
                    CurrentHP -= hp;
                else if (CurrentHP > MaxHP)
                    CurrentHP = MaxHP;

                OnChangedHPCoef?.Invoke();
                OnChangedMaxHP?.Invoke();
            }
        }
        public float HPRegenAdd
        {
            get { return hpRegenAdd; }
            protected set
            {
                hpRegenAdd = value;
                OnChangedHPRegenAdd?.Invoke();
                OnChangedTotalHPRegen?.Invoke();
            }
        }
        public float HPRegenCoef
        {
            get { return hpRegenCoef; }
            protected set
            {
                hpRegenCoef = value;
                OnChangedHPRegenCoef?.Invoke();
                OnChangedTotalHPRegen?.Invoke();
            }
        }
        public int MPAdd
        {
            get { return mpAdd; }
            protected set
            {
                // 변경 전 최대 MP
                int mp = MaxMP;
                mpAdd = value;
                // 변경 후 최대 MP를 빼서 변화량 구하기
                mp -= MaxMP;

                if (mp < 0)
                    CurrentMP -= mp;
                else if (CurrentMP > MaxMP)
                    CurrentMP = MaxMP;

                OnChangedMPAdd?.Invoke();
                OnChangedMaxMP?.Invoke();
            }
        }
        public float MPCoef
        {
            get { return mpCoef; }
            protected set
            {
                // 변경 전 최대 MP
                int mp = MaxMP;
                mpCoef = value;
                // 변경 후 최대 MP를 빼서 변화량 구하기
                mp -= MaxMP;

                if (mp < 0)
                    CurrentMP -= mp;
                else if (CurrentMP > MaxMP)
                    CurrentMP = MaxMP;

                OnChangedMPCoef?.Invoke();
                OnChangedMaxMP?.Invoke();
            }
        }
        public float MPRegenAdd
        {
            get { return mpRegenAdd; }
            protected set
            {
                mpRegenAdd = value;
                OnChangedMPRegenAdd?.Invoke();
                OnChangedTotalMPRegen?.Invoke();
            }
        }
        public float MPRegenCoef
        {
            get { return mpRegenCoef ; }
            protected set
            {
                mpRegenCoef = value;
                OnChangedMPRegenCoef?.Invoke();
                OnChangedTotalMPRegen?.Invoke();
            }
        }
        public int PhysicsPowerAdd
        {
            get { return physxPowerAdd; }
            protected set
            {
                physxPowerAdd = value;
                OnChangedPhysicsPowerAdd?.Invoke();
                OnChangedTotalPhysicsPower?.Invoke();
            }
        }
        public float PhysicsPowerCoef
        {
            get { return physxPowerCoef; }
            protected set
            {
                physxPowerCoef = value;
                OnChangedPhysicsPowerCoef?.Invoke();
                OnChangedTotalPhysicsPower?.Invoke();
            }
        }
        public int MagicPowerAdd
        {
            get { return magicPowerAdd; }
            protected set
            {
                magicPowerAdd = value;
                OnChangedMagicPowerAdd?.Invoke();
                OnChangedTotalMagicPower?.Invoke();
            }
        }
        public float MagicPowerCoef
        {
            get { return magicPowerCoef; }
            protected set
            {
                magicPowerCoef = value;
                OnChangedMagicPowerCoef?.Invoke();
                OnChangedTotalMagicPower?.Invoke();
            }
        }
        public float ArmorAdd
        {
            get { return armorAdd; }
            protected set
            {
                armorAdd = value;
                OnChangedArmorAdd?.Invoke();
                OnChangedTotalArmor?.Invoke();
            }
        }
        public float ArmorPntAdd
        {
            get { return armorPntAdd; }
            protected set
            {
                armorPntAdd = value;
                OnChangedArmorPntAdd?.Invoke();
                OnChangedTotalArmorPnt?.Invoke();
            }
        }
        public float AttackSpeedCoef
        {
            get { return atkSpdCoef; }
            protected set
            {
                atkSpdCoef = value;
                OnChangedAttackSpeedCoef?.Invoke();
                OnChangedTotalAttackSpeed?.Invoke();
            }
        }
        public int CriticalRateAdd
        {
            get { return criticalRateAdd; }
            protected set
            {
                criticalRateAdd = value;
                OnChangedCriticalRateAdd?.Invoke();
                OnChangedTotalCriticalRate?.Invoke();
            }
        }
        public float CDRAdd
        {
            get { return cdrAdd; }
            protected set
            {
                cdrAdd = value;
                OnChangedCDRAdd?.Invoke();
                OnChangedTotalCDR?.Invoke();
            }
        }
        public float TotalTakeDmgRate
        {
            get { return totalTakeDmgRate; }
            protected set
            {
                totalTakeDmgRate = value;
                OnChangedTotalTakeDmgRate?.Invoke();
            }
        }
        public float TotalDmgRate
        {
            get { return totalDmgRate; }
            protected set
            {
                totalDmgRate = value;
                OnChangedTotalDmgRate?.Invoke();
            }
        }
        #endregion
        [ShowInInspector, DisableIf("@true")]
        [BoxGroup("유닛")]
        public float CurrentHP
        {
            get { return curHP; }
            protected set
            {
                curHP = value;
                OnChangedCurrentHP?.Invoke();
            }
        }
        public float CurrentMP
        {
            get { return curMP; }
            protected set
            {
                curMP = value;
                OnChangedCurrentMP?.Invoke();
            }
        }
        public int Level
        {
            get { return level; }
            protected set
            {
                level = value;
                OnChangedLevel?.Invoke();
            }
        }
        public UNIT_TYPE UnitType => type;
        public abstract UNIT_ATTRIBUTE Attribute { get; }
        public abstract UNIT_SIZE Size { get; }
        [ShowInInspector]
        [BoxGroup("유닛")]
        public int MaxHP => (int)(baseMaxHP * hpCoef) + hpAdd;
        public int MaxMP => (int)(baseMaxMP * mpCoef) + mpAdd;
        public float TotalHPRegen => (int)(baseHPRegen * hpRegenCoef) + hpRegenAdd;
        public float TotalMPRegen => (int)(baseMPRegen * mpRegenCoef) + mpRegenAdd;
        public int TotalPhysicsPower => (int)(physxPower * physxPowerCoef) + physxPowerAdd;
        public float TotalArmorPnt => armorPnt + armorPntAdd;
        public float TotalAttackSpeed => atkSpd * atkSpdCoef;
        public int TotalMagicPower => (int)(magicPower * magicPowerCoef) + magicPowerAdd;
        public float TotalArmor => armor + armorAdd;
        public float TotalCriticalRate => criticalRate + criticalRateAdd;
        public float TotalCDR => cdr + cdrAdd;
        public float TotalMovementSpeed => mc.TotalMoveSpeed;
        public Skill[] Skills => skills;
        public Dictionary<int, PassiveSkillStateData> CurrentPSkills => curPSkills;
        public override bool IsLookingLeft => anim.transform.localScale.x > 0.0f;
        public abstract AnimationTrackSet[] AnimationSet_Idle { get; }
        public abstract AnimationTrackSet[] AnimationSet_Move { get; }
        public abstract AnimationTrackSet[] AnimationSet_Die { get; }
        public bool IsInvincible
        {
            get => isInvincible;
            set => isInvincible = value;
        }
        public bool IsRangedUnit => isRangedUnit;
        public bool IsTaunt => Utility.HasFlag((int)restrictionFlag, (int)RESTRICTION_FLAG.TAUNT);
        public bool IsMovable => !Utility.HasFlag((int)restrictionFlag, (int)RESTRICTION_FLAG.MOVE);
        public bool IsAttackable => !Utility.HasFlag((int)restrictionFlag, (int)RESTRICTION_FLAG.ATTACK);
        #endregion

        #region Unity Methods
        protected virtual void Update()
        {
            if (fsm.CurrentStateIndex == FSM.STATE_INDEX.DIE)
                return;

            restrictionFlag = RESTRICTION_FLAG.NONE;

            foreach (var passiveSkill in curPSkills.Values)
                restrictionFlag |= passiveSkill.pSkill.RestrictionFlag;

            if (TotalHPRegen != 0 && CurrentHP != MaxHP)
            {
                CurrentHP += TotalHPRegen * Time.deltaTime;
                CurrentHP = Mathf.Min(CurrentHP, MaxHP);
            }
            if (TotalMPRegen != 0 && CurrentMP != MaxMP)
            {
                CurrentMP += TotalMPRegen * Time.deltaTime;
                CurrentMP = Mathf.Min(CurrentMP, MaxMP);
            }

            UpdateEvent?.Invoke();
        }
        protected virtual void OnEnable()
        {
            for (int i = 0; i < hitboxCols.Length; ++i)
                hitboxCols[i].enabled = true;

            CurrentHP = MaxHP;
            CurrentMP = MaxMP;
        }
        protected virtual void OnDisable()
        {
            OnDisableEvent?.Invoke();

            fsm = null;
            OnDisableEvent = null;
            UpdateEvent = null;
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            OnGizmosEvent?.Invoke();
        }
#endif
        #endregion

        #region Methods
        public override void OnInstantiate()
        {
            base.OnInstantiate();

            gm = GameManager.Instance;
            mc = GetComponent<MoveComponent>();
            anim = transform.GetChild(0).GetComponent<SkeletonAnimation>();
            animState = anim.AnimationState;
            unitInfoBar = transform.GetChild(1).GetComponent<UnitInfoBar>();

            mainAnimPlayer = new AnimationPlayer(animState);
            moveAnimPlayer = new AnimationPlayer(animState);
            stopAnimPlayer = new AnimationPlayer(animState);

            mc.OnMoveStart += OnMoveStart;
            mc.OnMovingEvent += OnMoving;
            mc.OnArrive += OnArrive;
            mc.OnStop += OnStop;

            // 투사체 복사본 생성 후 이를 풀링하는 식으로 사용
            if (projectileDAPrefab != null)
            {
                projectileDAPrefab = Instantiate(projectileDAPrefab, transform);
                projectileDAPrefab.Speed = projectileDASpeed;
            }
        }
        protected virtual void OnMoveStart()
        {
            stopAnimPlayer.Stop();
            moveAnimPlayer.Play();
        }
        protected virtual void OnMoving(Vector3 lastPos, Vector3 curPos)
        {
            if (fsm.CurrentStateIndex == FSM.STATE_INDEX.IDLE || fsm.CurrentStateIndex == FSM.STATE_INDEX.SELECTING_TARGET)
                Utility.FlipUnitMoveDir(this);
        }
        protected virtual void OnArrive()
        {
        }
        protected virtual void OnStop()
        {
            moveAnimPlayer.Stop();
            stopAnimPlayer.Play();
        }
        public override void Flip(bool isLeft)
        {
            if (IsLookingLeft == isLeft)
                return;

            Vector3 scale = anim.transform.localScale;
            scale.x = -scale.x;
            anim.transform.localScale = scale;
        }
        protected virtual SKILL_ERROR_CODE CastSkill(Skill newSkill)
        {
            if (newSkill == null || newSkill.ASkill == null)
                return SKILL_ERROR_CODE.INVALID_SKILL;

            return newSkill.ASkill.CastSkill();
        }
        public void AddShield(BaseShield shield)
        {
            if (!curShields.ContainsKey(shield.GetInstanceID()))
                curShields.Add(shield.GetInstanceID(), shield);
        }
        public void RemoveShield(BaseShield shield)
        {
            curShields.Remove(shield.GetInstanceID());
        }
        protected virtual void OnDie(Unit killer)
        {
            TemperatureVarious = 0;
            for (int i = 0; i < hitboxCols.Length; ++i)
                hitboxCols[i].enabled = false;
            fsm.ChangeState(fsm.stateDie);
        }
        protected virtual void SetChangableStat(List<UnitChangableStatData> changeStat, int multiple)
        {
            if (multiple == 0)
                return;

            foreach (var stat in changeStat)
            {
                switch (stat.stat)
                {
                    case UNIT_CHANGABLE_STAT.HP:
                        HPAdd += (int)stat.value * multiple;
                        break;
                    case UNIT_CHANGABLE_STAT.MP:
                        MPAdd += (int)stat.value * multiple;
                        break;
                    case UNIT_CHANGABLE_STAT.PHYSICS_POWER:
                        PhysicsPowerAdd += (int)stat.value * multiple;
                        break;
                    case UNIT_CHANGABLE_STAT.MAGIC_POWER:
                        MagicPowerAdd += (int)stat.value * multiple;
                        break;
                    case UNIT_CHANGABLE_STAT.ARMOR:
                        ArmorAdd += stat.value * multiple;
                        break;
                    case UNIT_CHANGABLE_STAT.ARMOR_PENETRATION:
                        ArmorPntAdd += stat.value * multiple;
                        break;
                    case UNIT_CHANGABLE_STAT.ATTACK_SPEED:
                        AttackSpeedCoef += stat.value * multiple;
                        break;
                    case UNIT_CHANGABLE_STAT.MOVE_SPEED:
                        mc.MoveSpeedCoef += stat.value * multiple;
                        break;
                    case UNIT_CHANGABLE_STAT.CRITICAL_RATE:
                        CriticalRateAdd += (int)stat.value * multiple;
                        break;
                    case UNIT_CHANGABLE_STAT.COOLDOWN_REDUCTION:
                        CDRAdd += stat.value * multiple;
                        break;
                }
            }
        }
        #endregion

        public struct PassiveSkillStateData
        {
            public Skill.PassiveSkill pSkill;
            public int areaStack;
            public int tickStack;
            public float remainTime;

            public PassiveSkillStateData(Skill.PassiveSkill pSkill, int areaStack, int tickStack, float remainTime)
            {
                this.pSkill = pSkill;
                this.areaStack = areaStack;
                this.tickStack = tickStack;
                this.remainTime = remainTime;
            }
        }

        [System.Serializable]
        public abstract class UnitData : BaseObjectData
        {
            #region Field
            [FoldoutGroup("유닛")]
            [LabelText("속성"), Tooltip("유닛의 속성을 정합니다.")]
            public UNIT_ATTRIBUTE attribute;
            [FoldoutGroup("유닛")]
            [LabelText("크기타입"), Tooltip("유닛의 크기타입을 정합니다.")]
            public UNIT_SIZE size;

            #region 스탯
            [FoldoutGroup("유닛")]
            [LabelText("기본 최대 HP"), Tooltip("유닛의 기본 최대 HP를 정합니다.\n기본 최대 HP는 다양한 체력 증감 효과의 기본이 됩니다.")]
            public int baseMaxHP;
            [FoldoutGroup("유닛")]
            [LabelText("기본 최대 MP"), Tooltip("유닛의 기본 최대 MP를 정합니다.\n기본 최대 MP는 다양한 마나 증감 효과의 기본이 됩니다.")]
            public int baseMaxMP;
            [FoldoutGroup("유닛")]
            [LabelText("기본 초당 HP 재생속도"), Tooltip("유닛의 기본 초당 HP 재생속도 정합니다.\n기본 초당 HP 재생속도는 다양한 초당 HP 재생속도의 기본이 됩니다.")]
            public float baseHPRegen;
            [FoldoutGroup("유닛")]
            [LabelText("기본 초당 MP 재생속도"), Tooltip("유닛의 기본 초당 MP 재생속도 정합니다.\n기본 초당 MP 재생속도는 다양한 초당 MP 재생속도의 기본이 됩니다.")]
            public float baseMPRegen;
            [FoldoutGroup("유닛")]
            [LabelText("물리 공격력"), Tooltip("유닛의 물리 공격력을 정합니다.\n물리 공격력은 다양한 물리 공격력 증감 효과의 기본이 됩니다.")]
            public int physxPower;
            [FoldoutGroup("유닛")]
            [LabelText("마법 공격력"), Tooltip("유닛의 마법 공격력을 정합니다.\n마법 공격력은 다양한 마법 공격력 증감 효과의 기본이 됩니다.")]
            public int magicPower;
            [FoldoutGroup("유닛")]
            [LabelText("방어력"), Tooltip("유닛의 방어력을 정합니다.\n방어력은 다양한 방어력 증감 효과의 기본이 됩니다.")]
            public float armor;
            [FoldoutGroup("유닛")]
            [LabelText("방어관통력"), Tooltip("유닛의 방어관통력을 정합니다.\n방어관통력은 다양한 방어관통력 증감 효과의 기본이 됩니다.")]
            public float armorPnt;
            [FoldoutGroup("유닛")]
            [LabelText("공격속도"), Tooltip("유닛의 초당 공격속도를 정합니다. (1.0 = 1초에 한번 공격, 2.0 = 1초에 2번 공격) 2\n공격속도는 다양한 공격속도 증감 효과의 기본이 됩니다.")]
            public float atkSpd;
            [FoldoutGroup("유닛")]
            [LabelText("이동속도"), Tooltip("유닛의 이동속도를 정합니다.\n이동속도는 다양한 이동속도 증감 효과의 기본이 됩니다.")]
            public float moveSpeed;
            [FoldoutGroup("유닛"), Min(0)]
            [LabelText("치명타율"), Tooltip("유닛의 치명타율을 정합니다.\n치명타율은 다양한 치명타율 증감 효과의 기본이 됩니다.")]
            public int criticalRate;
            [FoldoutGroup("유닛"), Range(0.0f, 1.0f)]
            [LabelText("쿨타임 감소율"), Tooltip("유닛의 쿨타임 감소율을 정합니다.\n범위 : [0, 1] => [0% ~ 100%]\n쿨타임 감소율은 다양한 쿨타임 감소율 증감 효과의 기본이 됩니다.")]
            public float cooldownReduction;
            [FoldoutGroup("유닛")]
            [LabelText("온도 변화량"), Tooltip("유닛의 온도 변화량을 정합니다.\n온도 변화량은 해당 유닛이 필드에 존재할 때 글로벌 온도에 얼마만큼의 영향을 주는지에 대한 값입니다.")]
            public int tempVar;
            #endregion
            #region 레벨당 스탯 변화량
            [FoldoutGroup("유닛")]
            [LabelText("레벨당 기본 최대 HP 증가량"), LabelWidth(200.0f)]
            public int incHpPerLv;
            [FoldoutGroup("유닛")]
            [LabelText("레벨당 기본 최대 MP 증가량"), LabelWidth(200.0f)]
            public int incMpPerLv;
            [FoldoutGroup("유닛")]
            [LabelText("레벨당 기본 초당 HP 재생속도"), LabelWidth(200.0f)]
            public float incHPRegenPerLv;
            [FoldoutGroup("유닛")]
            [LabelText("레벨당 기본 초당 MP 재생속도"), LabelWidth(200.0f)]
            public float incMPRegenPerLv;
            [FoldoutGroup("유닛")]
            [LabelText("레벨당 물리 공격력 증가량"), LabelWidth(200.0f)]
            public int incPhysxPowerPerLv;
            [FoldoutGroup("유닛")]
            [LabelText("레벨당 마법 공격력 증가량"), LabelWidth(200.0f)]
            public int incMagicPowerPerLv;
            [FoldoutGroup("유닛")]
            [LabelText("레벨당 방어력 증가량"), LabelWidth(200.0f)]
            public float incArmorPerLv;
            [FoldoutGroup("유닛")]
            [LabelText("레벨당 방어관통력 증가량"), LabelWidth(200.0f)]
            public float incArmorPntPerLv;
            [FoldoutGroup("유닛")]
            [LabelText("레벨당 공격속도 증가량"), LabelWidth(200.0f)]
            public float incAtkSpdPerLv;
            [FoldoutGroup("유닛")]
            [LabelText("레벨당 이동속도 증가량"), LabelWidth(200.0f)]
            public float incMoveSpeed;
            [FoldoutGroup("유닛"), Min(0)]
            [LabelText("레벨당 치명타율 증가량"), LabelWidth(200.0f)]
            public int incCriticalRatePerLv;
            [FoldoutGroup("유닛")]
            [LabelText("레벨당 쿨타임 감소율 증가량"), LabelWidth(200.0f)]
            public float incCdrPerLv;
            #endregion

            [FoldoutGroup("스파인", true, 50)]
            [BoxGroup("스파인/대기 애니메이션", true), HideLabel, Tooltip("대기 시 재생할 애니메이션 에셋을 선택합니다.")]
            public AnimationTrackSet[] animSetIdle;
            [BoxGroup("스파인/이동 애니메이션", true), HideLabel, Tooltip("이동 시 재생할 애니메이션 에셋을 선택합니다.")]
            public AnimationTrackSet[] animSetMove;
            [BoxGroup("스파인/사망 애니메이션", true), HideLabel, Tooltip("사망 시 재생할 애니메이션 에셋을 선택합니다.")]
            public AnimationTrackSet[] animSetDie;

            [FoldoutGroup("유닛")]
            [LabelText("기본공격 데이터"), Tooltip("유닛의 공격속도에 영향받는 공격(스킬)입니다.")]
            public SkillSO defaultAttackData;
            [FoldoutGroup("유닛")]
            [LabelText("스킬 데이터들"), Tooltip("유닛의 쿨타입감소율에 영향받는 스킬입니다.")]
            public SkillSO[] skillDatas = new SkillSO[0];
            #endregion

            /// <summary>
            /// 유닛을 복제합니다.
            /// 비활성화 된 유닛으로 반환됩니다.
            /// </summary>
            /// <param name="lv">생성될 유닛의 레벨</param>
            /// <param name="spawnPos">생성될 위치</param>
            /// <param name="active">활성화 여부</param>
            /// <returns>생성된 유닛 반환</returns>
            protected Unit Instantiate(int lv, Vector3 spawnPos, bool active)
            {
                Unit unit = PrefabPool.GetObject(prefab, false).GetComponent<Unit>();

                unit.transform.position = spawnPos;

                unit.level = lv;

                unit.baseMaxHP = baseMaxHP + incHpPerLv * lv;
                unit.baseMaxMP = baseMaxMP + incMpPerLv * lv;
                unit.baseHPRegen = baseHPRegen + incHPRegenPerLv * lv;
                unit.baseMPRegen = baseMPRegen + incMPRegenPerLv * lv;
                unit.physxPower = physxPower + incPhysxPowerPerLv * lv;
                unit.magicPower = magicPower + incMagicPowerPerLv * lv;
                unit.armor = armor + incArmorPerLv * lv;
                unit.armorPnt = armorPnt + incArmorPntPerLv * lv;
                unit.atkSpd = atkSpd + incAtkSpdPerLv * lv;
                unit.mc.MoveSpeed = moveSpeed + incMoveSpeed * lv;
                unit.criticalRate = criticalRate + incCriticalRatePerLv * lv;
                unit.cdr = 1.0f - Mathf.Min(1.0f, cooldownReduction + incCdrPerLv * lv);

                unit.TemperatureVarious = tempVar;

                unit.unitInfoBar?.SetVisible(true);
                unit.gameObject.SetActive(active);

                unit.defaultAttack = defaultAttackData?.SkillData.Instantiate(unit, defaultAttackData, 0);
                unit.skills = new Skill[skillDatas.Length];
                for (int i = 0; i < skillDatas.Length; ++i)
                    unit.skills[i] = skillDatas[i]?.SkillData.Instantiate(unit, skillDatas[i], 0);

                return unit;
            }
        }
    }
}
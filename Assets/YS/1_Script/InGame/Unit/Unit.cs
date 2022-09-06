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

        #region ����
        private int                 baseMaxHP;                      // �⺻ �ִ� ü��
        private float               baseHPRegen;                    // �⺻ �ʴ� ü�� ���
        private int                 baseMaxMP;                      // �⺻ �ִ� ����
        private float               baseMPRegen;                    // �⺻ �ʴ� ���� ���
        private int                 physxPower;                     // �������ݷ�
        private int                 magicPower;                     // �ֹ���
        private float               armor;                          // ����
        private float               armorPnt;                       // ���������
        private float               atkSpd;                         // ���� �ӵ�
        private int                 criticalRate;                   // ġ��Ÿ�� (�ι� ���ط�)
        private float               cdr;                            // ��Ÿ�� ����
        private int                 tempVar;                        // �µ��������
        #endregion
        #region ���� ��ȭ��
        private int                 hpAdd               = 0;        // �߰� ü��
        private float               hpCoef              = 1.0f;     // �⺻ ü�� ���
        private float               hpRegenAdd          = 0.0f;     // �߰� �ʴ� ü�� ���
        private float               hpRegenCoef         = 1.0f;     // �ʴ� ü�� ��� ���
        private int                 mpAdd               = 0;        // �߰� ����
        private float               mpCoef              = 1.0f;     // �⺻ ���� ���
        private float               mpRegenAdd          = 0.0f;     // �߰� �ʴ� ���� ���
        private float               mpRegenCoef         = 1.0f;     // �ʴ� ü�� ���� ���
        private int                 physxPowerAdd       = 0;        // �߰� �������ݷ�
        private float               physxPowerCoef      = 1.0f;     // �������ݷ� ���
        private int                 magicPowerAdd       = 0;        // �߰� �������ݷ�
        private float               magicPowerCoef      = 1.0f;     // �������ݷ� ���
        private float               armorAdd            = 0.0f;     // �߰� ����
        private float               armorPntAdd         = 0.0f;     // �߰� �����
        private float               atkSpdCoef          = 1.0f;     // ���� �ӵ� ���
        private int                 criticalRateAdd     = 0;        // ġ��Ÿ ������
        private float               cdrAdd              = 0.0f;     // ��Ÿ�� ���ҷ�
        private float               totalTakeDmgRate    = 1.0f;     // �޴� ���ط� ������
        private float               totalDmgRate        = 1.0f;     // ���ϴ� ���ط� ������
        #endregion
        private float               curHP;
        private float               curMP;
        #region ���� �̺�Ʈ
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

        private bool isInvincible; // ��������

        // ���� �ɸ� ���� �̻�
        [ShowInInspector, DisableIf("@true"), BoxGroup("Debug", true, true, 1000)]
        protected RESTRICTION_FLAG restrictionFlag = RESTRICTION_FLAG.NONE;
        // ���� ���
        protected Unit tauntUnit;
        // ���� �������� ��ų
        [ShowInInspector, DisableIf("@true"), BoxGroup("Debug")]
        protected Skill.ActiveSkill curSkill;
        // ���� ����ް��ִ� �нú� ȿ����
        [ShowInInspector, DisableIf("@true"), BoxGroup("Debug")]
        protected Dictionary<int, PassiveSkillStateData> curPSkills = new Dictionary<int, PassiveSkillStateData>();
        // ���� ��ȣ���ִ� �����
        [ShowInInspector, DisableIf("@true"), BoxGroup("Debug")]
        protected Dictionary<int, BaseShield> curShields = new Dictionary<int, BaseShield>();

        [SerializeField, LabelText("���Ÿ� �����ΰ�?")]
        private bool isRangedUnit = false;
        [SerializeField, LabelText("�⺻���� ����ü ������"), ShowIf("isRangedUnit")]
        private Skill.BaseSkill.Projectile projectileDAPrefab;
        [SerializeField, LabelText("�⺻���� ����ü �ӵ�"), Min(0.0001f), ShowIf("@projectileDAPrefab != null")]
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
        #region ����
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
        #region ���� ��ȭ��
        public int HPAdd
        {
            get { return hpAdd; }
            protected set
            {
                // ���� �� �ִ� HP
                int hp = MaxHP;
                hpAdd = value;
                // ���� �� �ִ� HP�� ���� ��ȭ�� ���ϱ�
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
                // ���� �� �ִ� HP
                int hp = MaxHP;
                hpCoef = value;
                // ���� �� �ִ� HP�� ���� ��ȭ�� ���ϱ�
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
                // ���� �� �ִ� MP
                int mp = MaxMP;
                mpAdd = value;
                // ���� �� �ִ� MP�� ���� ��ȭ�� ���ϱ�
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
                // ���� �� �ִ� MP
                int mp = MaxMP;
                mpCoef = value;
                // ���� �� �ִ� MP�� ���� ��ȭ�� ���ϱ�
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
        [BoxGroup("����")]
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
        [BoxGroup("����")]
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

            // ����ü ���纻 ���� �� �̸� Ǯ���ϴ� ������ ���
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
            [FoldoutGroup("����")]
            [LabelText("�Ӽ�"), Tooltip("������ �Ӽ��� ���մϴ�.")]
            public UNIT_ATTRIBUTE attribute;
            [FoldoutGroup("����")]
            [LabelText("ũ��Ÿ��"), Tooltip("������ ũ��Ÿ���� ���մϴ�.")]
            public UNIT_SIZE size;

            #region ����
            [FoldoutGroup("����")]
            [LabelText("�⺻ �ִ� HP"), Tooltip("������ �⺻ �ִ� HP�� ���մϴ�.\n�⺻ �ִ� HP�� �پ��� ü�� ���� ȿ���� �⺻�� �˴ϴ�.")]
            public int baseMaxHP;
            [FoldoutGroup("����")]
            [LabelText("�⺻ �ִ� MP"), Tooltip("������ �⺻ �ִ� MP�� ���մϴ�.\n�⺻ �ִ� MP�� �پ��� ���� ���� ȿ���� �⺻�� �˴ϴ�.")]
            public int baseMaxMP;
            [FoldoutGroup("����")]
            [LabelText("�⺻ �ʴ� HP ����ӵ�"), Tooltip("������ �⺻ �ʴ� HP ����ӵ� ���մϴ�.\n�⺻ �ʴ� HP ����ӵ��� �پ��� �ʴ� HP ����ӵ��� �⺻�� �˴ϴ�.")]
            public float baseHPRegen;
            [FoldoutGroup("����")]
            [LabelText("�⺻ �ʴ� MP ����ӵ�"), Tooltip("������ �⺻ �ʴ� MP ����ӵ� ���մϴ�.\n�⺻ �ʴ� MP ����ӵ��� �پ��� �ʴ� MP ����ӵ��� �⺻�� �˴ϴ�.")]
            public float baseMPRegen;
            [FoldoutGroup("����")]
            [LabelText("���� ���ݷ�"), Tooltip("������ ���� ���ݷ��� ���մϴ�.\n���� ���ݷ��� �پ��� ���� ���ݷ� ���� ȿ���� �⺻�� �˴ϴ�.")]
            public int physxPower;
            [FoldoutGroup("����")]
            [LabelText("���� ���ݷ�"), Tooltip("������ ���� ���ݷ��� ���մϴ�.\n���� ���ݷ��� �پ��� ���� ���ݷ� ���� ȿ���� �⺻�� �˴ϴ�.")]
            public int magicPower;
            [FoldoutGroup("����")]
            [LabelText("����"), Tooltip("������ ������ ���մϴ�.\n������ �پ��� ���� ���� ȿ���� �⺻�� �˴ϴ�.")]
            public float armor;
            [FoldoutGroup("����")]
            [LabelText("�������"), Tooltip("������ ��������� ���մϴ�.\n��������� �پ��� ������� ���� ȿ���� �⺻�� �˴ϴ�.")]
            public float armorPnt;
            [FoldoutGroup("����")]
            [LabelText("���ݼӵ�"), Tooltip("������ �ʴ� ���ݼӵ��� ���մϴ�. (1.0 = 1�ʿ� �ѹ� ����, 2.0 = 1�ʿ� 2�� ����) 2\n���ݼӵ��� �پ��� ���ݼӵ� ���� ȿ���� �⺻�� �˴ϴ�.")]
            public float atkSpd;
            [FoldoutGroup("����")]
            [LabelText("�̵��ӵ�"), Tooltip("������ �̵��ӵ��� ���մϴ�.\n�̵��ӵ��� �پ��� �̵��ӵ� ���� ȿ���� �⺻�� �˴ϴ�.")]
            public float moveSpeed;
            [FoldoutGroup("����"), Min(0)]
            [LabelText("ġ��Ÿ��"), Tooltip("������ ġ��Ÿ���� ���մϴ�.\nġ��Ÿ���� �پ��� ġ��Ÿ�� ���� ȿ���� �⺻�� �˴ϴ�.")]
            public int criticalRate;
            [FoldoutGroup("����"), Range(0.0f, 1.0f)]
            [LabelText("��Ÿ�� ������"), Tooltip("������ ��Ÿ�� �������� ���մϴ�.\n���� : [0, 1] => [0% ~ 100%]\n��Ÿ�� �������� �پ��� ��Ÿ�� ������ ���� ȿ���� �⺻�� �˴ϴ�.")]
            public float cooldownReduction;
            [FoldoutGroup("����")]
            [LabelText("�µ� ��ȭ��"), Tooltip("������ �µ� ��ȭ���� ���մϴ�.\n�µ� ��ȭ���� �ش� ������ �ʵ忡 ������ �� �۷ι� �µ��� �󸶸�ŭ�� ������ �ִ����� ���� ���Դϴ�.")]
            public int tempVar;
            #endregion
            #region ������ ���� ��ȭ��
            [FoldoutGroup("����")]
            [LabelText("������ �⺻ �ִ� HP ������"), LabelWidth(200.0f)]
            public int incHpPerLv;
            [FoldoutGroup("����")]
            [LabelText("������ �⺻ �ִ� MP ������"), LabelWidth(200.0f)]
            public int incMpPerLv;
            [FoldoutGroup("����")]
            [LabelText("������ �⺻ �ʴ� HP ����ӵ�"), LabelWidth(200.0f)]
            public float incHPRegenPerLv;
            [FoldoutGroup("����")]
            [LabelText("������ �⺻ �ʴ� MP ����ӵ�"), LabelWidth(200.0f)]
            public float incMPRegenPerLv;
            [FoldoutGroup("����")]
            [LabelText("������ ���� ���ݷ� ������"), LabelWidth(200.0f)]
            public int incPhysxPowerPerLv;
            [FoldoutGroup("����")]
            [LabelText("������ ���� ���ݷ� ������"), LabelWidth(200.0f)]
            public int incMagicPowerPerLv;
            [FoldoutGroup("����")]
            [LabelText("������ ���� ������"), LabelWidth(200.0f)]
            public float incArmorPerLv;
            [FoldoutGroup("����")]
            [LabelText("������ ������� ������"), LabelWidth(200.0f)]
            public float incArmorPntPerLv;
            [FoldoutGroup("����")]
            [LabelText("������ ���ݼӵ� ������"), LabelWidth(200.0f)]
            public float incAtkSpdPerLv;
            [FoldoutGroup("����")]
            [LabelText("������ �̵��ӵ� ������"), LabelWidth(200.0f)]
            public float incMoveSpeed;
            [FoldoutGroup("����"), Min(0)]
            [LabelText("������ ġ��Ÿ�� ������"), LabelWidth(200.0f)]
            public int incCriticalRatePerLv;
            [FoldoutGroup("����")]
            [LabelText("������ ��Ÿ�� ������ ������"), LabelWidth(200.0f)]
            public float incCdrPerLv;
            #endregion

            [FoldoutGroup("������", true, 50)]
            [BoxGroup("������/��� �ִϸ��̼�", true), HideLabel, Tooltip("��� �� ����� �ִϸ��̼� ������ �����մϴ�.")]
            public AnimationTrackSet[] animSetIdle;
            [BoxGroup("������/�̵� �ִϸ��̼�", true), HideLabel, Tooltip("�̵� �� ����� �ִϸ��̼� ������ �����մϴ�.")]
            public AnimationTrackSet[] animSetMove;
            [BoxGroup("������/��� �ִϸ��̼�", true), HideLabel, Tooltip("��� �� ����� �ִϸ��̼� ������ �����մϴ�.")]
            public AnimationTrackSet[] animSetDie;

            [FoldoutGroup("����")]
            [LabelText("�⺻���� ������"), Tooltip("������ ���ݼӵ��� ����޴� ����(��ų)�Դϴ�.")]
            public SkillSO defaultAttackData;
            [FoldoutGroup("����")]
            [LabelText("��ų �����͵�"), Tooltip("������ ��Ÿ�԰������� ����޴� ��ų�Դϴ�.")]
            public SkillSO[] skillDatas = new SkillSO[0];
            #endregion

            /// <summary>
            /// ������ �����մϴ�.
            /// ��Ȱ��ȭ �� �������� ��ȯ�˴ϴ�.
            /// </summary>
            /// <param name="lv">������ ������ ����</param>
            /// <param name="spawnPos">������ ��ġ</param>
            /// <param name="active">Ȱ��ȭ ����</param>
            /// <returns>������ ���� ��ȯ</returns>
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
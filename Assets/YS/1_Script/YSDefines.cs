using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Spine.Unity;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public struct Pair<T, U>
    {
        public T first;
        public U second;

        public Pair(T first, U second)
        {
            this.first = first;
            this.second = second;
        }
    }
    public struct UnitSO
    {
        private static readonly PlayableUnitDataSO playableUnitData = ResourceManager.GetResource<PlayableUnitDataSO>("Datas/Units/PlayableUnitData");
        private static readonly TowerDataSO towerData = ResourceManager.GetResource<TowerDataSO>("Datas/Units/TowerData");
        private static readonly MonsterDataSO monsterData = ResourceManager.GetResource<MonsterDataSO>("Datas/Units/MonsterData");
        private static readonly CoreDataSO coreData = ResourceManager.GetResource<CoreDataSO>("Datas/Units/CoreData");

        public static PlayableUnitDataSO PlayableUnitData => playableUnitData;
        public static TowerDataSO TowerData => towerData;
        public static MonsterDataSO MonsterData => monsterData;
        public static CoreDataSO CoreData => coreData;
    }
    public class ResourceManager
    {
        static Dictionary<string, UnityEngine.Object> resourceMap = new Dictionary<string, UnityEngine.Object>();

        public static T GetResource<T>(string path) where T : UnityEngine.Object
        {
            if (resourceMap.ContainsKey(path))
                return (T)resourceMap[path];

            T obj = Resources.Load<T>(path);
            resourceMap.Add(path, obj);
            return obj;
        }

        public static void Remove(string path)
        {
            Resources.UnloadAsset(resourceMap[path]);
            resourceMap.Remove(path);
        }

        public static void Clear()
        {
            foreach (UnityEngine.Object o in resourceMap.Values)
                Resources.UnloadAsset(o);
            resourceMap.Clear();
        }
    }
    public enum COLLIDER_TYPE
    {
        SPHERE,
        CAPSULE,
        BOX
    }
    public enum AXIS
    {
        X_AXIS,
        Y_AXIS,
        Z_AXIS
    }
    public struct ColliderInfo
    {
        [LabelText("�浹ü Ÿ��")]
        public COLLIDER_TYPE type;
        [LabelText("�߽� ��ġ"), Tooltip("������Ʈ ���ð��������� ��ġ")]
        public Vector3 center;
        [LabelText("�ڽ� ũ��"), Tooltip("X, Y, Z������ ũ��"), ShowIf("type", COLLIDER_TYPE.BOX)]
        public Vector3 boxSize;
        [LabelText("������"), Min(0.0f), HideIf("type", COLLIDER_TYPE.BOX)]
        public float radius;
        [LabelText("����"), Tooltip("���⿡ ���� ����"), ShowIf("type", COLLIDER_TYPE.CAPSULE)]
        public float capsuleHeight;
        [LabelText("����"), Tooltip("ĸ���� ������� ����"), ShowIf("type", COLLIDER_TYPE.CAPSULE)]
        public AXIS capsuleDirection;
    }
    [System.Serializable]
    public struct EquipTowerInfo
    {
        public TOWER_INDEX ti;
        public int lv;
    }
    [System.Serializable]
    public struct EquipSkillInfo
    {
        public SkillSO sso;
        public int lv;
    }
    public enum E_CONTROL_TYPE
    {
        CLOSE_UI,
        PAUSE,
        GUIDE,
        CANCLE,
        MOVE,
        CANCLE_N_MOVE,
        DASH,
        INTERACT,
        DEFAULT_ATTACK,
        TS_1,
        TS_2,
        TS_3,
        TS_4,
        TS_5,
        TS_6,
        U_SKILL,
        USE_ITEM,
        QUICK_SHIFT,
        MAX
    }

    public enum INPUT_STATE
    {
        HOLD,
        DOWN,
        UP
    }
    #region SOULCARD_ENUM
    public enum SOULCARD_TYPE
    {
        CHARACTER_SOULCARD,
        TOWER_SOULCARD,
    }
    public enum SOULCARD_INDEX
    {
        RANGE_DA_1,
        RANGE_DA_1_HIDDEN,
        RANGE_DA_2,
        RANGE_DA_2_HIDDEN,
        RANGE_DA_3,
        RANGE_DA_3_HIDDEN,
        MELEE_DA_1,
        MELEE_DA_1_HIDDEN,
        MELEE_DA_2,
        MELEE_DA_2_HIDDEN,
        MELEE_DA_3,
        MELEE_DA_3_HIDDEN,
        ULTIMATE_SE,
        DASH_SE,
        DASH_SE_HIDDEN,
        OFFENCE_TOWER_1,
        OFFENCE_TOWER_2,
        DEFENCE_TOWER_1,
        DEFENCE_TOWER_2,
        BUFF_TOWER_1,
        BUFF_TOWER_2,
        ELIS_RAGE_OF_FROST,
        ELIS_EVER_FROST,
        ELIS_EXPLOSION_OF_FROST,
        ELIS_ICE_BULWARK,
        ELIS_BREATH_OF_FROST,
        ELIS_SUPPORT,
        ELIS_SNOWBALL,
        ELIS_BLIZZARD,
        ELIS_FLASH,
        ELIS_FLASH2,
        ELIS_FLASH3,
        MAX
    }
    public enum SOULCARD_GRADE
    {
        COMMON,
        RARE,
        HERO,
        LEGEND
    }
    #endregion
    #region UNIT_INDEX
    public enum PLAYABLE_UNIT_INDEX
    {
        ELIS = 0,
        FIRE,
        DARK,
        WATER,
        ELF,
        MAX
    }
    public enum CORE_INDEX
    {
        DEFAULT,
        MAX
    }
    public enum MONSTER_INDEX
    {
        C1_FLAME_SPIRIT,
        C1_TINY_WISP,
        C1_COWARDLY_FLAME_SPIRIT,
        C1_UNSTABLE_LAVA,
        C1_GIANT_FLAME_WARRIOR,
        C1_BLACK_FLAME_SPIRIT,
        C1_FIREBAT,
        C1_FLAME_ELEMENTALIST,
        C1_FLAME_CATAPULT,
        C1_FIRE_MASS,
        C1_QUEEN_OF_FIRE,
        MAX
    }
    public enum TOWER_INDEX
    {
        ICEWALL,
        ICICLE,
        ICE_CATAPULT,
        CROSSBOW,
        LASER,
        BLUE_FLAME,
        MAX
    }
    #endregion
    #region AI_INDEX
    public enum MONSTER_AI_INDEX
    {
        DEFAULT_MONSTER_AI,
        COWARDLY_FLAME_SPIRIT_AI,
    }
    public enum TOWER_AI_INDEX
    {
        DEFAULT_TOWER_AI,
    }
    #endregion
    #region SKILL_ENUM
    [System.Flags]
    public enum TARGET_LAYER_MASK
    {
        ALLY = 0x01,
        ENEMY = 0x02
    }
    public enum DAMAGE_TYPE
    {
        NORMAL,
        DOT,
        HEAL,
    }
    public enum SKILL_CASTING_TYPE
    {
        INSTANT,            // ��ųŰ�� ������ �ߵ�
        TICK,               // ��ųŰ�� ������������ �ߵ�
        CHARGING,           // ��ų Ű�� ���� �� ���� �ߵ�, ��ų Ű�� ������ ��ȣ�ۿ�� �ߵ�
    }
    public enum SKILL_RANGE_TYPE
    {
        CIRCLE,
        X_AXIS,
    }
    public enum SKILL_REMAINTIME_TYPE
    {
        NONE,
        RESET,
        ADD
    }
    public enum SKILL_ERROR_CODE
    {
        NO_ERR,             // ���� ����
        INVALID_SKILL,
        COOLTIME,           // ��Ÿ��
        CANT_CANCEL,        // ��ų ����� ĵ�� �Ұ�
        NOT_ENOUGH_MANA,    // ���� ����
        IN_CC,              // CC��� ���� ��� �Ұ�
        NO_TARGET,          // ����� ����
        OUT_OF_RANGE,       // ��Ÿ� ���� Ÿ��
    }
    #endregion
    #region MONSTER_ENUM
    public enum MONSTER_CATEGORY
    {
        LOW_CLASS,
        MIDDLE_CLASS,
        HIGH_CLASS,
        BOSS_CLASS
    }
    #endregion
    #region UNIT_ENUM
    [System.Flags]
    public enum RESTRICTION_FLAG
    {
        NONE = 0x00,
        MOVE = 0x01,
        ATTACK = 0x02,
        TAUNT = 0x04
    }
    public enum UNIT_TYPE
    {
        PLAYER,
        TOWER,
        MONSTER,
        CORE,
    }
    [System.Flags]
    public enum UNIT_FLAG
    {
        NONE = 0x00,
        CHARACTER = 0x01,
        TOWER = 0x02,
        MONSTER = 0x04,
        CORE = 0x08,
        ALL = CHARACTER | TOWER | MONSTER | CORE
    }

    [Serializable]
    public struct UnitChangableStatData
    {
        public UNIT_CHANGABLE_STAT stat;
        public float value;
    }
    public enum UNIT_CHANGABLE_STAT
    {
        HP,
        MP,
        PHYSICS_POWER,
        MAGIC_POWER,
        ARMOR,
        ARMOR_PENETRATION,
        ATTACK_SPEED,
        MOVE_SPEED,
        CRITICAL_RATE,
        COOLDOWN_REDUCTION
    }
    public enum UNIT_ATTRIBUTE
    {
        HUMAN,
        SPIRIT,
        MECHANIC,
        BEAST,
        DEVIL
    }
    public enum UNIT_SIZE
    {
        SMALL,
        MIDDLE,
        LARGE,
        BUILDING
    }
    #endregion
    #region TOWER_ENUM
    public enum TOWER_CATEGORY
    {
        ATTACK,
        DEFENSE,
        SUPPORT
    }
    #endregion
    [System.Flags]
    public enum LAYER_MASK
    {
        OUT_OF_FIELD    = 0x08,
        SKILL_SET       = 0x40,
        TOWER_SET       = 0x80,
        FIELD           = 0x100,
        TEAM1           = 0x200,
        TEAM2           = 0x400,
        ITEM            = 0x2000,
        TILE            = 0x4000,
        OUTLINE         = 0x10000,
    }
    public enum LAYER_NUMBER
    {
        OUT_OF_FIELD = 3,
        SKILL_SET = 6,
        TOWER_SET,
        FIELD,
        TEAM1,
        TEAM2,
        ITEM = 13,
        TILE,
        OUTLINE = 16,
        CHECK_ALLY,
        CHECK_ENEMY,
        CHECK_ALLY_ENEMY,
    }

    public delegate void Delegate_NoRetVal_NoParam();
    /// <summary>
    /// ���� ����Ǿ����� ȣ���ϵ��� ������� ��������Ʈ
    /// </summary>
    /// <param name="beforeValue">���� �� ��</param>
    /// <param name="newValue">���� �� ��</param>
    //public delegate void OnChangedValue<T>(T beforeValue, T newValue);
    public delegate void OnChangedValue();
    public delegate int TotalDamageCalcEvent(Unit victim);

    [System.Serializable]
    public struct Bezier
    {
        public Vector3[] bezierPos;

        public Vector3 GetBezierPosition(float t)
        {
            Vector3[] result = (Vector3[])bezierPos.Clone();

            for (int i = result.Length - 1; i > 0; --i)
                for (int j = 0; j < i; ++j)
                    result[j] = Vector3.Lerp(result[j], result[j + 1], t);

            return result[0];
        }

        #region Debug
        public void DrawGizmo(float t)
        {
            Vector3[] result = (Vector3[])bezierPos.Clone();

            for (int i = 0; i < result.Length - 1; ++i)
            {
                for (int j = 0; j < result.Length - 1 - i; ++j)
                {
                    Gizmos.color = Color.HSVToRGB(i * 0.3f % 1.0f, 1.0f, 1.0f);
                    Gizmos.DrawLine(result[j], result[j + 1]);
                    result[j] = Vector3.Lerp(result[j], result[j + 1], t);
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireSphere(result[j], 0.15f);
                }
            }
        }
        public void DrawMovingLineGizmo()
        {
            for (int i = 0; i < 100; ++i)
                Gizmos.DrawLine(GetBezierPosition(i / 100.0f), GetBezierPosition((i + 1) / 100.0f));
        }
        #endregion
    }

    public struct CachedWaitForSeconds
    {
        private static Dictionary<float, WaitForSeconds> cache = new Dictionary<float, WaitForSeconds>();
        public static WaitForSeconds Get(float time)
        {
            if (!cache.ContainsKey(time))
                cache.Add(time, new WaitForSeconds(time));

            return cache[time];
        }
    }

    public class AnimationPlayer
    {
        /// <summary>
        /// �ִ� Ʈ����
        /// </summary>
        private const int MaxTrack = 8;
        /// <summary>
        /// �̺�Ʈ���� �ʱ�ȭ�ϱ� ���� ���Ǵ� AnimationState
        /// </summary>
        private static readonly Spine.AnimationState animStateForReset = new Spine.AnimationState(new Spine.AnimationStateData(new Spine.SkeletonData()));
        /// <summary>
        /// �ߺ����θ� ������ ���� HashSet �ڷᱸ��.
        /// �޸� ���� ���̱� ���� ���������� �ϳ��� ���� �ٰ��� ����Ѵ�.
        /// </summary>
        private static HashSet<int> checkSet = new HashSet<int>();

        /// <summary>
        /// Ʈ���� ���׸�Ʈ���� ������ �ִ� �迭
        /// </summary>
        private AnimationTrackSet[] animSets;
        /// <summary>
        /// �� Ʈ���� ���׸�Ʈ�� ������� �ε����� ����ִ� �迭
        /// </summary>
        private int[] curSegmentIndices = new int[MaxTrack];
        /// <summary>
        /// �ִϸ��̼��� �������� ����
        /// </summary>
        private bool isComplete;

        /// <summary>
        /// �ִϸ��̼� �÷��̾ ����ϴ� �ִϸ��̼� ������Ʈ
        /// </summary>
        private readonly Spine.AnimationState animState;
        private Spine.AnimationState.TrackEntryEventDelegate EventDelegate;

        public event Delegate_NoRetVal_NoParam Complete;

        /// <summary>
        /// �ܺο��� �̺�Ʈ�� ����Ҷ� ������ �ִ� �̺�Ʈ�� ����ϱ� ���� ������Ƽ (�̱� �̺�Ʈ)
        /// </summary>
        public Spine.AnimationState.TrackEntryEventDelegate Event
        {
            set
            {
                animState.Event -= EventDelegate;
                EventDelegate = value;
                animState.Event += EventDelegate;
            }
        }
        /// <summary>
        /// ���� �ִϸ��̼� ��Ʈ�� index�� �ش��ϴ� �������� ���׸�Ʈ�� ������ �Լ�
        /// </summary>
        /// <param name="index">������ϴ� index��</param>
        /// <returns>���� �ִϸ��̼� ��Ʈ�� index�� �ش��ϴ� �������� ���׸�Ʈ</returns>
        private AnimationTrackSet.AnimationSegmentTime GetSegment(int index)
        {
            return animSets[index][curSegmentIndices[index]];
        }

        /// <summary>
        /// �ִϸ��̼� �÷��̾��� �ʱⰪ�� �������ֱ� ���� ������
        /// </summary>
        /// <param name="animState">����ϰ��� �ϴ� AnimationState, �ش� AnimationState���� �۵��Ѵ�</param>
        public AnimationPlayer(Spine.AnimationState animState)
        {
            this.animState = animState;

            animState.AssignEventSubscribersFrom(animStateForReset);
        }
        /// <summary>
        /// �ִϸ��̼� ��Ʈ���� ����ϴ� �Լ�
        /// </summary>
        /// <param name="newAnimSets">���ο� �ִϸ��̼� ��Ʈ��</param>
        /// <param name="bPlay">������ ��ģ �� ��� ����</param>
        /// <param name="timeScale">�ش� �ִϸ��̼� ��Ʈ�� ���</param>
        /// <param name="Complete">�ִϸ��̼��� �Ϸ� �� �� �̺�Ʈ �ݹ� �Լ�</param>
        /// <param name="Event">�ִϸ��̼��� �̺�Ʈ�� �ߵ� �� �� �̺�Ʈ �ݹ� �Լ�</param>
        public void SetAnimationSets(AnimationTrackSet[] newAnimSets, bool bPlay, float timeScale = 1.0f, Delegate_NoRetVal_NoParam Complete = null, Spine.AnimationState.TrackEntryEventDelegate Event = null)
        {
            // ���� �ִϸ��̼� ��Ʈ ����
            var lastAnimSets = animSets;

            // ���ϸ��̼� ��Ʈ�� ����
            animState.TimeScale = timeScale;
            animSets = newAnimSets;
            this.Complete = Complete;
            this.Event = Event;

            if (bPlay)
            {
                if (lastAnimSets != null && lastAnimSets != newAnimSets)
                {
                    // ���� �ִϸ��̼� ��Ʈ���� ������ �ʴ� ���� �ִϸ��̼� ��Ʈ�� �ִٸ� ������ �ʴ� Ʈ���� ����
                    checkSet.Clear();
                    foreach (var newAnimSet in newAnimSets)
                        checkSet.Add(newAnimSet.Track);

                    foreach (var animSet in lastAnimSets)
                        if (checkSet.Add(animSet.Track))
                            animState.SetEmptyAnimation(animSet.Track, 0.2f);
                }

                Play();
            }
            else
            {
                foreach (var animSet in animSets)
                    animState.SetEmptyAnimation(animSet.Track, 0.2f);
            }

            isComplete = false;
        }
        /// <summary>
        /// ������ �ִϸ��̼� ��Ʈ�� �ִϸ��̼��� ����մϴ�.
        /// </summary>
        public void Play()
        {
            for (int i = 0; i < animSets.Length; ++i)
            {
                curSegmentIndices[i] = 0;
                SetAnimations(i);
            }
        }
        /// <summary>
        /// �������� �ִϸ��̼��� ����ϴ�.
        /// </summary>
        public void Stop()
        {
            foreach (var animSet in animSets)
                animState.SetEmptyAnimation(animSet.Track, 0.0f);
        }
        public void ExitLoopAllTrack()
        {
            for (int i = 0; i < animSets.Length; ++i)
                ExitLoopTrack(animSets[i].Track);
        }
        public void ExitLoopTrack(int track)
        {
            int animSetIndex = GetAnimationSetIndex(track);
            var curTE = animState.GetCurrent(track);
            bool isSkip;

            if (!curTE.Loop)
                return;

            isSkip = GetSegment(animSetIndex).isSkip;

            if (++curSegmentIndices[animSetIndex] < animSets[animSetIndex].SegmentsLength)
            {
                if (isSkip)
                    SetAnimations(animSetIndex);
                else
                    AddAnimations(animSetIndex);
            }
            else
            {
                if (isSkip)
                {
                    animState.SetEmptyAnimation(track, 0.0f);
                    OnAnimationComplete(null);
                }
                else
                    curTE.Complete += (Spine.TrackEntry te) =>
                    {
                        te.Loop = false;
                        OnAnimationComplete(null);
                    };
            }
        }
        private void SetAnimations(int animSetIndex)
        {
            SetTrackEntry(animState.SetAnimation(animSets[animSetIndex].Track, animSets[animSetIndex].AnimationAsset, GetSegment(animSetIndex).isLoop), animSetIndex);

            if (GetSegment(animSetIndex).isLoop)
                return;

            ++curSegmentIndices[animSetIndex];

            AddAnimations(animSetIndex);
        }
        private void AddAnimations(int animSetIndex)
        {
            Spine.TrackEntry lastTE = null;

            while (curSegmentIndices[animSetIndex] < animSets[animSetIndex].SegmentsLength)
            {
                lastTE = SetTrackEntry(animState.AddAnimation(animSets[animSetIndex].Track, animSets[animSetIndex].AnimationAsset, GetSegment(animSetIndex).isLoop, 0.0f), animSetIndex);
                
                if (GetSegment(animSetIndex).isLoop)
                    return;

                ++curSegmentIndices[animSetIndex];
            }

            if (lastTE != null)
                lastTE.Complete += OnAnimationComplete;
            else
                animState.GetCurrent(animSets[animSetIndex].Track).Complete += OnAnimationComplete;
        }
        public void SetTimeScale(float timeScale)
        {
            animState.TimeScale = timeScale;
        }
        private Spine.TrackEntry SetTrackEntry(Spine.TrackEntry te, int animSetIndex)
        {
            te.AnimationStart = GetSegment(animSetIndex).startTime;
            te.AnimationEnd = GetSegment(animSetIndex).endTime;
            te.MixDuration = GetSegment(animSetIndex).mixTime;
            te.TimeScale = animState.TimeScale * GetSegment(animSetIndex).timeScale;

            return te;
        }
        private int GetAnimationSetIndex(int track)
        {
            for (int i = 0; i < animSets.Length; ++i)
                if (animSets[i].Track == track)
                    return i;

            throw new ArgumentOutOfRangeException("�������� �ʴ� Ʈ��");
        }
        private void OnAnimationComplete(Spine.TrackEntry te)
        {
            if (isComplete)
                return;

            Complete?.Invoke();
            isComplete = true;
        }
    }

    [Serializable]
    public struct AnimationTrackSet
    {
        #region Field
        [LabelText("Ʈ�� ��ȣ"), LabelWidth(100), Tooltip("�����ų Ʈ���� ��ȣ"), Min(0), SerializeField]
        private int track;
        [LabelText("�ִϸ��̼�"), LabelWidth(100), Tooltip("����� �ִϸ��̼�"), SerializeField]
        private AnimationReferenceAsset anim;
        [InfoBox("@\"�ִϸ��̼� ���� : \" + anim.Animation.Duration")]
        [HideIf("@anim == null"), ListDrawerSettings(CustomAddFunction = nameof(Add)), Required, SerializeField]
        [LabelText("�ִϸ��̼� ��Ʈ"), Tooltip("�ִϸ��̼��� �����ϰ� ������� �����ŵ�ϴ�.\n�ڹݵ�� �Ѱ����� �ִϸ��̼� ��Ʈ�� �����ؾ��մϴ�.")]
        private AnimationSegmentTime[] segments;
        #endregion

        #region Properties
        public int Track => track;
        public AnimationReferenceAsset AnimationAsset => anim;
        public AnimationSegmentTime this[int index] => segments[index];
        public int SegmentsLength => segments.Length;
        #endregion

        #region Methods
        private AnimationSegmentTime Add()
        {
            return new AnimationSegmentTime() { timeScale = 1.0f, startTime = 0.0f, endTime = anim.Animation.Duration, mixTime = 0.1f, isLoop = false, isSkip = false };
        }
        #endregion

        [Serializable]
        public struct AnimationSegmentTime
        {
            [LabelText("���"), LabelWidth(150.0f), Tooltip("�ִϸ��̼��� ��� �ӵ��� �����մϴ�."), Min(0.0f)]
            public float timeScale;
            [LabelText("���� �ð�"), LabelWidth(150.0f), Tooltip("�ִϸ��̼��� ���� �ð��� �����մϴ�."), Min(0.0f)]
            public float startTime;
            [LabelText("���� �ð�"), LabelWidth(150.0f), Tooltip("�ִϸ��̼��� ���� �ð��� �����մϴ�."), Min(0.0f)]
            public float endTime;
            [LabelText("�ͽ� �ð�"), LabelWidth(150.0f), Tooltip("���� �ִϸ��̼����κ��� �󸶸�ŭ�� �ð����� �ͽ������� �����մϴ�."), Min(0.0f)]
            public float mixTime;
            [LabelText("�����ΰ�?"), LabelWidth(150.0f), Tooltip("���۽ð����� ����ð������� �ִϸ��̼� ���� ���θ� �����մϴ�.")]
            public bool isLoop;
            [ShowIf("isLoop")]
            [LabelText("    ���� �ִϸ��̼� ��ŵ"), LabelWidth(150.0f), Tooltip("������ ����ǰ� ���� �ִϸ��̼��� ������� ���� �ִϸ��̼����� ��ŵ������ �����մϴ�.")]
            public bool isSkip;
        }
    }
    public struct Utility
    {
        static public StringBuilder stringBuilder = new StringBuilder();
        /**
        * <summary>
        * ������ �̵������� �����̸� unit�� ������ �ٶ󺸰�, �����̸� �������� �ٶ󺸰� �Ѵ�.
        * </summary>
        * <param name="unit">�����ϰ��� �ϴ� ����</param>
        * <param name="m">�̵� ���⿡ ���� MoveComponent</param>
        */
        static public void FlipUnitMoveDir(Unit unit)
        {
            if (!unit.mc.IsMoving)
                return;

            unit.Flip(Vector3.Dot(Vector3.right, unit.mc.Direction) < 0.0f);
        }
        /**
        * <summary>
        * ������ �ٶ󺸴� ���� �����̸� unit�� ������ �ٶ󺸰�, �����̸� �������� �ٶ󺸰� �Ѵ�.
        * </summary>
        * <param name="unit">�����ϰ��� �ϴ� ����</param>
        * <param name="lookAt">������ �ٶ󺸴� ��</param>
        */
        static public void FlipUnit(Unit unit, Vector3 lookAt)
        {
            unit.Flip(Vector3.Dot(Vector3.right, (lookAt - unit.transform.position).normalized) < 0.0f);
        }

        static public List<RaycastHit> Sweep(Vector3 origin, Vector3 destination, IHitBox[] hitBox, bool bRadialDamage, bool referDir, int layerMask)
        {
            Dictionary<int, RaycastHit> result = new Dictionary<int, RaycastHit>();
            RaycastHit[] hit;

            for (int i = 0; i < hitBox.Length; ++i)
            {
                hit = hitBox[i].Sweep(origin, referDir ? Quaternion.FromToRotation(Vector3.forward, destination - origin) : Quaternion.identity, bRadialDamage, layerMask);

                // �ߺ��� �浹ü ����
                for (int j = 0; j < hit.Length; ++j)
                {
                    int key = hit[i].transform.gameObject.GetInstanceID();

                    if (!result.ContainsKey(key))
                        result.Add(key, hit[i]);
                }
            }

            return new List<RaycastHit>(result.Values);
        }

        static public bool GetMouseRaycast(out RaycastHit hit, int layer)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hit, Camera.main.farClipPlane, layer);
        }

        static public RaycastHit[] GetMouseRaycastAll(int layer)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            return Physics.RaycastAll(ray, Camera.main.farClipPlane, layer);
        }
        static public bool GetUnitMouseRaycast(out RaycastHit hit, int layer, UNIT_FLAG targetUnitType)
        {
            return GetMouseRaycast(out hit, layer) && HasFlag((int)targetUnitType, 1 << (int)hit.transform.GetComponent<Unit>().UnitType);
        }
        static public List<RaycastHit> GetUnitMouseRaycastAll(int layer, UNIT_FLAG targetUnitType)
        {
            List<RaycastHit> hitList = new List<RaycastHit>();

            var hits = GetMouseRaycastAll(layer);

            foreach (var hit in hits)
                if (HasFlag((int)targetUnitType, 1 << (int)hit.transform.GetComponent<Unit>().UnitType))
                    hitList.Add(hit);

            return hitList;
        }

        static public bool GetMouseWorldPosition(out Vector3 pos, int layer)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool result = Physics.Raycast(ray, out hit, Camera.main.farClipPlane, layer);

            pos = hit.point;

            return result;
        }

        static public Vector3 GetPositionInRange(Vector3 origin, Vector3 dest, float range, SKILL_RANGE_TYPE skillType)
        {
            Vector3 result = Vector3.zero;

            if (range == 0.0f)
                return dest;

            switch (skillType)
            {
                case SKILL_RANGE_TYPE.CIRCLE:
                    result = dest - origin;
                    result = (result.sqrMagnitude > range * range) ? origin + result.normalized * range : dest;
                    break;
                case SKILL_RANGE_TYPE.X_AXIS:
                    float dist = dest.x - origin.x;
                    result = dest;
                    if (Mathf.Abs(dist) > range)
                        result.x = dist < 0.0f ? -range : range;
                    break;
            }

            return result;
        }
        /// <summary>
        /// flags�� checkFlag�� �����ϴ��� ����
        /// </summary>
        /// <param name="flags">Ž���� flags</param>
        /// <param name="checkFlag">ã�����ϴ� flag</param>
        static public bool HasFlag(int flags, int checkFlag)
        {
            return (flags & checkFlag) == checkFlag;
        }
        static public List<RaycastHit> SweepUnit(IHitBox[] hitboxes, Vector3 origin, Quaternion rot, bool isRadial, int layerMask, UNIT_FLAG targetUnitType)
        {
            List<RaycastHit> hitList = new List<RaycastHit>();

            foreach (var hitbox in hitboxes)
            {
                var hits = hitbox.Sweep(origin, rot, true, layerMask);

                foreach (var hit in hits)
                {
                    if (HasFlag((int)targetUnitType, 1 << (int)hit.transform.GetComponent<Unit>().UnitType))
                    {
                        hitList.Add(hit);
                        if (!isRadial)
                            return hitList;
                    }
                }
            }

            return hitList;
        }
        static public List<Unit> GetTargetUnitInUnits(List<Unit> units, UNIT_FLAG targetUnitType)
        {
            List<Unit> result = new List<Unit>();

            foreach (var unit in units)
                if (HasFlag((int)targetUnitType, 1 << (int)unit.UnitType))
                    result.Add(unit);

            return result;
        }
        static public List<RaycastHit> SweepUnit(IHitBox hitbox, Vector3 origin, Quaternion rot, bool isRadial, int layerMask, UNIT_FLAG targetUnitType)
        {
            return SweepUnit(new IHitBox[1] { hitbox }, origin, rot, isRadial, layerMask, targetUnitType);
        }
        static public Vector3 GetHitPoint(Vector3 origin, RaycastHit hit)
        {
            return hit.point == Vector3.zero ? hit.collider.ClosestPoint(origin) : hit.point;
        }

        /// <summary>
        /// type�� derived�� ��ӹ޾Ҵ��� Ȯ���ϴ� �Լ�
        /// </summary>
        /// <param name="type">Ȯ���ϰ��� �ϴ� Ÿ��</param>
        /// <param name="derived">��ӵ� Ÿ��</param>
        /// <returns></returns>
        static public bool IsDerived(Type type, Type derived)
        {
            var t = type.BaseType;
            while (t != null)
            {
                if (t == derived)
                    return true;

                t = t.BaseType;
            }

            return false;
        }

        static public string ConvertYSFormatToString(object obj, string str)
        {
            string[] elements = str.Split('{', '}');
            string result = "";
            int arrIndex = -1;

            for (int i = 0; i < elements.Length; ++i)
            {
                if (i % 2 == 0)
                    result += elements[i];
                else
                {
                    string[] subElements = elements[i].Split('.');
                    string[] findElement;

                    System.Reflection.FieldInfo fieldInfo;
                    object fieldObj = obj;
                    Type fieldType = obj.GetType();

                    for (int j = 0; j < subElements.Length; ++j)
                    {
                        findElement = subElements[j].Split('[', ']');

                        fieldInfo = fieldType.GetField(findElement[0]);
                        
                        if (findElement.Length != 1)
                        {
                            arrIndex = int.Parse(findElement[1]);
                            fieldObj = (fieldInfo.GetValue(fieldObj) as IList)[arrIndex];
                        }
                        else
                            fieldObj = fieldInfo.GetValue(fieldObj);

                        fieldType = fieldObj.GetType();
                    }

                    result += fieldObj.ToString();
                }
            }

            return result;
        }
        static public IHitBox ConvertColliderToHitbox(Collider collider)
        {
            IHitBox hitbox = null;
            if (collider is SphereCollider sphere)
                hitbox = new SphereHitcast(sphere.center, Vector3.zero, 0, sphere.radius);
            else if (collider is CapsuleCollider capsule)
            {
                Vector3 startPoint, endPoint;
                float length = Mathf.Max(0.0f, (capsule.height - (capsule.radius * 2.0f)) * 0.5f);
                startPoint = endPoint = capsule.center;
                switch (capsule.direction)
                {
                    case 0:
                        startPoint += Vector3.left * length;
                        endPoint += Vector3.right * length;
                        break;
                    case 1:
                        startPoint += Vector3.down * length;
                        endPoint += Vector3.up * length;
                        break;
                    case 2:
                        startPoint += Vector3.back * length;
                        endPoint += Vector3.forward * length;
                        break;
                }
                hitbox = new CapsuleHitcast(startPoint, endPoint, capsule.radius, Vector3.zero, 0);
            }
            else if (collider is BoxCollider box)
                hitbox = new BoxHitcast(box.center, Vector3.zero, 0, box.size / 2, Quaternion.identity);
            return hitbox;
        }
    }
}
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
        [LabelText("충돌체 타입")]
        public COLLIDER_TYPE type;
        [LabelText("중심 위치"), Tooltip("오브젝트 로컬공간에서의 위치")]
        public Vector3 center;
        [LabelText("박스 크기"), Tooltip("X, Y, Z방향의 크기"), ShowIf("type", COLLIDER_TYPE.BOX)]
        public Vector3 boxSize;
        [LabelText("반지름"), Min(0.0f), HideIf("type", COLLIDER_TYPE.BOX)]
        public float radius;
        [LabelText("높이"), Tooltip("방향에 대한 길이"), ShowIf("type", COLLIDER_TYPE.CAPSULE)]
        public float capsuleHeight;
        [LabelText("방향"), Tooltip("캡슐이 길어지는 방향"), ShowIf("type", COLLIDER_TYPE.CAPSULE)]
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
        INSTANT,            // 스킬키를 누르면 발동
        TICK,               // 스킬키를 누르고있으면 발동
        CHARGING,           // 스킬 키를 누른 후 때면 발동, 스킬 키를 누르고 상호작용시 발동
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
        NO_ERR,             // 에러 없음
        INVALID_SKILL,
        COOLTIME,           // 쿨타임
        CANT_CANCEL,        // 스킬 사용중 캔슬 불가
        NOT_ENOUGH_MANA,    // 마나 부족
        IN_CC,              // CC기로 인해 사용 불가
        NO_TARGET,          // 대상이 없음
        OUT_OF_RANGE,       // 사거리 밖의 타겟
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
    /// 값이 변경되었을때 호출하도록 만들어진 델리게이트
    /// </summary>
    /// <param name="beforeValue">변경 전 값</param>
    /// <param name="newValue">변경 후 값</param>
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
        /// 최대 트랙수
        /// </summary>
        private const int MaxTrack = 8;
        /// <summary>
        /// 이벤트들을 초기화하기 위해 사용되는 AnimationState
        /// </summary>
        private static readonly Spine.AnimationState animStateForReset = new Spine.AnimationState(new Spine.AnimationStateData(new Spine.SkeletonData()));
        /// <summary>
        /// 중복여부를 가리기 위한 HashSet 자료구조.
        /// 메모리 낭비를 줄이기 위해 전역변수로 하나를 만들어서 다같이 사용한다.
        /// </summary>
        private static HashSet<int> checkSet = new HashSet<int>();

        /// <summary>
        /// 트랙당 세그먼트들을 가지고 있는 배열
        /// </summary>
        private AnimationTrackSet[] animSets;
        /// <summary>
        /// 각 트랙별 세그먼트의 재생중인 인덱스를 담고있는 배열
        /// </summary>
        private int[] curSegmentIndices = new int[MaxTrack];
        /// <summary>
        /// 애니메이션이 끝났는지 여부
        /// </summary>
        private bool isComplete;

        /// <summary>
        /// 애니메이션 플레이어가 재생하는 애니메이션 스테이트
        /// </summary>
        private readonly Spine.AnimationState animState;
        private Spine.AnimationState.TrackEntryEventDelegate EventDelegate;

        public event Delegate_NoRetVal_NoParam Complete;

        /// <summary>
        /// 외부에서 이벤트를 등록할때 이전에 있던 이벤트를 취소하기 위한 프로퍼티 (싱글 이벤트)
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
        /// 현재 애니메이션 세트의 index에 해당하는 진행중인 세그먼트를 얻어오는 함수
        /// </summary>
        /// <param name="index">얻고자하는 index값</param>
        /// <returns>현재 애니메이션 세트의 index에 해당하는 진행중인 세그먼트</returns>
        private AnimationTrackSet.AnimationSegmentTime GetSegment(int index)
        {
            return animSets[index][curSegmentIndices[index]];
        }

        /// <summary>
        /// 애니메이션 플레이어의 초기값을 설정해주기 위한 생성자
        /// </summary>
        /// <param name="animState">재생하고자 하는 AnimationState, 해당 AnimationState에만 작동한다</param>
        public AnimationPlayer(Spine.AnimationState animState)
        {
            this.animState = animState;

            animState.AssignEventSubscribersFrom(animStateForReset);
        }
        /// <summary>
        /// 애니메이션 세트들을 등록하는 함수
        /// </summary>
        /// <param name="newAnimSets">새로운 애니메이션 세트들</param>
        /// <param name="bPlay">설정을 마친 후 재생 여부</param>
        /// <param name="timeScale">해당 애니메이션 세트의 배속</param>
        /// <param name="Complete">애니메이션이 완료 된 후 이벤트 콜백 함수</param>
        /// <param name="Event">애니메이션의 이벤트가 발동 될 때 이벤트 콜백 함수</param>
        public void SetAnimationSets(AnimationTrackSet[] newAnimSets, bool bPlay, float timeScale = 1.0f, Delegate_NoRetVal_NoParam Complete = null, Spine.AnimationState.TrackEntryEventDelegate Event = null)
        {
            // 지난 애니메이션 세트 저장
            var lastAnimSets = animSets;

            // 에니메이션 세트로 설정
            animState.TimeScale = timeScale;
            animSets = newAnimSets;
            this.Complete = Complete;
            this.Event = Event;

            if (bPlay)
            {
                if (lastAnimSets != null && lastAnimSets != newAnimSets)
                {
                    // 현재 애니메이션 세트에서 사용되지 않는 지난 애니메이션 세트가 있다면 사용되지 않는 트랙을 제거
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
        /// 설정된 애니메이션 세트로 애니메이션을 재생합니다.
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
        /// 진행중인 애니메이션을 멈춥니다.
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

            throw new ArgumentOutOfRangeException("존재하지 않는 트랙");
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
        [LabelText("트랙 번호"), LabelWidth(100), Tooltip("재생시킬 트랙의 번호"), Min(0), SerializeField]
        private int track;
        [LabelText("애니메이션"), LabelWidth(100), Tooltip("재생할 애니메이션"), SerializeField]
        private AnimationReferenceAsset anim;
        [InfoBox("@\"애니메이션 길이 : \" + anim.Animation.Duration")]
        [HideIf("@anim == null"), ListDrawerSettings(CustomAddFunction = nameof(Add)), Required, SerializeField]
        [LabelText("애니메이션 파트"), Tooltip("애니메이션을 분할하고 순서대로 재생시킵니다.\n★반드시 한가지의 애니메이션 파트가 존재해야합니다.")]
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
            [LabelText("배속"), LabelWidth(150.0f), Tooltip("애니메이션의 재생 속도를 설정합니다."), Min(0.0f)]
            public float timeScale;
            [LabelText("시작 시간"), LabelWidth(150.0f), Tooltip("애니메이션의 시작 시간을 설정합니다."), Min(0.0f)]
            public float startTime;
            [LabelText("종료 시간"), LabelWidth(150.0f), Tooltip("애니메이션의 종료 시간을 설정합니다."), Min(0.0f)]
            public float endTime;
            [LabelText("믹스 시간"), LabelWidth(150.0f), Tooltip("지난 애니메이션으로부터 얼마만큼의 시간동안 믹스할지를 설정합니다."), Min(0.0f)]
            public float mixTime;
            [LabelText("루프인가?"), LabelWidth(150.0f), Tooltip("시작시간부터 종료시간까지의 애니메이션 루프 여부를 설정합니다.")]
            public bool isLoop;
            [ShowIf("isLoop")]
            [LabelText("    남은 애니메이션 스킵"), LabelWidth(150.0f), Tooltip("루프가 종료되고 남은 애니메이션을 재생할지 다음 애니메이션으로 스킵할지를 설정합니다.")]
            public bool isSkip;
        }
    }
    public struct Utility
    {
        static public StringBuilder stringBuilder = new StringBuilder();
        /**
        * <summary>
        * 유닛의 이동방향이 좌측이면 unit을 왼쪽을 바라보게, 우측이면 오른쪽을 바라보게 한다.
        * </summary>
        * <param name="unit">변경하고자 하는 유닛</param>
        * <param name="m">이동 방향에 대한 MoveComponent</param>
        */
        static public void FlipUnitMoveDir(Unit unit)
        {
            if (!unit.mc.IsMoving)
                return;

            unit.Flip(Vector3.Dot(Vector3.right, unit.mc.Direction) < 0.0f);
        }
        /**
        * <summary>
        * 유닛이 바라보는 곳이 좌측이면 unit을 왼쪽을 바라보게, 우측이면 오른쪽을 바라보게 한다.
        * </summary>
        * <param name="unit">변경하고자 하는 유닛</param>
        * <param name="lookAt">유닛이 바라보는 곳</param>
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

                // 중복된 충돌체 제거
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
        /// flags에 checkFlag가 존재하는지 여부
        /// </summary>
        /// <param name="flags">탐색할 flags</param>
        /// <param name="checkFlag">찾고자하는 flag</param>
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
        /// type이 derived를 상속받았는지 확인하는 함수
        /// </summary>
        /// <param name="type">확인하고자 하는 타입</param>
        /// <param name="derived">상속된 타입</param>
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
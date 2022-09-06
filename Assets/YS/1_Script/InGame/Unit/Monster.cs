using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TypeReferences;

namespace YS
{
    public partial class Monster : Unit
    {
        public delegate void OnMonsterDieDelegate(Monster monster);
        #region Field
        private MONSTER_INDEX index;
        [HideInInspector]
        public bool isReality;
        private bool isEpicMonster;

        protected float zLane;
        protected int zLaneNum;
        protected bool isLeftSide;
        protected int curTempLevelApplied;

        public event OnMonsterDieDelegate OnMonsterDieEvent;
        #endregion

        #region Properties
        public MONSTER_INDEX Index => Index;
        public override UNIT_ATTRIBUTE Attribute => UnitSO.MonsterData[index].attribute;
        public override UNIT_SIZE Size => UnitSO.MonsterData[index].size;
        public override string Name => UnitSO.MonsterData[index].name;
        public override string Description => UnitSO.MonsterData[index].desc;
        public override Sprite Icon => UnitSO.MonsterData[index].icon;
        public override AnimationTrackSet[] AnimationSet_Idle => UnitSO.MonsterData[index].animSetIdle;
        public override AnimationTrackSet[] AnimationSet_Move => UnitSO.MonsterData[index].animSetMove;
        public override AnimationTrackSet[] AnimationSet_Die => UnitSO.MonsterData[index].animSetDie;
        public bool IsEpicMonster => isEpicMonster;
        public float ZLanePos => zLane;
        public int ZLaneNum => zLaneNum;
        public int EarnEXP => UnitSO.MonsterData[index].earnEXP;
        public int EarnSilver => UnitSO.MonsterData[index].earnSilver;
        public MONSTER_CATEGORY Category => UnitSO.MonsterData[index].category;
        public List<UnitChangableStatData> TemperatureFluctuateStats => UnitSO.MonsterData[index].tempFluctuateStats;
        #endregion

        #region Unity Methods
        protected override void OnEnable()
        {
            base.OnEnable();
            gm.OnChangedTemperatureLevel += OnTemperatureLevelChanged;
        }
        protected override void OnDisable()
        {
            OnMonsterDieEvent = null;

            gm.OnChangedTemperatureLevel -= OnTemperatureLevelChanged;
            base.OnDisable();
        }
        #endregion

        #region Methods
        public override void OnInstantiate()
        {
            base.OnInstantiate();

            type = UNIT_TYPE.MONSTER;
        }
        protected override void OnDie(Unit killer)
        {
            OnMonsterDieEvent?.Invoke(this);

            if (isReality)
            {
                gm.Player.GetReward(EarnEXP, EarnSilver);
                Vector3 silverPos, expPos;
                expPos = silverPos = transform.position;
                silverPos.y += 1.0f;
                gm.silverFont.Spawn(silverPos, EarnSilver);
                gm.expFont.Spawn(expPos, EarnEXP);
            }
            base.OnDie(killer);
        }
        /// <summary>
        /// 기온영향력 단계가 변경될때 호출되는 함수
        /// 변경되기 전에 호출된다.
        /// </summary>
        /// <param name="level">변경될 레벨</param>
        protected void OnTemperatureLevelChanged()
        {
            int curTempLevel = Mathf.Max(gm.CurrentTempLevel, 0);

            int deltaLevel = curTempLevel - curTempLevelApplied;
            curTempLevelApplied = curTempLevel;

            SetChangableStat(TemperatureFluctuateStats, deltaLevel);
        }
        #endregion

        [System.Serializable]
        public class MonsterData : UnitData
        {
            [HideInInspector]
            public MONSTER_INDEX index;

            [FoldoutGroup("몬스터", true, 0)]
            [Inherits(typeof(BaseMonsterAI), Grouping = Grouping.None, IncludeBaseType = true, ShowNoneElement = false, ShortName = true), LabelText("몬스터 AI"), Tooltip("몬스터의 작동방식을 정합니다.")]
            public TypeReference monsterAI = typeof(BaseMonsterAI);
            [FoldoutGroup("몬스터")]
            [LabelText("몬스터 카테고리"), Tooltip("몬스터의 카테고리를 정합니다.")]
            public MONSTER_CATEGORY category;
            [FoldoutGroup("몬스터")]
            [LabelText("획득할 수 있는 경험치"), Tooltip("몬스터가 죽을때 얼마만큼의 경험치를 플레이어에게 주는지에 대한 설정입니다.")]
            public int earnEXP;
            [FoldoutGroup("몬스터")]
            [LabelText("획득할 수 있는 실버"), Tooltip("몬스터가 죽을때 얼마만큼의 실버를 플레이어에게 주는지에 대한 설정입니다.")]
            public int earnSilver;
            [FoldoutGroup("몬스터")]
            [LabelText("온도변화에 따라 증감하는 스탯들"), Tooltip("온도의 변화에 따라 증감하는 스탯들의 목록입니다.")]
            public List<UnitChangableStatData> tempFluctuateStats = new List<UnitChangableStatData>();

            public virtual Monster Instantiate(int lv, Vector3 spawnPos, int zLaneNum, bool isEpicMonster)
            {
                Monster monster = Instantiate(lv, spawnPos, true) as Monster;
                
                monster.index = index;

                monster.fsm = ObjectPool.GetObject(monsterAI) as BaseMonsterAI;
                (monster.fsm as BaseMonsterAI).Initialize(monster);
                monster.fsm.Start();

                monster.isEpicMonster = isEpicMonster;
                monster.zLane = spawnPos.z;
                monster.zLaneNum = zLaneNum;
                monster.isLeftSide = spawnPos.x < 0.0f;
                monster.curTempLevelApplied = 0;
                monster.OnTemperatureLevelChanged();

                return monster;
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [CreateAssetMenu(fileName = "Stage", menuName = "ScriptableObjects/Stage", order = 0)]
    public class Stage : ScriptableObject
    {
        [System.Serializable]
        public struct Wave
        {
            [System.Serializable]
            public struct WaveTimeline
            {
                [System.Serializable]
                public struct SpawnMonsterInfo
                {
                    [LabelText("몬스터 종류")]
                    public MONSTER_INDEX monsterIndex;
                    [LabelText("몬스터 레벨"), Tooltip("소환될 몬스터의 레벨")]
                    public int monsterLevel;
                    [LabelText("에픽 몬스터"), Tooltip("에픽몬스터인가")]
                    public bool isEpicMonster;
                    [LabelText("랜덤 사이드 사용"), Tooltip("스폰위치의 사이드를 랜덤으로 할지")]
                    public bool isRandomSide;
                    [DisableIf("isRandomSide"), LabelText("    왼쪽 사이드 스폰"), Tooltip("true : 왼쪽 사이드 스폰, false : 오른쪽 사이드 스폰")]
                    public bool isLeftSide;
                    [LabelText("랜덤 Z라인 사용"), Tooltip("스폰위치의 Z라인을 랜덤으로 할지")]
                    public bool isRandomZLane;
                    [DisableIf("isRandomZLane"), LabelText("    Z 라인"), Tooltip("소환할 Z라인")]
                    public int zLane;
                    [LabelText("랜덤 Z오프셋 사용"), Tooltip("설정된 Z라인으로부터의 오프셋을 랜덤으로 할지")]
                    public bool isRandomZOffset;
                    [DisableIf("isRandomZOffset"), LabelText("    Z 오프셋"), Tooltip("오프셋 설정")]
                    public float zOffset;
                    [DisableIf("@(!isRandomZOffset)"), LabelText("    Z 오프셋 랜덤 범위")]
                    public float zRandomOffsetRange;
                }

                [Min(0.0f)]
                [LabelText("스폰 시간(지난 스폰으로부터)"), Tooltip("지난 스폰 시점부터 얼마만큼의 시간이 지난 후에 스폰을 시킬지")]
                public float spawnTime;
                [Min(0.0f)]
                [LabelText("중복 불가 스폰 위치 수치"), Tooltip("랜덤 위치로 몬스터가 스폰될때 몬스터들의 최소 간격 거리")]
                public float overlapDistance;
                public SpawnMonsterInfo[] spawnMonsters;

                public float OverlapDistanceSqr => overlapDistance * overlapDistance;
            }

            [LabelText("준비 시간"), Tooltip("다음 웨이브까지의 준비 시간")]
            public float readyTime;
            [SerializeField]
            private WaveTimeline[] timeline;

            public WaveTimeline[] Timeline => timeline;
        }

        [SerializeField]
        private Wave[] waves;

        public Wave[] Waves => waves;
    }
}
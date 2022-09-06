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
                    [LabelText("���� ����")]
                    public MONSTER_INDEX monsterIndex;
                    [LabelText("���� ����"), Tooltip("��ȯ�� ������ ����")]
                    public int monsterLevel;
                    [LabelText("���� ����"), Tooltip("���ȸ����ΰ�")]
                    public bool isEpicMonster;
                    [LabelText("���� ���̵� ���"), Tooltip("������ġ�� ���̵带 �������� ����")]
                    public bool isRandomSide;
                    [DisableIf("isRandomSide"), LabelText("    ���� ���̵� ����"), Tooltip("true : ���� ���̵� ����, false : ������ ���̵� ����")]
                    public bool isLeftSide;
                    [LabelText("���� Z���� ���"), Tooltip("������ġ�� Z������ �������� ����")]
                    public bool isRandomZLane;
                    [DisableIf("isRandomZLane"), LabelText("    Z ����"), Tooltip("��ȯ�� Z����")]
                    public int zLane;
                    [LabelText("���� Z������ ���"), Tooltip("������ Z�������κ����� �������� �������� ����")]
                    public bool isRandomZOffset;
                    [DisableIf("isRandomZOffset"), LabelText("    Z ������"), Tooltip("������ ����")]
                    public float zOffset;
                    [DisableIf("@(!isRandomZOffset)"), LabelText("    Z ������ ���� ����")]
                    public float zRandomOffsetRange;
                }

                [Min(0.0f)]
                [LabelText("���� �ð�(���� �������κ���)"), Tooltip("���� ���� �������� �󸶸�ŭ�� �ð��� ���� �Ŀ� ������ ��ų��")]
                public float spawnTime;
                [Min(0.0f)]
                [LabelText("�ߺ� �Ұ� ���� ��ġ ��ġ"), Tooltip("���� ��ġ�� ���Ͱ� �����ɶ� ���͵��� �ּ� ���� �Ÿ�")]
                public float overlapDistance;
                public SpawnMonsterInfo[] spawnMonsters;

                public float OverlapDistanceSqr => overlapDistance * overlapDistance;
            }

            [LabelText("�غ� �ð�"), Tooltip("���� ���̺������ �غ� �ð�")]
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
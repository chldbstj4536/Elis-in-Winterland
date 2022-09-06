using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Moru
{
    public partial class UserSaveData
    {
        [System.Serializable]
        public partial class Stage_DB
        {
            private bool isClear;
            public bool IsClear => isClear;

            private string stageName;
            public string StageName => stageName;

            private int score;
            public int Score => score;

            public const int maxStar = 3;
            private int star;
            public int Star => star;

            public YS.Stage stageInfo;

            public Stage_DB(bool _isClear, YS.Stage _stageInfo, int _score = 0)
            {
                isClear = _isClear;
                stageInfo = _stageInfo;
                stageName = _stageInfo.name;
                score = _score;
            }
        }

        /// <summary>
        /// ���������� ���� ȹ���� �� ������ �ִ� �� ������ �޾ƿɴϴ�.
        /// </summary>
        /// <param name="MaxStar"></param>
        /// <returns></returns>
        public int GetAllStageStar(out int MaxStar)
        {
            int currentStar = 0;
            MaxStar = 0;
            foreach(var stage in Stages)
            {
                currentStar += stage.Value.Star;
                MaxStar += 3;
            }
            return currentStar;
        }
        
        /// <summary>
        /// ���������� �Ű�����(Ŭ���� ����)�� �ľ��Ͽ� Ŭ������ �������� ���� ���������� �ִ���� �޾ƿɴϴ�.
        /// </summary>
        /// <param name="isClear"></param>
        /// <param name="MaxStageCount"></param>
        /// <returns></returns>
        public int GetStageInfo(bool isClear, out int MaxStageCount)
        {
            int returnvalue = 0;
            var IsClear = isClear;
            MaxStageCount = Stages.Count;
            foreach(var stage in Stages)
            {
                if(stage.Value.IsClear == IsClear)
                {
                    returnvalue++;
                }
            }
            return returnvalue;
        }
    }
}
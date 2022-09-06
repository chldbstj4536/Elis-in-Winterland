using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Moru.UI
{
    public class UI_Stage_Content : MonoBehaviour
    {
        public string StageTarget;
        public string stageTarget => StageTarget;
        private UserSaveData.Stage_DB stageDB;

        #region Varis
        public string score => stageDB != null? stageDB.Score.ToString() : "0";
        public int Star => stageDB != null ? stageDB.Star : 0;  //0~3
        public GameObject[] Stars = new GameObject[3];
        public bool isClear => stageDB != null ? stageDB.IsClear : false;
        #endregion


        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
            stageDB = UserInfo.instance.Userinfo.Stages["Stage"+stageTarget];
            StarUpdate();
        }



        private void StarUpdate()
        {
            for(int i = 0; i < Stars.Length; i++) //0~2
            {
                if(i < Star)
                {
                    Stars[i].SetActive(true);
                }
                else
                {
                    Stars[i].SetActive(false);
                }
            }
        }

        public void OnClick()
        {
            var StageInfo = FindObjectOfType<UI_Stage_Detail_Info>(true);
            StageInfo.Initialized(StageTarget);
        }
    }
}
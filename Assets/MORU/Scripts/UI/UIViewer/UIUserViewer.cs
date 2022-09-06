using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moru.UI
{
    public partial class UIViewer : MonoBehaviour
    {
        #region Property

        public float StarPercent { get; set; }
        public float StagePercent { get; set; }
        public float TowerPercent { get; set; }
        public float CharacterPercent { get; set; }

        #endregion


        public void Init_UserInfo_Bind()
        {
            StartCoroutine(User_Bind());
        }
        IEnumerator User_Bind()
        {
            yield return null;
            #region #1. 유저정보 관련

            //유저정보 관련
            m_Context["UserID"] = UserData.UserID;      //유저 아이디
            m_Context["UserNickName"] = UserData.UserNick;  //유저 닉네임
            m_Context["UserLevel"] = UserData.playerLevel;  //유저 레벨

            //유저 경험치 관련
            m_Context["UserEXP"] = UserData.EXP_Percent * 100;    //유저 레벨경험치바 백분율
            m_Context["CurrentEXP"] = UserData.Current_EXP;     //유저현재경험치 int
            m_Context["TargetEXP"] = UserData.Target_EXP;       //유저 레벨업경험치 int

            //유저 골드 관련
            m_Context["UserGold"] = UserData.Cash;              //유저의 현재보유 골드







            //스테이지의 수집량
            int MaxStage = 0;
            int currentClearStage = userData.GetStageInfo(true, out MaxStage);
            m_Context["currentClearStage"] = currentClearStage;
            m_Context["MaxStage"] = MaxStage;
            m_Context["StagePercent"] = ((float)currentClearStage/(float)MaxStage)*100;
            StagePercent = ((float)currentClearStage / (float)MaxStage);



            //스테이지의 별의 수집량
            int MaxStar = 0;
            int currentStar = UserData.GetAllStageStar(out MaxStar);
            m_Context["CurrentStar"] = currentStar;                     //현재 수집별 수
            m_Context["MaxStar"] = MaxStar;                             //최대 수집가능 별 수
            m_Context["StarPercent"] = ((float)currentStar / (float)MaxStar)*100;           //현재별/최대별%
            StarPercent = ((float)currentStar / (float)MaxStar);




            #endregion
        }
    }
}
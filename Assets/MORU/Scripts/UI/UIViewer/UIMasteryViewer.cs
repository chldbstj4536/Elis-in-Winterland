using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moru;

namespace Moru.UI
{
    public partial class UIViewer : MonoBehaviour
    {

        public void InitMastery_Bind()
        {
            StartCoroutine(Mastery_Bind());
        }

        IEnumerator Mastery_Bind()
        {
            yield return null;
            #region 마스터리 타겟레벨 바인딩
            var data = userData.GetCurrentCharacterDB.MasteryData;
            for (int i = 0; i < data.NeedPlayerLevelList.Length; i++)
            {
                m_Context[$"NeedLv{i}"] = userData.GetCurrentCharacterDB.MasteryData.NeedPlayerLevelList[i];
            }
            #endregion

            #region 선택된 마스터리 바인딩
            if (userData.GetCurrentCharacterDB.Current_Selected_Mastery != null)
            {
                m_Context["CurrentMastery.Name"] = userData.GetCurrentCharacterDB.Current_Selected_Mastery.MasteryName;     //마스터리 네임
                m_Context["CurrentMastery.CurLv"] = userData.GetCurrentCharacterDB.Current_Selected_Mastery.CurrentLevel;    //현재 레벨
                m_Context["CurrentMastery.MaxLv"] = userData.GetCurrentCharacterDB.Current_Selected_Mastery.MaxLevel;        //최대 레벨

                string type =
                    userData.GetCurrentCharacterDB.Current_Selected_Mastery.Mastery_Type == Define.MasteryType.Character ? "캐릭터" : "유틸리티";
                m_Context["CurrentMastery.Type"] = type;    //마스터리 타입

                m_Context["CurrentMastery.Icon"] = userData.GetCurrentCharacterDB.Current_Selected_Mastery.Icon;            //아이콘
                m_Context["CurrentMastery.Desc"] = userData.GetCurrentCharacterDB.Current_Selected_Mastery.Desc;            //설명
            }

            m_Context["MasteryPoint"] = userData.GetCurrentCharacterDB.Current_MasteryPoint;
            #endregion

        }
    }

}


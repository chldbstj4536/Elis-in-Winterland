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
            #region �����͸� Ÿ�ٷ��� ���ε�
            var data = userData.GetCurrentCharacterDB.MasteryData;
            for (int i = 0; i < data.NeedPlayerLevelList.Length; i++)
            {
                m_Context[$"NeedLv{i}"] = userData.GetCurrentCharacterDB.MasteryData.NeedPlayerLevelList[i];
            }
            #endregion

            #region ���õ� �����͸� ���ε�
            if (userData.GetCurrentCharacterDB.Current_Selected_Mastery != null)
            {
                m_Context["CurrentMastery.Name"] = userData.GetCurrentCharacterDB.Current_Selected_Mastery.MasteryName;     //�����͸� ����
                m_Context["CurrentMastery.CurLv"] = userData.GetCurrentCharacterDB.Current_Selected_Mastery.CurrentLevel;    //���� ����
                m_Context["CurrentMastery.MaxLv"] = userData.GetCurrentCharacterDB.Current_Selected_Mastery.MaxLevel;        //�ִ� ����

                string type =
                    userData.GetCurrentCharacterDB.Current_Selected_Mastery.Mastery_Type == Define.MasteryType.Character ? "ĳ����" : "��ƿ��Ƽ";
                m_Context["CurrentMastery.Type"] = type;    //�����͸� Ÿ��

                m_Context["CurrentMastery.Icon"] = userData.GetCurrentCharacterDB.Current_Selected_Mastery.Icon;            //������
                m_Context["CurrentMastery.Desc"] = userData.GetCurrentCharacterDB.Current_Selected_Mastery.Desc;            //����
            }

            m_Context["MasteryPoint"] = userData.GetCurrentCharacterDB.Current_MasteryPoint;
            #endregion

        }
    }

}


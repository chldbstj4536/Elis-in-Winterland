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
            #region #1. �������� ����

            //�������� ����
            m_Context["UserID"] = UserData.UserID;      //���� ���̵�
            m_Context["UserNickName"] = UserData.UserNick;  //���� �г���
            m_Context["UserLevel"] = UserData.playerLevel;  //���� ����

            //���� ����ġ ����
            m_Context["UserEXP"] = UserData.EXP_Percent * 100;    //���� ��������ġ�� �����
            m_Context["CurrentEXP"] = UserData.Current_EXP;     //�����������ġ int
            m_Context["TargetEXP"] = UserData.Target_EXP;       //���� ����������ġ int

            //���� ��� ����
            m_Context["UserGold"] = UserData.Cash;              //������ ���纸�� ���







            //���������� ������
            int MaxStage = 0;
            int currentClearStage = userData.GetStageInfo(true, out MaxStage);
            m_Context["currentClearStage"] = currentClearStage;
            m_Context["MaxStage"] = MaxStage;
            m_Context["StagePercent"] = ((float)currentClearStage/(float)MaxStage)*100;
            StagePercent = ((float)currentClearStage / (float)MaxStage);



            //���������� ���� ������
            int MaxStar = 0;
            int currentStar = UserData.GetAllStageStar(out MaxStar);
            m_Context["CurrentStar"] = currentStar;                     //���� ������ ��
            m_Context["MaxStar"] = MaxStar;                             //�ִ� �������� �� ��
            m_Context["StarPercent"] = ((float)currentStar / (float)MaxStar)*100;           //���纰/�ִ뺰%
            StarPercent = ((float)currentStar / (float)MaxStar);




            #endregion
        }
    }
}
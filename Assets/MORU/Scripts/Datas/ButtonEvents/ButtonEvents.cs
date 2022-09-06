using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Moru.UI;

namespace Moru
{
    public class ButtonEvents : MonoBehaviour
    {
        #region Preset
        /// <summary>
        /// ��ų �������� �Ű������� �����ϴ� ��ư �޼���
        /// </summary>
        /// <param name="adress"></param>
        public void SetSkillPreset(int adress)
        {
            if (adress > 3 || adress < 0) return;
            var UserInfo = FindObjectOfType<Moru.UserInfo>().Userinfo;
            UserInfo.GetCurrentCharacterDB.Current_preset_Adress = adress;
            TitleUI_Events.Del_CharacterSKill_Click?.Invoke();
        }

        /// <summary>
        /// Ÿ�� �������� �Ű������� �����ϴ� ��ư �޼���
        /// </summary>
        /// <param name="adress"></param>
        public void SetTowerPreset(int adress)
        {
            if (adress > 3 || adress < 0) return;
            var UserInfo = FindObjectOfType<Moru.UserInfo>().Userinfo;
            UserInfo.Current_Preset_Adress = adress;
            TitleUI_Events.Del_Tower_Click.Invoke();
        }
        #endregion

        #region Skill
        /// <summary>
        /// ĳ������ ��ų�� ��� 0������ ����� ��ų����Ʈ�� ȯ�޹޽��ϴ�.
        /// </summary>
        public void CurrentCharacter_Skill_Initialize()
        {
            var Character = UserInfo.instance.Userinfo.GetCurrentCharacterDB;
            foreach (var skill in Character.Character_Skills)
            {
                skill.Value.Preset_Level[Character.Current_preset_Adress] = 0;
            }
            Character.Max_Skill_Point = UserInfo.instance.Userinfo.playerLevel;
        }

        /// <summary>
        /// ���� ���õ� ��ų�� ������ �Ű�������ŭ ������ŵ�ϴ�. ��ų����Ʈ�� ���ڶ�� �̺�Ʈ�� �߻����� �ʽ��ϴ�.
        /// </summary>
        /// <param name="updateLv"></param>
        public void OnSkillLevelUpdate(int updateLv)
        {
            UserSaveData.Character_DB.SkillLevelup(updateLv);
            TitleUI_Events.Del_CharacterSKill_Click?.Invoke();
        }

        /// <summary>
        /// ���� ��ų����Ʈ���� ��� �����մϴ�.
        /// </summary>
        public void SaveSkill()
        {
            var characterDB = UserInfo.instance.Userinfo.GetCurrentCharacterDB;
            characterDB.SkillLv_Action.Clear();
            TitleUI_Events.Del_CharacterSKill_Click?.Invoke();
        }

        public void Rollback_Skill()
        {
            var characterDB = UserInfo.instance.Userinfo.GetCurrentCharacterDB;
            var arr = characterDB.SkillLv_Action.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i].Pop();
            }
            characterDB.SkillLv_Action.Clear();
            TitleUI_Events.Del_CharacterSKill_Click?.Invoke();
        }
        #endregion

        #region Mastery

        #endregion

        #region UI
        /// <summary>
        /// �Ű������� �Ѱų� ���� ��ư �޼���
        /// </summary>
        /// <param name="target"></param>
        public void Toggle_DetailInfo(GameObject target)
        {
            if (target == null) return;
            if (target.activeInHierarchy)
            { target.gameObject.SetActive(false); }
            else
            { target.gameObject.SetActive(true); }
        }

        /// <summary>
        /// ĳ����, Ÿ���� ���� �Ϸ���Ʈ Ÿ���� ��������ִ� �޼���
        /// </summary>
        public void Toggle_IllustType()
        {
            if (Define.unitViewMode == Define.UnitViewMode.SPINEMODE)
            {
                Define.unitViewMode = Define.UnitViewMode.ILLUSTMODE;
            }
            else
            {
                Define.unitViewMode = Define.UnitViewMode.SPINEMODE;
            }
        }

        /// <summary>
        /// ĳ���͸� ������ �Ű�������ŭ�� �ε��� ��, �Ǵ� �ڷ� �̵����� �������ִ� �޼���
        /// </summary>
        /// <param name="howMany"></param>
        public void Quick_SetCharacter_ArrowBtn(int howMany)
        {
            int currentIndex = (int)TitleUIManager.instance.userInfo.Userinfo.Current_PLAYABLE_UNIT_INDEX;
            int maxIndex = (int)YS.PLAYABLE_UNIT_INDEX.MAX;
            int findValue = 0;
            if (currentIndex + howMany < 0) //������� 0���� ����(�ε��� �������� ���� ��)
            {
                findValue = currentIndex + howMany + maxIndex;
            }
            else if (currentIndex + howMany >= maxIndex) //������� �ƽ����� ����
            {
                findValue = currentIndex + howMany - maxIndex;
            }
            else
            {
                findValue = currentIndex + howMany;
            }
            TitleUIManager.instance.userInfo.Userinfo.Current_PLAYABLE_UNIT_INDEX = (YS.PLAYABLE_UNIT_INDEX)findValue;
            TitleUI_Events.Del_CharacterClick?.Invoke();
        }
        #endregion

        #region Tower
        /// <summary>
        /// Ÿ���� ������ ������������ �õ��ϴ� ��ư
        /// </summary>
        public void Btn_TowerLevelUp()
        {
            var cash = UserInfo.instance.Userinfo.Cash;
            var TowerDB = UserInfo.instance.Userinfo.GetCurrentTowerDB;
            if (cash >= TowerDB.GoldCost && TowerDB.Level < UserInfo.instance.Userinfo.playerLevel)
            {
                UserInfo.instance.Userinfo.Paid(TowerDB.GoldCost);
                TowerDB.Level++;
                TitleUI_Events.Del_Tower_Click?.Invoke();
            }
            //���� ���ڶ� ��
            else if (!(cash >= TowerDB.GoldCost))
            {

            }
            //������ ���Ѽ��� �������� ��
            else if (!(TowerDB.Level < UserInfo.instance.Userinfo.playerLevel))
            {

            }
        }

        #endregion
        public void TestButton()
        {
            UserInfo.instance.Userinfo.GetCurrentCharacterDB.current_Skill_Point[0] = 50;
            TitleUI_Events.Del_BindAll?.Invoke();
        }










        /// <summary>
        /// �˾��׼ǿ� ���� �ݹ��̺�Ʈ�� �����մϴ�.
        /// </summary>
        public void Select()
        {

        }

        /// <summary>
        /// �˾��׼ǿ� ���� �̺�Ʈ�� ����մϴ�.
        /// </summary>
        public void Cancel()
        {

        }

        /// <summary>
        /// �˾�â�� ��� ��Ʈ���� �̺�Ʈ�� ����ϰ� ���ϴ�.
        /// </summary>
        /// <param name="Box"></param>
        /// <param name="callback"></param>
        public void PopUP(string Box, Action callback)
        {

        }
    }
}
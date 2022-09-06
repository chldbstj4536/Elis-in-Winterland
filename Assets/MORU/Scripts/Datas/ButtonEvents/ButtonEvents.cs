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
        /// 스킬 프리셋을 매개변수로 변경하는 버튼 메서드
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
        /// 타워 프리셋을 매개변수로 변경하는 버튼 메서드
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
        /// 캐릭터의 스킬을 모두 0레벨로 만들고 스킬포인트를 환급받습니다.
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
        /// 현재 선택된 스킬의 레벨을 매개변수만큼 증감시킵니다. 스킬포인트가 모자라면 이벤트가 발생하지 않습니다.
        /// </summary>
        /// <param name="updateLv"></param>
        public void OnSkillLevelUpdate(int updateLv)
        {
            UserSaveData.Character_DB.SkillLevelup(updateLv);
            TitleUI_Events.Del_CharacterSKill_Click?.Invoke();
        }

        /// <summary>
        /// 현재 스킬포인트들을 모두 저장합니다.
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
        /// 매개변수를 켜거나 끄는 버튼 메서드
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
        /// 캐릭터, 타워에 대한 일러스트 타입을 변경시켜주는 메서드
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
        /// 캐릭터를 빠르게 매개변수만큼의 인덱스 앞, 또는 뒤로 이동시켜 선택해주는 메서드
        /// </summary>
        /// <param name="howMany"></param>
        public void Quick_SetCharacter_ArrowBtn(int howMany)
        {
            int currentIndex = (int)TitleUIManager.instance.userInfo.Userinfo.Current_PLAYABLE_UNIT_INDEX;
            int maxIndex = (int)YS.PLAYABLE_UNIT_INDEX.MAX;
            int findValue = 0;
            if (currentIndex + howMany < 0) //결과값이 0보다 이하(인덱스 범위보다 작을 때)
            {
                findValue = currentIndex + howMany + maxIndex;
            }
            else if (currentIndex + howMany >= maxIndex) //결과값이 맥스보다 높음
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
        /// 타워의 레벨을 증가시켜줌을 시도하는 버튼
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
            //돈이 모자랄 때
            else if (!(cash >= TowerDB.GoldCost))
            {

            }
            //레벨이 상한선에 도달했을 때
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
        /// 팝업액션에 대한 콜백이벤트를 실행합니다.
        /// </summary>
        public void Select()
        {

        }

        /// <summary>
        /// 팝업액션에 대해 이벤트를 취소합니다.
        /// </summary>
        public void Cancel()
        {

        }

        /// <summary>
        /// 팝업창에 띄울 스트링과 이벤트를 등록하고 띄웁니다.
        /// </summary>
        /// <param name="Box"></param>
        /// <param name="callback"></param>
        public void PopUP(string Box, Action callback)
        {

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Moru;
using Sirenix.OdinInspector;

namespace Moru
{
    public class MasteryContent : MonoBehaviour
    {
        public UserSaveData.Character_DB.Mastery_DB Mastery = null;
        public GameObject[] Directions = new GameObject[3];

        [SerializeField] TextMeshProUGUI curLv_TextComp;
        [SerializeField] TextMeshProUGUI maxLv_TextComp;

        string curLV => Mastery != null ? Mastery.CurrentLevel.ToString() : "";
        string maxLV => Mastery != null ? Mastery.MaxLevel.ToString() : "";

        [BoxGroup("그래픽")] [SerializeField] Graphic[] graphics;

        public void Initialize(UserSaveData.Character_DB.Mastery_DB _mastery, int adress)
        {
            _mastery.Initialize();
            Mastery = _mastery;
            DrawingDirection(_mastery, adress);
            SetGraphic();

            var Button = GetComponentInChildren<Button>();
            Button.onClick.AddListener(OnClick);
        }

        public void SetView()
        {
            if (Mastery.Data == null) { transform.GetChild(0).gameObject.SetActive(false); }
            else
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }


        /// <summary>
        /// 화살표 방향을 그리는 내부메소드
        /// </summary>
        /// <param name="_mastery"></param>
        /// <param name="adress"></param>
        private void DrawingDirection(UserSaveData.Character_DB.Mastery_DB _mastery, int adress)
        {
            foreach (var dir in Directions)
            {
                dir?.SetActive(false);
            }
            if (adress % 4 == 0)
            {
                Directions[0]?.SetActive(false);
            }
            else if (adress % 4 == 3)
            {
                Directions[2]?.SetActive(false);
            }
            int colume = adress % 4;
            Debug.Log($"가로값 : {colume}");
            if (_mastery.Low_Mastery == null)
            {
                foreach (var dir in Directions)
                {
                    dir?.SetActive(false);
                }
            }
            else if (_mastery.Low_Mastery.Length == 0)
            {
                foreach (var dir in Directions)
                {
                    dir?.SetActive(false);
                }
            }
            else if (_mastery.Low_Mastery?.Length != 0)
            {
                foreach (var _lowMastery in _mastery.Low_Mastery)
                {
                    if (colume == _lowMastery.target_mastery)
                    {
                        Directions[1]?.SetActive(true);
                    }
                    else if (colume < _lowMastery.target_mastery)
                    {
                        Directions[2]?.SetActive(true);
                    }
                    else if (colume > _lowMastery.target_mastery)
                    {
                        Directions[0]?.SetActive(true);
                    }
                }
            }
        }

        /// <summary>
        /// 해당 특성의 그래픽을 업데이트합니다.
        /// </summary>
        private void SetGraphic()
        {
            curLv_TextComp.text = curLV;
            maxLv_TextComp.text = maxLV;
            var UserData = UserInfo.instance.Userinfo;
            if (UserInfo.instance.Userinfo.playerLevel >
                UserSaveData.MasterySO[UserData.Current_PLAYABLE_UNIT_INDEX].NeedPlayerLevelList[(int)Mastery.Mastery_Level])
            {
                if (Mastery.CurrentLevel == 0)
                {
                    foreach (var graphic in graphics)
                    { graphic.color = UserSaveData.BindingSO.ZeroColor; }
                }
                else
                {
                    foreach (var graphic in graphics)
                    { graphic.color = UserSaveData.BindingSO.EnableColor; }
                }
            }
            else
            {
                foreach (var graphic in graphics)
                { graphic.color = UserSaveData.BindingSO.DisableColor; }
            }
        }

        public void OnClick()
        {
            if (Mastery != null)
            { UserInfo.instance.Userinfo.GetCurrentCharacterDB.Current_Selected_Mastery = Mastery; }
            TitleUI_Events.Del_CharacterMastery_Click?.Invoke();
        }

    }
}

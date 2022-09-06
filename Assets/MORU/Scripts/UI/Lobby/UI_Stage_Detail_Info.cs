using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Moru;
using Moru.UI;

namespace Moru.UI
{
    public class UI_Stage_Detail_Info : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI StageNum;
        [SerializeField] TextMeshProUGUI ChapterNum;



        public void Initialized(string stageName)
        {
            var _stageName = stageName;
            var strings = _stageName.Split('-');
            ChapterNum.text = "Chapter. "+strings[0];
            StageNum.text = "스테이지 "+strings[1];
            TitleUIManager.instance.userInfo.Userinfo.Select_Stage("Stage"+stageName);

            FindObjectOfType<InGameLoadData>(true).Init();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace Moru.Language
{

    public class Translate_TextComponent : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        public Language current_Language { get; private set; }
        public string myLabel;
        [Space(15), Title("Translate"), ShowInInspector]
        public Dictionary<Language, string> text_value;


        private void Start()
        {
            //각텍스트의 value를 기록
            //엑셀데이터에서 자신의 라벨을 찾아 자신의 키에 대입
            //근데 이런방식은 좀 별로같다. 다른방식으로 생각해보기
            //현재는 테스트로 채우기
            if (text_value == null)
            {
                text_value = new Dictionary<Language, string>();
                for (int i = 0; i < (int)Language.None; i++)
                {
                    text_value.Add((Language)i, $"test {i}");
                }
            }
            current_Language = Language_Selecter.instance.current_Language;
            SetLanguage(current_Language);
        }

        public void SetLanguage(Language targetLanguage)
        {
            current_Language = targetLanguage;
            if (TryGetComponent<Text>(out Text component1))
            {
                if (text_value.TryGetValue(targetLanguage, out string value))
                {
                    component1.text = value;
                }
            }
            else if (TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI component2))
            {
                if (text_value.TryGetValue(targetLanguage, out string value))
                {
                    component2.text = value;
                }
            }
        }

    }
}
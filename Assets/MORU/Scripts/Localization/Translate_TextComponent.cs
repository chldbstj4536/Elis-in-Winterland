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
            //���ؽ�Ʈ�� value�� ���
            //���������Ϳ��� �ڽ��� ���� ã�� �ڽ��� Ű�� ����
            //�ٵ� �̷������ �� ���ΰ���. �ٸ�������� �����غ���
            //����� �׽�Ʈ�� ä���
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
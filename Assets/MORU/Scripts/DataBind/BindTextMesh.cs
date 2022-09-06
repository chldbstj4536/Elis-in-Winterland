using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BindTextMesh : MonoBehaviour, IBindable
{
    private string m_OriginalText;
    private TextMeshProUGUI m_Text;

    public string key
    {
        get { return null; }
    }

    /// <summary>
    /// 오리지널 텍스트(바인딩 데이터 패스)를 변경합니다. isAuto가 true일 경우 value에 자동으로 중괄호를 씌웁니다.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="isAuto"></param>
    public void SetOriginText(string value, bool isAuto = true)
    {
        if(isAuto)
        {
            m_OriginalText = @"{{" + value + @"}}";
        }
        else
        {
            m_OriginalText = value;
        }
    }

    public void Bind(DataContext context)
    {
        if (m_OriginalText == null)
        {
            m_Text = GetComponent<TextMeshProUGUI>();
            m_OriginalText = m_Text.text;
        }

        var matches = Regex.Matches(m_OriginalText, @"\{\{[^}]*}}");
        var any = false;

        for (var i = 0; i < matches.Count; i++)
        {
            var m = matches[i];
            var key = m.Value.Substring(2, m.Value.Length - 4)
                       .Split(':')[0];

            if (context.ContainsKey(key))
            {
                any = true;
                break;
            }
        }

        if (!any)
        {
            return;
        }

        m_Text.text = Regex.Replace(m_OriginalText, @"\{\{[^}]*}}", m =>
        {
            var target = m.Value.Substring(2, m.Value.Length - 4)
                          .Split(':');
            var key = target[0];
            System.Object val;
            if (context.ContainsKey(key))
            {
                val = context[key];
            }
            else
            {
                val = new object();
            }
            if (target.Length == 2 && context[key] is IFormattable)
            {
                var format = target[1];

                return ((IFormattable)val).ToString(format, CultureInfo.CurrentCulture);
            }

            return val.ToString();
        });
    }
}

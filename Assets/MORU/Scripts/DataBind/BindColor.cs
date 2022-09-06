using UnityEngine;
using UnityEngine.UI;

public class BindColor : MonoBehaviour, IBindable
{
	[SerializeField]
	private Graphic m_Graphic;
	[SerializeField]
	private string m_Key;

	public string key {
		get { return m_Key; }
	}

    /// <summary>
    /// 오리지널 바인딩 데이터 패스를 변경합니다. isAuto가 true일 경우 value에 자동으로 중괄호를 씌웁니다.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="isAuto"></param>
    public void SetOriginText(string value, bool isAuto = true)
    {
        if (isAuto)
        {
            m_Key = @"{{" + value + @"}}";
        }
        else
        {
            m_Key = value;
        }
    }

    public void Bind(DataContext context)
	{
        if (m_Graphic == null) m_Graphic = GetComponent<Graphic>();
		if (context.ContainsKey(m_Key)) {
			m_Graphic.color = (Color) context[m_Key];
		}
	}
}
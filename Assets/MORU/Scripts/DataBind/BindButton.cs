using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class BindButton : MonoBehaviour, IBindable
{
	[SerializeField]
	private Button m_Button;
	[SerializeField]
	private string m_Key;

	IEnumerator Start()
    {
		yield return null;

		m_Button = GetComponent<Button>();
		DataContext data = FindObjectOfType<DataBindContext>().m_DataContext;
		if (data.ContainsKey(m_Key))
		{
			m_Button.onClick.AddListener((UnityAction)data[m_Key]);
		}
	}

	public string key
	{
		get { return m_Key; }
	}

	public void Bind(DataContext context)
	{
		if (context.ContainsKey(m_Key))
		{
			//m_Button.color = (Color)context[m_Key];
		}
	}
}

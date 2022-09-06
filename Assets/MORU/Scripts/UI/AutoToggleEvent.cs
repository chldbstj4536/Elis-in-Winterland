using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class AutoToggleEvent : MonoBehaviour
{
    public GameObject graphic;
    public void Awake()
    {
        var toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    public virtual void OnValueChanged(bool isOn)
    {
        if(graphic != null)
        { graphic.SetActive(isOn); }
    }
}

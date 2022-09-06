using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_SceneButtonLoader : MonoBehaviour
{
    private void Awake()
    {
        var bt = GetComponent<Button>();
        bt.onClick.AddListener(OnClickEvent);
    }

    public void OnClickEvent()
    {
        Moru.SceneManager.WSceneManager.instance.OnSceneLoad_WhileLoading(2);
    }
}

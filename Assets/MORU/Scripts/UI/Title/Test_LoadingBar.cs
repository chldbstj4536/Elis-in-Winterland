using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Moru.SceneManager;

public class Test_LoadingBar : MonoBehaviour
{
    public Image fill_Bar;
    public Image progress_Bar;
    public GameObject TextBar;
    float current_waitTime = 0;
    const float WholeWaitTime = 1.5f;
    private void Update()
    {
        fill_Bar.fillAmount = WSceneManager.instance.loading_gauge;
        if (fill_Bar.fillAmount >= 1)
        {
            if (!TextBar.activeInHierarchy)
            { current_waitTime += Time.deltaTime; }
            if (current_waitTime >= WholeWaitTime)
            {
                if (!TextBar.activeInHierarchy)
                {
                    TextBar.SetActive(true);
                    fill_Bar.gameObject.SetActive(false);
                    progress_Bar.gameObject.SetActive(false);
                    current_waitTime = 0;
                }
            }
        }

        if (TextBar.activeInHierarchy)
        {
            if (Input.anyKey)
            {
                WSceneManager.instance.SceneLoad();
            }
        }
    }
}

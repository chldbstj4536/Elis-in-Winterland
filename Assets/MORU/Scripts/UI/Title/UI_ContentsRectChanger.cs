using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class UI_ContentsRectChanger : MonoBehaviour
{
    [LabelText("콘텐츠 부모 오브젝트")]
    public GameObject Contents_Parents;


    [ShowInInspector, ReadOnly] private List<GameObject> contents;

    private void Awake()
    {
        if (Contents_Parents != null)
        {
            for (int i = 0; i < Contents_Parents.transform.childCount; i++)
            {
                contents.Add(Contents_Parents.transform.GetChild(i).gameObject);
            }
        }
    }
}

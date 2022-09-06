using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Current_Info_Update : MonoBehaviour
{

    public enum ShowType { Character, Tower, Max}
    public ShowType showType;
    private void Start()
    {
        
    }

    public void OnUpdateInfo()
    {
        if(showType == ShowType.Character)
        {

        }
        else if(showType == ShowType.Tower)
        {

        }
    }
}

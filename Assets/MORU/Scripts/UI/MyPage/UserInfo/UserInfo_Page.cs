using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInfo_Page : MonoBehaviour
{
    [SerializeField] GameObject InputField;

    private void OnEnable()
    {
        InputField?.SetActive(false);
    }

    public void OnEndEdit(string value)
    {
        if(value.Length == 0)
        {
            InputField?.SetActive(false);
            return;
        }
        if (CheckWord(value))
        {
            Moru.UserInfo.instance.Userinfo.UserNick = value;
            Moru.TitleUI_Events.Del_UserInfo_Update?.Invoke();
            InputField?.SetActive(false);
            return;
        }
        else
        { 
            InputField.GetComponent<TMP_InputField>().text = "";
            InputField?.SetActive(false);
        }
    }

    private bool CheckWord(string value)
    {
        bool isRight = true;
        string strBannger = "!@#$%^&*()_+<>?\":}{][`" +
            " ㅂㅃㅈㅉㄷㄸㄱㄲㅅㅆㅁㄴㅇㄹㅎㅋㅌㅊㅍㄳㄺㅀㄵㅄ" +
            "ㅏㅑㅓㅕㅗㅛㅜㅠㅢㅐㅔㅒㅖㅚㅟㅢㅘㅝ" +
            "";
        foreach (char ch in value)
        {
            var str = ch.ToString();
            if (strBannger.Contains(str))
            {
                isRight = false;
                Debug.Log($"{str}의 형식문자는 사용할 수 없습니다.");
            }
        }
        return isRight;
    }

}

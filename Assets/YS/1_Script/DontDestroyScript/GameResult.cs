using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult : MonoBehaviour
{
    // ������ ����� ��������
    public string stage;
    // �¸� �� true, �й� �� false
    public bool result;

    /// <summary>
    /// �����ؼ� �����ֽø� �ɰŰ��ƿ�
    /// </summary>
    public StageResult _result;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
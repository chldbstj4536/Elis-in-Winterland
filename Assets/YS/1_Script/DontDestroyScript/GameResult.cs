using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult : MonoBehaviour
{
    // 게임이 진행된 스테이지
    public string stage;
    // 승리 시 true, 패배 시 false
    public bool result;

    /// <summary>
    /// 생성해서 보내주시면 될거같아용
    /// </summary>
    public StageResult _result;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnscaledTimeWapper1 : MonoBehaviour
{
    private Material mat;
    private Material mat2;
    private Shader shader;
    public float speed;

    Vector2 distortPanner { get { return new Vector2(Time.unscaledTime, Time.unscaledTime); } }

    // Start is called before the first frame update
    void Start()
    {
        //유나님의 코드를 조금 수정했어요 - 모루남김-
        //mat = GetComponent<SpriteRenderer>().material;
        mat = GetComponent<Renderer>().material;
        
        if (speed <= 0)
        { speed = 1; }

    }

    // Update is called once per frame
    void Update()
    {
        if (mat != null)
        {
            mat.SetFloat("_TimeScale", Time.unscaledTime * speed);
        }
        else
        { Debug.Log($"mat이 널 : {gameObject.name}"); }
        // 실제로 시간이 흐르는 만큼 => 대입해주세요
        // 실제로 시간이 흐르는 만큼에서 스피드만큼 좁한 후에 _timeScale 변수에 대입해주세요
    }
}

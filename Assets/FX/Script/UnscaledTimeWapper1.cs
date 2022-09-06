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
        //�������� �ڵ带 ���� �����߾�� - ��糲��-
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
        { Debug.Log($"mat�� �� : {gameObject.name}"); }
        // ������ �ð��� �帣�� ��ŭ => �������ּ���
        // ������ �ð��� �帣�� ��ŭ���� ���ǵ常ŭ ���� �Ŀ� _timeScale ������ �������ּ���
    }
}

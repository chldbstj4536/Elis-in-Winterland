using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnscaledTimeWapper : MonoBehaviour
{
    private Material mat;
    private Material mat2;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        //�������� �ڵ带 ���� �����߾�� - ��糲��-
        //mat = GetComponent<SpriteRenderer>().material;
        mat = GetComponent<Image>().material;
        if (speed <= 0)
        { speed = 1; }

    }

    // Update is called once per frame
    void Update()
    {
        if(mat != null)
        mat.SetFloat("_timeScale", Time.unscaledTime * speed);
        // ������ �ð��� �帣�� ��ŭ => �������ּ���
        // ������ �ð��� �帣�� ��ŭ���� ���ǵ常ŭ ���� �Ŀ� _timeScale ������ �������ּ���
    }
}

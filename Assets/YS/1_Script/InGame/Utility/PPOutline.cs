using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ImageEffectAllowedInSceneView] //! Scene�信�� ��ó�� ����Ʈ ����
[ExecuteInEditMode]             //! ����Ʈ��忡�� ����(���� ������ص� ����)
public class PPOutline : MonoBehaviour
{
    [SerializeField]
    private Material ppMtrl = null;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (ppMtrl == null) return;
        Graphics.Blit(src, dest, ppMtrl);    //! ���̴� ����
    }
}
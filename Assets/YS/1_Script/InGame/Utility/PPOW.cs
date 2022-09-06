using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ImageEffectAllowedInSceneView] //! Scene�信�� ��ó�� ����Ʈ ����
[ExecuteInEditMode]             //! ����Ʈ��忡�� ����(���� ������ص� ����)
public class PPOW : MonoBehaviour
{
    [SerializeField]
    private Material m_pPPMaterial = null;
    [SerializeField]
    private RenderTexture rt;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (m_pPPMaterial == null) return;
        m_pPPMaterial.SetTexture("_OWTex", rt);
        Graphics.Blit(src, dest, m_pPPMaterial);    //! ���̴� ����
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ImageEffectAllowedInSceneView] //! Scene뷰에서 후처리 이펙트 적용
[ExecuteInEditMode]             //! 에디트모드에서 실행(게임 실행안해도 적용)
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
        Graphics.Blit(src, dest, m_pPPMaterial);    //! 쉐이더 적용
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ImageEffectAllowedInSceneView] //! Scene뷰에서 후처리 이펙트 적용
[ExecuteInEditMode]             //! 에디트모드에서 실행(게임 실행안해도 적용)
public class PPOutline : MonoBehaviour
{
    [SerializeField]
    private Material ppMtrl = null;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (ppMtrl == null) return;
        Graphics.Blit(src, dest, ppMtrl);    //! 쉐이더 적용
    }
}
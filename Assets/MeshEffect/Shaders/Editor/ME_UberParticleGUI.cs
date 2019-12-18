using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Graphs;

public class ME_UberParticleGUI : ShaderGUI
{
    static float TOLERANCE = 0.001f;
    public override void OnGUI(MaterialEditor m, MaterialProperty[] properties)
    {
        var _MainTex = ShaderGUI.FindProperty("_MainTex", properties);
        var _TintColor = ShaderGUI.FindProperty("_TintColor", properties);

        var _UseSoft = ShaderGUI.FindProperty("_UseSoft", properties);
        var _InvFade = ShaderGUI.FindProperty("_InvFade", properties);
        var _SoftInverted = ShaderGUI.FindProperty("_SoftInverted", properties);

        var _UseFresnelFading = ShaderGUI.FindProperty("_UseFresnelFading", properties);
        var _FresnelFadeFactor = ShaderGUI.FindProperty("_FresnelFadeFactor", properties);

        var _UseVertexLight = ShaderGUI.FindProperty("_UseVertexLight", properties);
        //var _LightTranslucent = ShaderGUI.FindProperty("_LightTranslucent", properties);

        var _UseNoiseDistortion = ShaderGUI.FindProperty("_UseNoiseDistortion", properties);
        var _NoiseTex = ShaderGUI.FindProperty("_NoiseTex", properties);
        var _DistortionSpeedScale = ShaderGUI.FindProperty("_DistortionSpeedScale", properties);
        var _UseVertexStreamRandom = ShaderGUI.FindProperty("_UseVertexStreamRandom", properties);
        //var _UseAlphaMask = ShaderGUI.FindProperty("_UseAlphaMask", properties);

        var _UseFresnel = ShaderGUI.FindProperty("_UseFresnel", properties);
        var _FresnelColor = ShaderGUI.FindProperty("_FresnelColor", properties);
        var _FresnelPow = ShaderGUI.FindProperty("_FresnelPow", properties);
        var _FresnelR0 = ShaderGUI.FindProperty("_FresnelR0", properties);

        var _UseCutout = ShaderGUI.FindProperty("_UseCutout", properties);
        var _UseSoftCutout = ShaderGUI.FindProperty("_UseSoftCutout", properties);
        var _UseParticlesAlphaCutout = ShaderGUI.FindProperty("_UseParticlesAlphaCutout", properties);
        var _Cutout = ShaderGUI.FindProperty("_Cutout", properties);

        var _UseCutoutTex = ShaderGUI.FindProperty("_UseCutoutTex", properties);
        var _CutoutTex = ShaderGUI.FindProperty("_CutoutTex", properties);

        var _UseCutoutThreshold = ShaderGUI.FindProperty("_UseCutoutThreshold", properties);
        var _CutoutColor = ShaderGUI.FindProperty("_CutoutColor", properties);
        var _CutoutThreshold = ShaderGUI.FindProperty("_CutoutThreshold", properties);


        var _UseFrameBlending = ShaderGUI.FindProperty("_UseFrameBlending", properties);
        var _ZWriteMode = ShaderGUI.FindProperty("_ZWriteMode", properties);
        var _CullMode = ShaderGUI.FindProperty("_CullMode", properties);

        var _BlendMode = ShaderGUI.FindProperty("_BlendMode", properties);
        var _SrcMode = ShaderGUI.FindProperty("_SrcMode", properties);
        var _DstMode = ShaderGUI.FindProperty("_DstMode", properties);
        var _FogColorMultiplier = ShaderGUI.FindProperty("_FogColorMultiplier", properties);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        m.TextureProperty(_MainTex, _MainTex.displayName);
        m.ColorProperty(_TintColor, _TintColor.displayName);

        m.ShaderProperty(_UseSoft, _UseSoft.displayName);
        if (Mathf.Abs(_UseSoft.floatValue - 1) < TOLERANCE)
        {
            m.ShaderProperty(_InvFade, _InvFade.displayName);
            m.ShaderProperty(_SoftInverted, _SoftInverted.displayName);
        }

        m.ShaderProperty(_UseFresnelFading, _UseFresnelFading.displayName);
        if (Mathf.Abs(_UseFresnelFading.floatValue - 1) < TOLERANCE)
            m.ShaderProperty(_FresnelFadeFactor, _FresnelFadeFactor.displayName);

        m.ShaderProperty(_UseVertexLight, _UseVertexLight.displayName);
        //if (Mathf.Abs(_UseLighting.floatValue - 1) < TOLERANCE)
        //    m.ShaderProperty(_LightTranslucent, _LightTranslucent.displayName);


        m.ShaderProperty(_UseNoiseDistortion, _UseNoiseDistortion.displayName);
        if (Mathf.Abs(_UseNoiseDistortion.floatValue - 1) < TOLERANCE)
        {
            m.TextureProperty(_NoiseTex, _NoiseTex.displayName);
            m.ShaderProperty(_DistortionSpeedScale, _DistortionSpeedScale.displayName);
            m.ShaderProperty(_UseVertexStreamRandom, _UseVertexStreamRandom.displayName);
            //m.ShaderProperty(_UseAlphaMask, _UseAlphaMask.displayName);
        }

        m.ShaderProperty(_UseFresnel, _UseFresnel.displayName);
        if (Mathf.Abs(_UseFresnel.floatValue - 1) < TOLERANCE)
        {
            m.ColorProperty(_FresnelColor, _FresnelColor.displayName);
            m.ShaderProperty(_FresnelPow, _FresnelPow.name);
            m.ShaderProperty(_FresnelR0, _FresnelR0.displayName);
        }

        m.ShaderProperty(_UseCutout, _UseCutout.displayName);
        if (Mathf.Abs(_UseCutout.floatValue - 1) < TOLERANCE)
        {
            m.ShaderProperty(_Cutout, _Cutout.displayName);
            m.ShaderProperty(_UseSoftCutout, _UseSoftCutout.name);
            m.ShaderProperty(_UseParticlesAlphaCutout, _UseParticlesAlphaCutout.displayName);

            m.ShaderProperty(_UseCutoutTex, _UseCutoutTex.displayName);
            if (Mathf.Abs(_UseCutoutTex.floatValue - 1) < TOLERANCE)
                m.TextureProperty(_CutoutTex, _CutoutTex.displayName);

            m.ShaderProperty(_UseCutoutThreshold, _UseCutoutThreshold.displayName);
            if (Mathf.Abs(_UseCutoutThreshold.floatValue - 1) < TOLERANCE)
            {
                m.ColorProperty(_CutoutColor, _CutoutColor.displayName);
                m.ShaderProperty(_CutoutThreshold, _CutoutThreshold.displayName);
            }
        }

        m.ShaderProperty(_UseFrameBlending, _UseFrameBlending.displayName);
      
        m.ShaderProperty(_ZWriteMode, _ZWriteMode.displayName);
        m.ShaderProperty(_CullMode, _CullMode.displayName);
        m.ShaderProperty(_BlendMode, _BlendMode.displayName);

        Material material = (Material)m.target;

        if (Math.Abs(_BlendMode.floatValue) < TOLERANCE)
        {
            _SrcMode.floatValue = (int) UnityEngine.Rendering.BlendMode.SrcAlpha;
            _DstMode.floatValue = (int) UnityEngine.Rendering.BlendMode.One;
            // material.DisableKeyword("_ALPHABLEND_ON");
            _FogColorMultiplier.vectorValue = new Vector4(0, 0, 0, 0);
        }
        if (Math.Abs(_BlendMode.floatValue - 1) < TOLERANCE)
        {
            _SrcMode.floatValue = (int)UnityEngine.Rendering.BlendMode.SrcAlpha;
            _DstMode.floatValue = (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
            _FogColorMultiplier.vectorValue = new Vector4(0, 0, 0, 0);
        }
        if (Math.Abs(_BlendMode.floatValue - 2) < TOLERANCE)
        {
            _SrcMode.floatValue = (int)UnityEngine.Rendering.BlendMode.Zero;
            _DstMode.floatValue = (int)UnityEngine.Rendering.BlendMode.SrcColor;
            // material.DisableKeyword("_ALPHABLEND_ON");
            _FogColorMultiplier.vectorValue = new Vector4(1, 1, 1, 1);
        }
       
        m.RenderQueueField();

#if UNITY_5_6_OR_NEWER
        m.EnableInstancingField();
       
        material.enableInstancing = true;
#endif
    }
}

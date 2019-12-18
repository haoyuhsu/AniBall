// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "KriptoFX/ME/Particle"
{
	Properties
	{
		[Header(Main Settings)]
	[Space]
	[PerRendererData] [HDR]_TintColor("Tint Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}

	[Header(Fading)]
	[Space]
	[Toggle(_FADING_ON)] _UseSoft("Use Soft Particles", Int) = 0
		_InvFade("Soft Particles Factor", Float) = 1
		_SoftInverted("Inverted Soft Particles", Range(0, 1)) = 0
		[Toggle(USE_FRESNEL_FADING)] _UseFresnelFading("Use Fresnel Fading", Int) = 0
		_FresnelFadeFactor("Fresnel Fade Factor", Float) = 3

		[Space]
	[Header(Light)]
	[Toggle(USE_VERTEX_LIGHT)] _UseVertexLight("Use Vertex Lighting", Int) = 0

	[Space]
	[Header(Noise Distortion)]
	[Toggle(USE_NOISE_DISTORTION)] _UseNoiseDistortion("Use Noise Distortion", Int) = 0
		_NoiseTex("Noise Texture (RG)", 2D) = "black" {}
	_DistortionSpeedScale("Speed (XY) Scale(XY)", Vector) = (1, -1, .15, .15)
		_UseVertexStreamRandom("Use Vertex Stream Random", Int) = 0

		[Space]
	[Header(Fresnel)]
	[Toggle(USE_FRESNEL)] _UseFresnel("Use Fresnel", Int) = 0
		[HDR]_FresnelColor("Fresnel Color", Color) = (1,1,1,1)

		_FresnelPow("Fresnel Pow", Float) = 5
		_FresnelR0("Fresnel R0", Float) = 0.04

		[Space]
	[Header(Cutout)]
	[Toggle(USE_CUTOUT)] _UseCutout("Use Cutout", Int) = 0
		[PerRendererData]	_Cutout("Cutout", Range(0, 1)) = 1
		_UseSoftCutout("Use Soft Cutout", Int) = 0
		_UseParticlesAlphaCutout("Use Particles Alpha", Int) = 0

		[Toggle(USE_CUTOUT_TEX)] _UseCutoutTex("Use Cutout Texture", Int) = 0
		_CutoutTex("Cutout Tex", 2D) = "white" {}

	[Toggle(USE_CUTOUT_THRESHOLD)] _UseCutoutThreshold("Use Cutout Threshold", Int) = 0
		[HDR]_CutoutColor("Cutout Color", Color) = (1,1,1,1)
		_CutoutThreshold("Cutout Threshold", Range(0, 1)) = 0.015

		[Space]
	[Header(Rendering)]
	[Toggle(_FLIPBOOK_BLENDING)] _UseFrameBlending("Use Frame Blending", Int) = 0

		[Toggle] _ZWriteMode("ZWrite On?", Int) = 0
		[Enum(Cull Off,0, Cull Front,1, Cull Back,2)] _CullMode("Culling", Float) = 0 //0 = off, 2=back
		[KeywordEnum(Add, Blend, Mul)] _BlendMode("Blend Mode", Float) = 1
		_SrcMode("SrcMode", int) = 5
		_DstMode("DstMode", int) = 10
		_FogColorMultiplier("Fog Color Multiplier", Vector) = (0,0,0,0)
	}

		Category{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend[_SrcMode][_DstMode]
		Cull[_CullMode]
		ZWrite[_ZWriteMode]
		Offset -1, -1

		SubShader{

		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		//#pragma target 3.0

#pragma multi_compile_particles
#pragma multi_compile_fog
#pragma multi_compile_instancing
#pragma shader_feature USE_NOISE_DISTORTION
#pragma shader_feature USE_FRESNEL

#pragma shader_feature USE_VERTEX_LIGHT
//#pragma shader_feature USE_CUTOUT
//#pragma shader_feature USE_CUTOUT_TEX
//#pragma shader_feature USE_CUTOUT_THRESHOLD

#pragma shader_feature USE_FRESNEL_FADING
#pragma shader_feature _FLIPBOOK_BLENDING
#pragma shader_feature USE_SCRIPT_FRAMEBLENDING
#pragma shader_feature _FADING_ON
#pragma shader_feature _BLENDMODE_BLEND


#include "UnityCG.cginc"

		sampler2D _MainTex;
	sampler2D _NoiseTex;
	sampler2D _CutoutTex;
	sampler2D _CameraDepthTexture;

	float4 _MainTex_ST;
	float4 _NoiseTex_ST;
	float4 _CutoutTex_ST;
	float4 _MainTex_NextFrame;
	float4x4 unity_Projector;
	float4x4 unity_ProjectorClip;

	half4 _CutoutColor;
	half4 _FresnelColor;
	half4 _DistortionSpeedScale;
	fixed4 _FogColorMultiplier;

	half _InvFade;
	half _SoftInverted;
	//half _LightTranslucent;
	half _FresnelFadeFactor;
	half _FresnelPow;
	half _FresnelR0;
	half _CutoutThreshold;
	half InterpolationValue;
	half _UseSoftCutout;
	half _UseParticlesAlphaCutout;
	fixed _UseVertexStreamRandom;


	UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_DEFINE_INSTANCED_PROP(half4, _TintColor)
#define _TintColor_arr Props
		UNITY_DEFINE_INSTANCED_PROP(half, _Cutout)
#define _Cutout_arr Props
	UNITY_INSTANCING_BUFFER_END(Props)


	struct appdata_t 
	{
		float4 vertex : POSITION;
		float4 normal : NORMAL;
		half4 color : COLOR0;
#ifdef _FLIPBOOK_BLENDING
		float2 texcoord : TEXCOORD0;
		float4 texcoordBlendFrame : TEXCOORD1;
#ifdef USE_NOISE_DISTORTION
		float4 randomID : TEXCOORD2;
#endif
#else
		float2 texcoord : TEXCOORD0;
#ifdef USE_NOISE_DISTORTION
		float4 randomID : TEXCOORD1;
#endif
#endif

		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f 
	{
		float4 vertex : SV_POSITION;
		half4 color : COLOR0;

#ifdef _FLIPBOOK_BLENDING
		float4 texcoord : TEXCOORD0;
		fixed blend : TEXCOORD1;
#else
		float2 texcoord : TEXCOORD0;
#endif

#if defined (USE_NOISE_DISTORTION) || defined (USE_CUTOUT_TEX)
		float4 noiseCutoutTexcoord : TEXCOORD2;
#endif
#ifdef USE_NOISE_DISTORTION
		float randomID : TEXCOORD3;
#endif
		UNITY_FOG_COORDS(4)

#ifdef _FADING_ON
	#ifdef SOFTPARTICLES_ON
			float4 projPos : TEXCOORD5;
	#endif
#endif

#if defined (USE_FRESNEL_FADING) || defined (USE_FRESNEL)
		float fresnel : TEXCOORD6;
#endif

#ifdef USE_SCRIPT_FRAMEBLENDING
		float2 scriptTexcoord : TEXCOORD7;
#endif

		UNITY_VERTEX_INPUT_INSTANCE_ID
		UNITY_VERTEX_OUTPUT_STEREO
	};

	sampler2D ME_PointLightAttenuation;
	half4 ME_AmbientColor;
	float4 ME_LightPositions[40];
	float4 ME_LightColors[40];
	int ME_LightCount;

	half3 ShadeCustomLights(float4 vertex, half3 normal, int lightCount)
	{
		float3 worldPos = mul(unity_ObjectToWorld, vertex);
		float3 worldNormal = UnityObjectToWorldNormal(normal);

		float3 lightColor = 0;
		for (int i = 0; i < lightCount; i++) {
			float3 lightDir = ME_LightPositions[i].xyz - worldPos.xyz * ME_LightColors[i].w;
			half normalizedDist = length(lightDir) / ME_LightPositions[i].w;
			fixed attenuation = tex2Dlod(ME_PointLightAttenuation, half4(normalizedDist.xx, 0, 0));
			attenuation = lerp(1, attenuation, ME_LightColors[i].w);
			float diff = max(0, dot(normalize(worldNormal), normalize(lightDir)));
			lightColor += ME_LightColors[i].rgb * (diff * attenuation);
		}
		return (lightColor);
	}

	v2f vert(appdata_t v)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_TRANSFER_INSTANCE_ID(v, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		o.vertex = UnityObjectToClipPos(v.vertex);
#ifdef _FADING_ON
	#ifdef SOFTPARTICLES_ON
		o.projPos = ComputeScreenPos(o.vertex);
		COMPUTE_EYEDEPTH(o.projPos.z);
	#endif
#endif
		o.color = v.color;
#if USE_VERTEX_LIGHT
		o.color.rgb *= ShadeCustomLights(v.vertex, 1, ME_LightCount) * 3;
#endif

		o.texcoord.xy = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
#ifdef _FLIPBOOK_BLENDING
		o.texcoord.zw = TRANSFORM_TEX(v.texcoordBlendFrame.xy, _MainTex);
		o.blend = v.texcoordBlendFrame.z;
#endif

#ifdef USE_SCRIPT_FRAMEBLENDING
		o.scriptTexcoord = v.texcoord.xy * _MainTex_NextFrame.xy + _MainTex_NextFrame.zw;
#endif

#if defined (USE_NOISE_DISTORTION) || defined (USE_CUTOUT_TEX)
		o.noiseCutoutTexcoord = 0;
#endif

#ifdef USE_NOISE_DISTORTION
		o.noiseCutoutTexcoord.xy = TRANSFORM_TEX(v.texcoord, _NoiseTex);
#endif

#ifdef USE_CUTOUT_TEX
		o.noiseCutoutTexcoord.zw = TRANSFORM_TEX(v.texcoord, _CutoutTex);
#endif

#ifdef USE_NOISE_DISTORTION
		o.randomID = dot(v.randomID, 255) * _UseVertexStreamRandom;
#endif

#if defined (USE_FRESNEL_FADING) || defined (USE_FRESNEL)
		half fresnel = abs(dot(normalize(v.normal), normalize(ObjSpaceViewDir(v.vertex))));
#ifdef USE_FRESNEL_FADING
		o.fresnel = saturate(pow(fresnel, _FresnelFadeFactor) * _FresnelFadeFactor);
#endif
#ifdef USE_FRESNEL
		o.fresnel = 1 - fresnel;
		o.fresnel = pow(o.fresnel, _FresnelPow);
		o.fresnel = saturate(_FresnelR0 + (1.0 - _FresnelR0) * o.fresnel);
#endif
#endif

		UNITY_TRANSFER_FOG(o,o.vertex);
			return o;
	}


	half4 frag(v2f i) : SV_Target
	{

		UNITY_SETUP_INSTANCE_ID(i);

#ifdef _FADING_ON
	#ifdef SOFTPARTICLES_ON
		float z = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)).r;
		float sceneZ = LinearEyeDepth(UNITY_SAMPLE_DEPTH(z));
		float partZ = i.projPos.z;
		float fade = saturate(_InvFade * (sceneZ - partZ));
		fade = lerp(fade, 1 - fade, _SoftInverted);
		i.color.a *= fade;
	#endif
#endif

#ifdef USE_NOISE_DISTORTION
	half2 noiseMask;
	float time = _Time.x;

	time += i.randomID;

	half2 mask1 = tex2D(_NoiseTex, i.noiseCutoutTexcoord.xy + _DistortionSpeedScale.x * time) * 2 - 1;
	half2 mask2 = tex2D(_NoiseTex, i.noiseCutoutTexcoord.xy - _DistortionSpeedScale.x * time * 1.4 + float2(0.4, 0.6)) * 2 - 1;
	half2 mask3 = tex2D(_NoiseTex, i.noiseCutoutTexcoord.xy * 3 + _DistortionSpeedScale.y * time) * 2 - 1;
	half2 mask4 = tex2D(_NoiseTex, i.noiseCutoutTexcoord.xy * 3 - _DistortionSpeedScale.y * time * 1.25 + float2(0.3, 0.7)) * 2 - 1;
	noiseMask = (mask1 + mask2) * _DistortionSpeedScale.z + (mask3 + mask4) * _DistortionSpeedScale.w;

	i.texcoord.xy += noiseMask;
#ifdef _FLIPBOOK_BLENDING
	i.texcoord.zw += noiseMask;
#endif
#ifdef USE_CUTOUT_TEX
	i.noiseCutoutTexcoord.zw += noiseMask;
#endif

#endif

	half4 tex = tex2D(_MainTex, i.texcoord);

#ifdef _FLIPBOOK_BLENDING
	half4 tex2 = tex2D(_MainTex, i.texcoord.zw);
	tex = lerp(tex, tex2, i.blend);
#endif

#ifdef USE_SCRIPT_FRAMEBLENDING
	half4 tex3 = tex2D(_MainTex, i.scriptTexcoord);
	tex = lerp(tex, tex3, InterpolationValue);
#endif

	half4 res = 2 * tex *  UNITY_ACCESS_INSTANCED_PROP(_TintColor_arr, _TintColor);

#ifdef USE_CUTOUT
	fixed cutout = UNITY_ACCESS_INSTANCED_PROP(_Cutout_arr, _Cutout);
	cutout = lerp(cutout, (1.001 - i.color.a + cutout), _UseParticlesAlphaCutout);

#ifdef USE_CUTOUT_TEX
	fixed mask = tex2D(_CutoutTex, i.noiseCutoutTexcoord.zw);
#else
	fixed mask = tex.a;
#endif
	
	fixed diffMask = mask - cutout;
	fixed alphaMask = lerp(saturate(diffMask * 10000) * res.a, saturate(diffMask * 2) * res.a, _UseSoftCutout);

#ifdef USE_CUTOUT_THRESHOLD
	fixed alphaMaskThreshold = saturate((diffMask - _CutoutThreshold) * 10000) * res.a;
	res.rgb = lerp(res.rgb, _CutoutColor, saturate((1 - alphaMaskThreshold) * alphaMask));
	res.a = alphaMask;
#else
	res.a = alphaMask;
#endif

#endif	

	res *= i.color;


#ifdef USE_FRESNEL_FADING
	res.a *= i.fresnel;
#endif

#ifdef USE_FRESNEL
	res.rgb += i.fresnel * _FresnelColor;
#endif

	res.a = saturate(res.a);

#ifdef _BLENDMODE_BLEND
	UNITY_APPLY_FOG(i.fogCoord, res);
#else
	res.rgb = lerp(res.rgb, lerp(1, res.rgb, res.a), _FogColorMultiplier.r);
	UNITY_APPLY_FOG_COLOR(i.fogCoord, res, _FogColorMultiplier);
#endif
	
		return res;
	}
		ENDCG
	}
	}
	}


		CustomEditor "ME_UberParticleGUI"
}

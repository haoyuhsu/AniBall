Shader "KriptoFX/ME/GlowCutoutGradient" {
	Properties{
		[HDR]_TintColor("Tint Color", Color) = (0.5,0.5,0.5,1)
		_GradientStrength("Gradient Strength", Float) = 0.5
		_TimeScale("Time Scale", Vector) = (1,1,1,1)
		_MainTex("Noise Texture", 2D) = "white" {}
	_BorderScale("Border Scale (XY) Offset (Z)", Vector) = (0.5,0.05,1,1)
	}
		Category{

		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

		SubShader{

		Pass{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend One OneMinusSrcAlpha
		Cull Off
		Offset -1, -1
		ZWrite Off


		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ KRIPTO_FX_LWRP_RENDERING KRIPTO_FX_HDRP_RENDERING

#include "UnityCG.cginc"

		sampler2D _MainTex;
	float4 _TintColor;
	float4 _TimeScale;
	float4 _BorderScale;
	half _GradientStrength;

	struct appdata_t {
		float4 vertex : POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;
		float3 normal : NORMAL;
	};

	struct v2f {
		float4 vertex : POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;
		float4 worldPosScaled : TEXCOORD1;
		float3 normal : NORMAL;

	};

	float4 _MainTex_ST;

	v2f vert(appdata_t v)
	{
		v2f o;
		//v.vertex.xyz += v.normal / 100 * _BorderScale.z;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.color = v.color;
		o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
		float3 worldPos = v.vertex * float3(length(unity_ObjectToWorld[0].xyz), length(unity_ObjectToWorld[1].xyz), length(unity_ObjectToWorld[2].xyz));
		o.worldPosScaled.x = worldPos.x *  _MainTex_ST.x;
		o.worldPosScaled.y = worldPos.y *  _MainTex_ST.y;
		o.worldPosScaled.z = worldPos.z *  _MainTex_ST.x;
		o.worldPosScaled.w = worldPos.z *  _MainTex_ST.y;
		o.normal = abs(v.normal);
		return o;
	}

	sampler2D _CameraDepthTexture;

	half tex2DTriplanar(sampler2D tex, float2 offset, float4 worldPos, float3 normal)
	{
		half3 texColor;
		texColor.x = tex2D(tex, worldPos.zy + offset);
		texColor.y = tex2D(tex, worldPos.xw + offset);
		texColor.z = tex2D(tex, worldPos.xy + offset);
		normal = normal / (normal.x + normal.y + normal.z);
		return dot(texColor, normal);
	}

	half4 frag(v2f i) : COLOR
	{
		//_Time.x = 0;
		half mask = tex2DTriplanar(_MainTex, _Time.x * _TimeScale.xy, i.worldPosScaled, i.normal);

	half tex = tex2DTriplanar(_MainTex, _Time.x * _TimeScale.zw + mask * _BorderScale.x, i.worldPosScaled, i.normal);
	half alphaMask = tex2DTriplanar(_MainTex, 0.3 + mask * _BorderScale.y, i.worldPosScaled, i.normal);

	float4 res;
#if (KRIPTO_FX_HDRP_RENDERING) || ((KRIPTO_FX_LWRP_RENDERING) && (!UNITY_COLORSPACE_GAMMA))
	res = i.color * _TintColor;
	res = pow(i.color * _TintColor, 2.2);
#else
	res = i.color * _TintColor;
#endif
	res *= tex * mask;

	res = lerp(float4(0, 0, 0, 0), res, alphaMask.xxxx);
	res.rgb = pow(res.rgb, _BorderScale.w);
	//#ifndef UNITY_COLORSPACE_GAMMA
	//		res.rgb = pow(res.rgb, 0.75);
	//#endif
	half gray = dot(saturate(res.rgb + _GradientStrength), 0.33);
	return float4(res.rgb, gray )* _TintColor.a;


	return  res;
	}
		ENDCG
	}
	}

	}
}
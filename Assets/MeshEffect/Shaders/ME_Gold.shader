Shader "KriptoFX/ME/Gold" {
	Properties{
		_MainColor("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_Cutoff("Cutoff", Range(0,.9)) = .5
		_SpeedDistort("Speed(XY) Distort(ZW)", Vector) = (1,0,0,0)
	}




		SubShader{

		Cull Back
		ZWrite On
		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#pragma multi_compile_fog

#include "UnityCG.cginc"


		sampler2D ME_PointLightAttenuation;
	half4 ME_AmbientColor;
	float4 ME_LightPositions[40];
	float4 ME_LightColors[40];
	int ME_LightCount;


	sampler2D _MainTex;
	float4 _MainTex_ST;
	half _Cutoff;
	fixed4 _MainColor;
	half4 _SpeedDistort;
	samplerCUBE  ME_Reflection;

	struct appdata_t {
		float4 vertex : POSITION;
		fixed4 color : COLOR0;
		half3 normal : NORMAL;
		float2 texcoord : TEXCOORD0;

		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		fixed3 lightColor : COLOR;
		float2 texcoord : TEXCOORD0;
		half3 normal : NORMAL;
		float3 viewDir : TEXCOORD1;
		UNITY_FOG_COORDS(2)

			UNITY_VERTEX_OUTPUT_STEREO
	};




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
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		o.vertex = UnityObjectToClipPos(v.vertex);

		o.lightColor = max(ShadeCustomLights(v.vertex, v.normal, ME_LightCount), ShadeCustomLights(v.vertex, 1, ME_LightCount) * 3);
		o.lightColor = lerp(dot(o.lightColor, 0.33), o.lightColor, 0.25);

		o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
		o.normal = normalize(UnityObjectToWorldNormal(v.normal));
		o.viewDir = normalize(WorldSpaceViewDir(v.vertex));

		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}


	fixed4 frag(v2f i) : SV_Target
	{
		float3 reflectionDir = reflect(-(i.viewDir), i.normal);
		float4 envSample = texCUBE(ME_Reflection, reflectionDir);
		half fresnel = 1 - saturate(dot(i.viewDir, i.normal));
		fresnel *= fresnel;
		fixed4 finalCol = 1;
		finalCol.rgb = saturate(envSample.rgb * _MainColor.rgb * 1.5 + fresnel * i.lightColor *  _MainColor.rgb * 0.25);

		fixed4 mask = tex2D(_MainTex, i.texcoord + _Time.x * _SpeedDistort.xy);
		mask = tex2D(_MainTex, i.texcoord + mask * _SpeedDistort.zw - _Time.x * _SpeedDistort.xy  * 1.4);
#ifndef UNITY_COLORSPACE_GAMMA
		mask.rgb = pow(mask.rgb, 0.45);

#endif
		if (mask.r > _MainColor.a - (1 - _Cutoff)) discard;

		UNITY_APPLY_FOG(i.fogCoord, finalCol);

		return finalCol;
	}
		ENDCG
	}

		Pass
	{
		Tags{ "Queue" = "Transparent" "LightMode" = "ShadowCaster" }

		CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_shadowcaster
#pragma fragmentoption ARB_precision_hint_fastest

#include "UnityCG.cginc"

		struct appdata
	{
		float2 texcoord : TEXCOORD0;
		float4 vertex : POSITION;
		half3 normal : NORMAL;
	};


	sampler2D _MainTex;
	float4 _MainTex_ST;
	half _Cutoff;
	fixed4 _MainColor;
	half4 _SpeedDistort;

	struct v2f
	{
		float2 texcoord : TEXCOORD3;
		V2F_SHADOW_CASTER;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
			return o;
	}

	float4 frag(v2f i) : COLOR
	{
		fixed4 mask = tex2D(_MainTex, i.texcoord + _Time.x * _SpeedDistort.xy);
	mask = tex2D(_MainTex, i.texcoord + mask * _SpeedDistort.zw - _Time.x * _SpeedDistort.xy  * 1.4);
#ifndef UNITY_COLORSPACE_GAMMA
	mask.rgb = pow(mask.rgb, 0.45);

#endif
	if (mask.r > _MainColor.a - (1 - _Cutoff)) discard;
	SHADOW_CASTER_FRAGMENT(i)
	}

		ENDCG
	}


	}

}

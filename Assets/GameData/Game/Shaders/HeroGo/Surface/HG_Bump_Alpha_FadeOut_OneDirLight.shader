﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "HeroGo/General/OneDirLight/Bumped_Alpha_FadeOut"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Bump (RGB)", 2D) = "white" {}
		_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}

		_ShadowColor("Shadow Color(Toon color in shadow)",Color) = (0.0,0.0,0.0,0.0)
		_LittenColor("Litten Color(Toon color lit)",Color) = (1.0,1.0,1.0,1.0)

		//[HideInInspector]_ElapsedTime("Elapsed time", Range(0, 1)) = 0.0
		_ElapsedTime("Elapsed time", Float) = 0.0
		_AnimTimeLen("Animat time length", Range(0, 10)) = 0.6
	}

	SubShader
	{
		Tags
		{
			"LightMode" = "ForwardBase"
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}
		LOD 100

		Pass
		{
			Name "BUMPED_ALPHA_FADEOUT"

			Cull Back
			Lighting Off
			ZWrite On

			Fog{ Mode Off }

			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
					// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			#include "../Inc/Base.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv_MainTex		: TEXCOORD0;
				UNITY_FOG_COORDS(1)
					float3 WorldNormal		: TEXCOORD2;
				float3 WorldTangent		: TEXCOORD3;
				float3 WorldBinormal	: TEXCOORD4;

				float4 HPosition		: SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _BumpMap;
			sampler2D _Ramp;
			float4 _MainTex_ST;

			fixed4 _ShadowColor;
			fixed4 _LittenColor;

			half _ElapsedTime;
			half _AnimTimeLen;

			v2f vert(appdata v)
			{
				v2f o;
				o.HPosition = UnityObjectToClipPos(v.vertex);
				half3 binormal = cross(v.normal, v.tangent.xyz) * v.tangent.w;
				o.WorldNormal = mul(unity_ObjectToWorld, real4(v.normal, 0));
				o.WorldTangent = mul(unity_ObjectToWorld, real4(v.tangent.xyz, 0));
				o.WorldBinormal = mul(unity_ObjectToWorld, real4(binormal, 0));

				o.uv_MainTex = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.HPosition);

				return o;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				/// Albedo comes from a texture tinted by color
				fixed4 albedo = tex2D(_MainTex, IN.uv_MainTex);

				/// Normal map
				real3 bump = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));

				real3 Nn = normalize(IN.WorldNormal);
				real3 Tn = normalize(IN.WorldTangent);
				real3 Bn = normalize(IN.WorldBinormal);

				Nn = Nn * bump.z + bump.x * Tn + bump.y * Bn;
				Nn = normalize(Nn);

				fixed4 col = toon_term(_Ramp,_WorldSpaceLightPos0.xyz, Nn, albedo, _ShadowColor, _LittenColor);
				col.a = saturate(col.a * (1 - _ElapsedTime / _AnimTimeLen));

				// apply fog
				UNITY_APPLY_FOG(IN.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
	FallBack "HeroGo/General/OneDirLight/Bumped"
}

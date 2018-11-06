Shader "PACKT/Decal" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Metallic ("Metal (R), Smoothness (A)", 2D) = "white" {}
		_DecalTex ("Decal Albedo (RGB)", 2D) = "white" {}
	
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Metallic;
		sampler2D _DecalTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_DecalTex;
		};

		//half _Glossiness;
		//half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 d = tex2D (_DecalTex, IN.uv_DecalTex);
			fixed4 m = tex2D (_Metallic, IN.uv_MainTex);
			o.Albedo = lerp (c.rgb, d.rgb, d.a);
			// Metallic and smoothness come from slider variables
			o.Metallic = m.r;
			o.Smoothness = m.a;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

Shader "PACKT/Glass01" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		//_Specular ("Specular", Range(0,1)) = 0.0
		_Thickness ("Thickness", Range(-1,1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque"}
		Cull Back
		LOD 100
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf StandardSpecular fullforwardshadows alpha
		#pragma surface surf Standard fullforwardshadows alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		//half _Specular;
		fixed4 _Color;

		//void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			//o.Specular = _Specular;
			//o.Specular = _Color.a;
			o.Smoothness = _Glossiness;
			o.Alpha = _Color.a;
		}
		ENDCG
		
		Cull Front
		CGPROGRAM
      #pragma surface surf Standard fullforwardshadows alpha vertex:vert
      struct Input {
          float2 uv_MainTex;
      };
      float _Thickness;
      void vert (inout appdata_full v) {
          v.vertex.xyz += v.normal * _Thickness;
      }
     	sampler2D _MainTex;
      	half _Glossiness;
		//half _Specular;
		half _Metallic;
		fixed4 _Color;
		
		void surf (Input IN, inout SurfaceOutputStandard o) {
          fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			//o.Specular = _Specular;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a; 
      }
      ENDCG

	}
	FallBack "Diffuse"
}

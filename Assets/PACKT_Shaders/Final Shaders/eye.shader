Shader "PACKT/eye" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Metallic ("Metallic", Range(0,1)) = 0.0
		_SpecColor ("Specular Color", Color) = (1,1,1,1)
		_BumpMap ("Normal Map)", 2D) = "bump" {}
		_PupilScale("Pupil Size", Range(0, 100)) = 1.0
		_UVRange("UV Range", Range(50, 100)) = 50
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf StandardSpecular fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _PupilScale;
		float _UVRange;

		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
			float2 pos = IN.uv_MainTex.xy-0.5;
            float dist = 1-length(pos);
            dist = pow(dist, _UVRange);
            float2 disp = pos * clamp(dist * _PupilScale, 0.0, 1);
            pos -= disp;
            
            // Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, pos + 0.5);
			 //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			o.Specular = _SpecColor.rgb;
			o.Normal = UnpackNormal ( tex2D (_BumpMap, IN.uv_MainTex ));
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

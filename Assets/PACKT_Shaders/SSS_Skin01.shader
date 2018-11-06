Shader "PACKT/SSS_Skin01" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Metallic ("Metal (R), Smoothness (A)", 2D) = "white" {}
		//_Specular ("Specular", Range(0,1)) = 0.0
		_Scale ("Thickness Scale", Range(0,1)) = 0.0
		_BumpMap ("Normal Map)", 2D) = "bump" {}
		_SSS_Color ("Subsurface Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Diffusion ("Subsurface Diffusion", Float) = 0.0

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf SkinSSS fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _BumpMap;
		sampler2D _MainTex;
		sampler2D _Metallic;
		
		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		//half _Glossiness;
		//half _Metallic;
		half _Scale;
		fixed4 _Color;
		fixed4 _SSS_Color; 
		float _Diffusion;
		//half _Specular;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex)* _Color;
			fixed4 m = tex2D (_Metallic, IN.uv_MainTex);
			
			float3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
			float thickness = length (fwidth (WorldNormalVector (IN, n)))
			/length (fwidth (IN.worldPos)) * _Scale;
			o.Albedo = c.rgb + (_SSS_Color.rgb * thickness);
			o.Albedo = c.rgb * _Color.rgb;
			//o.Metallic = m.r;   
			//o.Smoothness = m.a;
			//o.Specular = 1.0;
			//o.Gloss = m.a;
			o.Gloss = m.a * m.r;
			//o.Alpha = c.a;
			o.Alpha = _Color.a;
		}
		
		inline fixed4 LightingSkinSSS (SurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)
		{
			//viewDir = normalize (viewDir);
			//lightDir = normalize (lightDir);

			half3 diffLightDir = lightDir + s.Normal * _Diffusion;
			float transDot = max(0, dot(viewDir, -diffLightDir)) * _Scale;
			fixed3 transLight = transDot * _SSS_Color.rgb * atten;
			fixed3 transAlbedo = transLight * s.Albedo * _LightColor0.rgb;

			half3 h = normalize (lightDir + viewDir);
			fixed diff = max(0, dot (s.Normal, lightDir));
			float nh = max(0, dot(s.Normal, h));
			float spec = pow (nh, s.Specular * 128.0) * s.Gloss;
			//float spec = pow (nh, s.Specular * s.Normal) * s.Gloss;
			fixed3 diffAlbedo = (s.Albedo * (_LightColor0.rgb * diff) + (_LightColor0.rgb * spec)) * atten;

			fixed4 c;
			c.rgb = diffAlbedo + transAlbedo;
			//c.a = _LightColor0.a * _SpecColor.a * spec * atten;
			//c.a = _LightColor0.a * spec * atten;
			c.a = _LightColor0.a * atten;
			return c;

		}
		ENDCG
	}
	FallBack "Diffuse"
}

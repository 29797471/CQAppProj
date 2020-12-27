Shader "ACT_P" {
	Properties {
	    _Color("Color",color)=(1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_LightTex("LightTex",2D)="white"{}
		_NormalTex("NormalTex",2D)="white"{}

		// Slider to control fading between normalmaps
		_BumpMapSlider ("BumpMap Slider", Range (0, 1)) = 0
		_BumpMap("BumpTex", 2D) = "white"{}

		_BrightnessAmount ("亮度 Amount", Range(0.0, 2.0)) = 1.0
		_SaturationAmount ("饱和度 Amount", Range(0.0, 1.0)) = 1.0
		_ContrastAmount ("对比度 Amount", Range(0.0, 1.0)) = 1.0
		_MetallicX("MetallicX", Range(0.0, 1.0)) = 0
		_Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200	
		Cull off
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0
		float4 _Color;
		sampler2D _MainTex;
		sampler2D _LightTex;
		sampler2D _NormalTex;
		sampler2D _BumpMap;
		
		fixed _BrightnessAmount;
		fixed _SaturationAmount;
		fixed _ContrastAmount;
		fixed _MetallicX;
		fixed _Smoothness;
		
		float _BumpMapSlider;
		struct Input {
			float2 uv_MainTex;
			float2 uv_LightTex;
			float2 uv_NormalTex;
			float2 uv_BumpMap;
		};
		half _Glossiness;
		half _Metallic;
		
		float3 ContrastSaturationBrightness (float3 color, float brt, float sat, float con) {
			// Increase or decrease these values to
			// adjust r, g and b color channels separately
			float avgLumR = 0.5;
			float avgLumG = 0.5;
			float avgLumB = 0.5;
			
			// Luminance coefficients for getting luminance from the image
			float3 LuminanceCoeff = float3 (0.2125, 0.7154, 0.0721);
			
			// Operation for brightmess
			float3 avgLumin = float3 (avgLumR, avgLumG, avgLumB);
			float3 brtColor = color * brt;
			float intensityf = dot (brtColor, LuminanceCoeff);
			float3 intensity = float3 (intensityf, intensityf, intensityf);
			
			// Operation for saturation
			float3 satColor = lerp (intensity, brtColor, sat);
			
			// Operation for contrast
			float3 conColor = lerp (avgLumin, satColor, con);
			
			return conColor;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex)*_Color;
			c*=tex2D (_LightTex, IN.uv_LightTex);
			c.rgb = ContrastSaturationBrightness (c.rgb, _BrightnessAmount, _SaturationAmount, _ContrastAmount);
			o.Albedo = c.rgb;
			o.Metallic = _MetallicX;
			o.Smoothness = _Smoothness;
			o.Alpha = c.a;
			
	    	//o.Normal=UnpackNormal(tex2D(_NormalTex,IN.uv_NormalTex));			
			fixed3 normal = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex));
			fixed3 bump = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Normal = lerp(normal, bump, _BumpMapSlider*10);
		}

		ENDCG

	} 
	FallBack "Diffuse"
}

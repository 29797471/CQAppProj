Shader "ACT1.0" {
	Properties {
	    _Color("Color",color)=(1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_LightTex("LightTex",2D)="white"{}
		_NormalTex("NormalTex",2D)="white"{}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200	
		Cull back
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0
		float4 _Color;
		sampler2D _MainTex;
		sampler2D _LightTex;
		sampler2D _NormalTex;
		struct Input {
			float2 uv_MainTex;
			float2 uv_LightTex;
			float2 uv_NormalTex;
		};
		half _Glossiness;
		half _Metallic;
		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex)*_Color;
			c*=tex2D (_LightTex, IN.uv_LightTex);
			o.Albedo = c.rgb;
			o.Metallic = 0;
			o.Smoothness = 1;
			o.Alpha = c.a;
	    	o.Normal=UnpackNormal(tex2D(_NormalTex,IN.uv_NormalTex));			
		}
		ENDCG
		Cull front
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0
		sampler2D _MainTex;
		struct Input {
			float2 uv_MainTex;
		};
		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) ;
			o.Albedo = c.rgb*0.5;
			o.Metallic = 0;
			o.Smoothness = 1;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}

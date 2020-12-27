Shader "AlphaYY 2" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_NewMainTex ("New Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        _USpeed("USpeed ", float) = 1.0
        _UCount("UCount", float) = 1.0
        _VSpeed("VSpeed", float) = 1.0
        _VCount("VCount", float) = 1.0
}

SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 200

CGPROGRAM
#pragma surface surf Lambert alphatest:_Cutoff

sampler2D _MainTex;
sampler2D _NewMainTex;
fixed4 _Color;
    // U�᷽������ٶ�
        float _USpeed; 
        // U�᷽��ƽ�̸���
        float _UCount;

        // V�᷽������ٶ�
        float _VSpeed;
        // V�᷽��ƽ�̸���
        float _VCount;

struct Input {
	float2 uv_MainTex;
	float2 uv_NewMainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
    float2 uv = IN.uv_NewMainTex;
	 float detalTime = 1;

            //  ����X�᷽��仯
            uv.x += detalTime * _USpeed;
            uv.x *=  _UCount;

            // ����Y�᷽��仯
            uv.y += detalTime * _VSpeed;
            uv.y *= _VCount;

	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	fixed4 c1 = tex2D( _NewMainTex, IN.uv_NewMainTex);
	o.Albedo = c.rgb ;
	o.Alpha = c1.a;
}
ENDCG
}

Fallback "Legacy Shaders/Transparent/VertexLit"
}

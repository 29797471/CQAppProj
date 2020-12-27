Shader "AlphaYHMask" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_YHTex ("YHTex", 2D) = "white" {}
	_MaskTex("MaskTexture",2D) = "white"{}
	 _RotationSpeed ("Rotation Speed", Float) = 0
	 _ScrollX("_ScrollX",Float) = -0.06
	 _ScrollY("_ScrollY",Float) = -0.08
    _Scale("_Scale",Range(0,0.4)) = 0
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 200

CGPROGRAM
#pragma surface surf Lambert vertex:vert alpha:blend
sampler2D _MainTex;
sampler2D _MaskTex;
sampler2D _YHTex;
float4 _Color;

float _ScrollX;
float _ScrollY;
float _Scale;

struct Input {
	float2 uv_MainTex;
	float2 uv_MaskTex;
	float2 uv_YHTex;
};

    
            float _RotationSpeed;

		 

		float2 mapuv(float2 uv,float xLength)  
          {  
            float2 newuv;  
            float rate=(1-2*xLength)/1;        
            if(uv.x>xLength&&uv.x<(1-xLength)&&uv.y>xLength&&uv.y<(1-xLength))  
            {  
                 newuv.x=(uv.x-xLength)/rate;  
                 newuv.y=(uv.y-xLength)/rate;  
            }  
            else  
            {  
               newuv.x=uv.x;  
               newuv.y=uv.y;  
            }  
             return newuv;  
          }  
		   void vert (inout appdata_full v) {
                v.texcoord.xy -=0.5;
                float s = sin ( _RotationSpeed);
	            float c = cos ( _RotationSpeed);
                float2x2 rotationMatrix = float2x2( c, -s, s, c);
                rotationMatrix *=0.5;
                rotationMatrix +=0.5;
                rotationMatrix = rotationMatrix * 2-1;
             //   v.texcoord.xy = mul ( v.texcoord.xy, rotationMatrix );
             //   v.texcoord.xy += 0.5;

			    float2 newuv = mapuv( v.texcoord.xy,_Scale);
				(newuv.x ) +=_ScrollX;
				(newuv.y ) +=_ScrollY;
                v.texcoord.xy = mul ( newuv, rotationMatrix );
                v.texcoord.xy += 0.5;

			 //**mapuv() 方法拿出来
		     //	  float2 newuv;  
			//	  float rate=(1-2*_Scale)/1;        
			//	  newuv.x=(v.texcoord.xy.x-_Scale)/rate;  
			//	  newuv.y=(v.texcoord.xy.y-_Scale)/rate;  
			//	  newuv.x +=_ScrollX;
			//	  newuv.y +=_ScrollY;
            //      v.texcoord.xy = mul ( newuv, rotationMatrix );
             //     v.texcoord.xy += 0.5;
            }
void surf (Input IN, inout SurfaceOutput o) {

	//float4 c = tex2D(_MainTex, scrolledUV) * _Color; 

	fixed4 yh = tex2D( _YHTex, IN.uv_YHTex);
	fixed4 c1 = tex2D( _MaskTex, IN.uv_MaskTex);
	   float xLength= _Scale;  
	 float2 newuv=mapuv(IN.uv_MainTex,xLength);  

	          //**mapuv() 方法拿出来

     fixed4 c = tex2D(_MainTex,newuv ) * _Color;
            if(!(IN.uv_MainTex.x>xLength&&IN.uv_MainTex.x<(1-xLength)&&IN.uv_MainTex.y>xLength&&IN.uv_MainTex.y<(1-xLength)))  
               c=float4(0,0,0,0);  
 
	o.Albedo = c.rgb * c1.rgb;
	o.Alpha = c1.a;
}
ENDCG
}

Fallback "Legacy Shaders/Transparent/VertexLit"
}

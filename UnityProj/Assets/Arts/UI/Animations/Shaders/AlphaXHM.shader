Shader "AlphaXHM" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	 _RotationSpeed ("Rotation Speed", Float) = 0
	 _ScrollX("_ScrollX",Float) = 0.48
	 _ScrollY("_ScrollY",Float) = 0.02
    _Scale("_Scale",Range(0,0.4)) = 0
}

SubShader {
	Tags {"Queue"="Transparent" "RenderType"="Transparent"}
	LOD 200

CGPROGRAM
#pragma surface surf Standard fullforwardshadows vertex:vert alpha:blend

		#pragma target 3.0

sampler2D _MainTex;
float4 _Color;

float _RotationSpeed;
float _ScrollX;
float _ScrollY;
float _Scale;

struct Input {
	float2 uv_MainTex;
};

    

		 

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

            }
void surf (Input IN, inout SurfaceOutputStandard o) {

	   float xLength= _Scale;  
   	   float2 newuv=mapuv(IN.uv_MainTex,xLength);  

	          //**mapuv() 方法拿出来
			   //   float2 newuv;  
				//  float rate=(1-2*_Scale)/1;        
				//  newuv.x=(IN.uv_MainTex.x-_Scale)/rate;  
				//  newuv.y=(IN.uv_MainTex.y-_Scale)/rate;

     fixed4 c = tex2D(_MainTex,newuv ) * _Color;
            if(!(IN.uv_MainTex.x>xLength&&IN.uv_MainTex.x<(1-xLength)&&IN.uv_MainTex.y>xLength&&IN.uv_MainTex.y<(1-xLength)))  
               c=float4(0,0,0,0);  
   
	o.Albedo = c.rgb;
	//o.Smoothness = 0.8;
	o.Alpha = c.a;
}
ENDCG
}

Fallback "Legacy Shaders/Transparent/VertexLit"
}

Shader "Custom/UnitFade"
{
	//-------------------------------【属性】------------------------------------
	Properties
	{
		_MainTex("基础纹理(RGBA)", 2D) = "white" {}
		_Color("颜色(RGBA) ", Color) = (1,1,1,1)
		_Cutoff("Alpha cutoff", Range(0.000000,1.000000)) = 0.500000
	}

	//--------------------------------【子着色器】--------------------------------  
	SubShader
	{
		Tags{ "Queue" = "AlphaTest" "RenderType" = "TransparentCutout" 
		"PreviewType" = "Plane" }
		Pass
		{
			//关闭光照
			Lighting Off
			//ZWrite Off
			//裁剪背面
			Cull back
			//Tags{ "Queue" = "AlphaTest" }
			//目标Alpha通道混合
			Blend SrcAlpha OneMinusSrcAlpha
			//Blend One OneMinusSrcAlpha
			//标记（队列：透明）
			

			//纹理与颜色混合 
			SetTexture[_MainTex]{ConstantColor[_Color] Combine Texture * constant }
		}
	}
}

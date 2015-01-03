Shader "Custom/AddMapShader" {
Properties {
	_MainColor ("Main Tint color", Color)  = (1.0, 1.0, 1.0, 1.0) // color
	_MainTex   ("Base (RGB)", 2D) = "white" {}
	_MixColor ("Mix Tint color", Color)  = (1.0, 1.0, 1.0, 1.0) // color
	_MixTex    ("Mix (RGB)", 2D) = "white" {}	
	magColor   ("magnification",Range(0.1, 5.0)) = 1.0	
	TimeFactor ("TimeFactor",Float) = 1.0	
	_Cutoff ("Alpha cutoff", Range (0,1)) = 0.0	
}



Category {
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	SubShader {
		Pass {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		
			AlphaTest GEqual [_Cutoff] 
			BlendOp Add
	  		Blend One One	  		
			ZTest Always
			Fog { Mode off }			
			Cull Off
				
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#include "UnityCG.cginc"

float _Cutoff = 0.0;

struct v2f {
	float4 pos	 : POSITION;
	float4 color : COLOR;
	float2 uv0	 : TEXCOORD0;
	float2 uv1	 : TEXCOORD1;
}; 

uniform sampler2D _MainTex;
uniform sampler2D _MixTex;

float magColor = 1.0;
float TimeFactor = 1.0;
float BlendValue = 1.0;

float GrayFactor = 0.0;

float4 OffsetUV = float4(0,0,0,0);

float4 _MainColor;
float4 _MixColor;

float4x4 Tex0Matrix =
	float4x4 (
         1,0,0,0,
         0,1,0,0,
         0,0,1,0,
         0,0,0,1
    );

float4x4 Tex1Matrix =
	float4x4 (
         1,0,0,0,
         0,1,0,0,
         0,0,1,0,
         0,0,0,1
    );

float4 Tex0Translation = float4(0,0,0,0);
float4 Tex1Translation = float4(0,0,0,0);

v2f vert (appdata_img v)
{
	v2f o;
	    
	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		
	o.uv0 = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord);
	o.uv1 = MultiplyUV (UNITY_MATRIX_TEXTURE1, v.texcoord);
	
	float4 Axis = float4(o.uv0.x,o.uv0.y,0,1);	
	
	Axis = mul(Tex0Matrix, Axis);
	o.uv0.x  = Axis.x + OffsetUV.x;
	o.uv0.y  = Axis.y + OffsetUV.y;
	
	Axis = float4(o.uv1.x,o.uv1.y,0,1);
	
	Axis = mul(Tex1Matrix, Axis);
	o.uv1.x  = Axis.x + OffsetUV.z;
	o.uv1.y  = Axis.y + OffsetUV.w;
	
	o.uv0.x = o.uv0.x + (Tex0Translation.x * TimeFactor);
	o.uv0.y = o.uv0.y + (Tex0Translation.y * TimeFactor);
	
	o.uv1.x = o.uv1.x + (Tex1Translation.x * TimeFactor);
	o.uv1.y = o.uv1.y + (Tex1Translation.y * TimeFactor);
	
	return o;
}

float4 frag (v2f i) : COLOR
{	
	float4 col = tex2D(_MainTex, i.uv0) ;
	float4 adapted = tex2D(_MixTex, i.uv1);	
	
	
	float4 GrayCol;	
	float GC = (col.r + col.g + col.b) / 3.0;
	GrayCol = float4(GC,GC,GC,col.a);	
	col = ((1.0 - GrayFactor) * col) + (GrayFactor * GrayCol);
	
	GC = (adapted.r + adapted.g + adapted.b) / 3.0;
	GrayCol = float4(GC,GC,GC,adapted.a);	
	adapted = ((1.0 - GrayFactor) * adapted) + (GrayFactor * GrayCol);
	
	
	col.rgba = col.rgba * _MainColor.rgba;
	adapted.rgba = adapted.rgba * _MixColor.rgba;
	
		
	float4 ResCol;
	ResCol.rgb = (col.rgb * adapted.rgb) * magColor  * BlendValue;
	
	ResCol.a = adapted.a * col.a * BlendValue;
			
	return ResCol;
}
ENDCG

		}//end pass
	}

}

Fallback off
}

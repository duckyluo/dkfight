Shader "Dk/DkTransparent" 
{
	Properties 
	{
		_MainTex("Texture", 2D) = "white" {}
		_Transparent("Transparent",Float) = 1
	}
	
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		//AlphaTest Greater 0.2
		
		Pass
		{
				
CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float _Transparent;
			
			struct v2f 
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert(appdata_img v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord);
				return o;
			}

			float4 frag (v2f i) : COLOR
			{
				float4 outp;
                
                outp = tex2D(_MainTex,i.uv);
       
     			if(outp.w > _Transparent)
           		{
           			outp.w = _Transparent;
           			if(_Transparent > 0)
           			{
           				
           			}
                	else
                	{
                		outp.w = 0;
                	}
                }
                return outp;
			}
ENDCG
		}
	}
	FallBack "Diffuse"
}

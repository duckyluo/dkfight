Shader "Dk/DkFlashLight" 
{
	Properties 
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color)  = (1,1,1,1)
		_Strength("Strength",Float) = 0.5
		_Transparent("Transparent",Float) = 0
	}
	
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
//		Blend SrcAlpha OneMinusSrcAlpha
//		AlphaTest Greater 0.4
		Pass
		{
				
CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float4 _Color;
			float _Strength;
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
			
			//interval间隔 beginTime开始时间 strength强度
			float4 inFlash(int interval,int beginTime,float strength)
            {
                
                float brightness = 0;
              
                float currentTime = _Time.y;
           
                int currentTimeInt = _Time.y/interval;
                currentTimeInt *=interval;
                  
                float currentTimePassed = currentTime -currentTimeInt;
                float endTime = interval - 2*beginTime;
                if(currentTimePassed > beginTime && currentTimePassed < endTime)
                {
                	
                	brightness = (1 - (abs(currentTimePassed - interval*0.5) / (interval*0.5)))*strength;
                }

                brightness = max(brightness,0);
               
                float4 col = _Color*brightness;
                return col;
            }
			
			float4 frag (v2f i) : COLOR
			{
				float4 outp;
                
                float4 texCol = tex2D(_MainTex,i.uv);
       
                float4 tmpBrightness = inFlash(1.5,0.9,_Strength);
           
           		if(_Transparent > 0)
           		{
	           		if(texCol.w > 0.4)
	           		{
	                	outp = texCol + tmpBrightness;
	                }
	                else
	                {
	                    outp = float4(0,0,0,0);
	                }
	           	}
           		else
           		{
           			outp = texCol + tmpBrightness;
           		}
  
                return outp;
			}
ENDCG
		}
	}
	FallBack "Diffuse"
}

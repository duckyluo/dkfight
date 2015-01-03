Shader "Dk/DkColor" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Color", Color)  = (1,1,1,1)
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		
		AlphaTest Greater 0.4
		
		CGPROGRAM
		#pragma surface surf Lambert finalcolor:chcolor

		sampler2D _MainTex;
		float4 _Color;

		struct Input 
		{
			float2 uv_MainTex;
		};
		
		void chcolor (Input In,SurfaceOutput o, inout fixed4 color)
		{
			if(color.a > 0.5 )
			{
				color = lerp(color,_Color,0.5);
			}
		}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}

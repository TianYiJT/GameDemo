// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Mask/MaskLayer" 
{

	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MaskLayer("Culling Mask",2D) = "white"{}
	}
	SubShader 
	{
		Pass
		{
			Blend SrcColor OneMinusSrcColor
			cull off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include"UnityCG.cginc"
			sampler2D _MainTex;
			sampler2D _MaskLayer;
			struct v2f
			{
				float4 pos:POSITION;
				float2 uv:TEXCOORD0;
			};
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos=UnityObjectToClipPos(v.vertex);
				o.uv=v.texcoord.xy;
				return o;
			}
			fixed4 frag(v2f IN):COLOR
			{
				fixed4 col_main=tex2D(_MainTex,IN.uv);
				fixed4 col_mask=tex2D(_MaskLayer,IN.uv);
				return col_main*col_mask*1.5;
			}
			ENDCG
		} 

	}
}
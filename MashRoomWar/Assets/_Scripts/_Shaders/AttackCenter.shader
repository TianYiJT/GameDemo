// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/AttackCenter" 
{
Properties 
	{
		
		Main_Color("maincolor",Color)=(1,1,1,1)
	}
	SubShader 
	{
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include"UnityCG.cginc"
			fixed4 Main_Color;
			struct v2f
			{
				float4 pos:POSITION;
				float2 uv:TEXCOORD0;
			};
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos=UnityObjectToClipPos(v.vertex);
				return o;
			}
			fixed4 frag(v2f IN):COLOR
			{
				return Main_Color;
			}
			ENDCG
		} 

	}
}

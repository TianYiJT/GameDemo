// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DefaultMatForPrefab" 
{
Properties 
	{
		Main_Color("maincolor",Color)=(1,1,1,1)
	}
	SubShader 
	{
		
		Pass
		{
			Tags 
			{  
            	"IgnoreProjector"="True"  
            	"Queue"="Transparent"  
            	"RenderType"="Transparent"  
        	}
        	cull off
        	Blend SrcAlpha OneMinusSrcAlpha  
            ZWrite Off    
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include"UnityCG.cginc"
			fixed4 Main_Color;
			struct v2f
			{
				float4 pos:POSITION;
				float diff:TEXCOORD0;
			};
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos=UnityObjectToClipPos(v.vertex);
				float3 N=normalize(v.normal);
				float3 L=_WorldSpaceLightPos0;
				L=mul(unity_WorldToObject,float4(L,0)).xyz;
				L=normalize(L);
				o.diff=saturate(dot(N,L));
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

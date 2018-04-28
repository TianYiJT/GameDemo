// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Water" 
{
Properties 
	{
		Water_Texture("Albedo (RGB)", 2D) = "white" {}
		Main_Color("maincolor",Color)=(1,1,1,1)
		Second_Color("Seccolor",Color)=(1,1,1,1)
		_Intensity("_Intensity",float)=1
		_ToonKit("ToonKit", 2D) = "white" {}
	}
	SubShader 
	{
		
		Pass
		{
			Tags 
			{  
            	"Queue"="Transparent+1"  
            	"RenderType"="Transparent"  
        	}
        	Cull off
        	Blend SrcAlpha OneMinusSrcAlpha  
            ZWrite off 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include"UnityCG.cginc"
			sampler2D Water_Texture;
			sampler2D _ToonKit;
			fixed4 Second_Color;
			float _Intensity;
			float4 Water_Texture_ST;
			fixed4 Main_Color;
			struct v2f
			{
				float4 pos:POSITION;
				float diff:TEXCOORD0;
				float2 uv:TEXCOORD1;

			};
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos=UnityObjectToClipPos(v.vertex);
				o.uv=v.texcoord.xy;
				float3 N=normalize(v.normal);
				float3 L=_WorldSpaceLightPos0;
				L=mul(unity_WorldToObject,float4(L,0)).xyz;
				L=normalize(L);
				o.diff=saturate(dot(N,L));
				return o;
			}
			fixed4 frag(v2f IN):COLOR
			{
				
				IN.uv.x+=sin(_Time.x)*1.5f;
				IN.uv.y-=cos(_Time.x)*1.5f;
				fixed4 _water=tex2D(Water_Texture,IN.uv);
				fixed4 _ToonColor=tex2D(_ToonKit,IN.uv);
				//fixed4 _diff=_ToonColor*IN.diff;
				float mag_water=_water.x*0.299f+_water.y*0.587f+_water.z*0.114f;
				fixed4 col;
				if(mag_water>_Intensity*0.02f)
				{
					_water*=1.15f;
				}
				col=Main_Color*(_water);
				//col+=_diff;
				col*=1.8f;
				//float mag_col=col.x*0.299f+col.y*0.587f+col.z*0.114f;
				col+=Second_Color;
				return col;
			}
			ENDCG
		} 
	}
}
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/3DPrint" {
	Properties 
	{
		_ConstructY("constructY",float)=1
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_ConstructSize("constructsize",float)=1
		_SecondColor("secendcolor",Color)=(1,1,1,1)
	}
	SubShader 
	{
	pass
	{
		cull off
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 3.0
		#include"UnityCG.cginc"
		#include"Lighting.cginc"
		struct v2f
		{
			float4 pos:POSITION;
			float2 uv_MainTex:TEXCOORD0;
			float4 vertex_model:TEXCOORD1;
			float3 normal:TEXCOORD2;
		};
		float _ConstructY;
		sampler2D _MainTex;
		float4 _SecondColor;
		float _ConstructSize;
		v2f vert (appdata_full v) 
		{
			v2f vop;
			vop.pos=UnityObjectToClipPos(v.vertex);
			vop.vertex_model=v.vertex;
			vop.normal=v.normal;
			vop.uv_MainTex=v.texcoord.xy;
			return vop;
		}
		fixed4 frag(v2f vop):COLOR
		{
			float ObjectY=vop.vertex_model.y;

			if(ObjectY>_ConstructY+_ConstructSize)
			{
				discard;
			}
			else if(ObjectY<_ConstructY)
			{
				float3 nor=vop.normal;
				float3 L=normalize(_WorldSpaceLightPos0);
				float3 Lig=mul(unity_WorldToObject,float4(L,0)).xyz;
				nor=normalize(nor);
				Lig=normalize(Lig);
				float diffuse_Object=dot(nor,Lig);
				diffuse_Object=saturate(diffuse_Object);
				fixed4 Light_Diffuse=diffuse_Object*_LightColor0;
				float3 reflectLight=reflect(Lig,nor);
				float3 View=WorldSpaceViewDir(vop.vertex_model);
				View=mul(unity_WorldToObject,float4(View,0)).xyz;
				View=normalize(View);
				reflectLight=normalize(reflectLight);
				float specularscale=pow(saturate(dot(View,reflectLight)),8);
				fixed4 Light_specular=specularscale*_LightColor0;
				fixed4 _color=tex2D(_MainTex,vop.uv_MainTex);
				_color+=Light_Diffuse;
				_color+=Light_specular;
				return _color;
			}
			else
			{
				return _SecondColor;
			}
			return fixed4(1,1,1,1);
		}
		ENDCG
	}
	}
	FallBack "Diffuse"
}

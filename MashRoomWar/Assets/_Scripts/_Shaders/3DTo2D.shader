// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/3DTo2D" {
	Properties 
	{
		_Z("Z",float)=1
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
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
		};
		float _Z;
		sampler2D _MainTex;
		float4 _SecondColor;
		v2f vert (appdata_full v) 
		{
			v2f vop;
			vop.pos=UnityObjectToClipPos(v.vertex);
			vop.pos.z=_Z;
			vop.uv_MainTex=v.texcoord.xy;
			return vop;
		}
		fixed4 frag(v2f vop):COLOR
		{
			fixed4 col=tex2D(_MainTex,vop.uv_MainTex);
			col*=_SecondColor;
			return col;
		}
		ENDCG
	}
	}
	FallBack "Diffuse"
}

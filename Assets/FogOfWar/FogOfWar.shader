// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/FogOfWar" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_FogRadius ("FogRadius", Float) = 1.0
	_FogMaxRadius ("FogMaxRadius", Float) = 0.5
	_Player_Pos ("_Player_Pos", Vector) = (0,0,0,1)
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 200
	Cull Off

CGPROGRAM
#pragma surface surf Lambert vertex:vert

sampler2D _MainTex;
fixed4 _Color;
float  _FogRadius;
float  _FogMaxRadius;
float4 _Player_Pos;

struct Input {
    float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
    o.Albedo = c.rgb;
    o.Alpha = c.a;
}
ENDCG
}

Fallback "Legacy Shaders/Transparent/VertexLit"
}

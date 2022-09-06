// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/FX_Fresnal"
{
	Properties
	{
		_RimColor("RimColor", Color) = (0,0,0,0)
		_RimBias("RimBias", Range( -1 , 1)) = -0.3764706
		_RimScale("RimScale", Range( 0 , 1)) = 1
		_RimPower("RimPower", Range( 0 , 1)) = 1
		_Color0("Color 0", Color) = (0,0,0,0)
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float4 uv_tex4coord;
		};

		uniform float4 _Color0;
		uniform float4 _RimColor;
		uniform float _RimBias;
		uniform float _RimScale;
		uniform float _RimPower;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV6 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode6 = ( _RimBias + _RimScale * pow( 1.0 - fresnelNdotV6, _RimPower ) );
			o.Emission = ( _Color0 + ( _RimColor * fresnelNode6 * 0.35 ) ).rgb;
			o.Alpha = i.uv_tex4coord.w;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
7;211;1906;808;1517.236;323.61;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;8;-1167.86,257.5913;Inherit;False;Property;_RimScale;RimScale;2;0;Create;True;0;0;False;0;False;1;0.7611759;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1199.86,333.5913;Inherit;False;Property;_RimPower;RimPower;3;0;Create;True;0;0;False;0;False;1;0.4317641;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1160.86,174.5913;Inherit;False;Property;_RimBias;RimBias;1;0;Create;True;0;0;False;0;False;-0.3764706;-0.4658835;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-739.8599,439.5913;Inherit;False;Constant;_Emission;Emission;0;0;Create;True;0;0;False;0;False;0.35;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;6;-928.4778,161.4048;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-937.8599,-175.4087;Inherit;False;Property;_RimColor;RimColor;0;0;Create;True;0;0;False;0;False;0,0,0,0;0.1367925,1,0.9911819,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-502.8599,151.5913;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;19;-473.2356,-102.61;Inherit;False;Property;_Color0;Color 0;4;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-276.8599,339.5913;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-195.2356,71.38995;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Unlit/FX_Fresnal;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;1;7;0
WireConnection;6;2;8;0
WireConnection;6;3;9;0
WireConnection;11;0;10;0
WireConnection;11;1;6;0
WireConnection;11;2;12;0
WireConnection;20;0;19;0
WireConnection;20;1;11;0
WireConnection;0;2;20;0
WireConnection;0;9;16;4
ASEEND*/
//CHKSM=60400D1E70DEC7DFD47A5D1F7F924F56B78D9E9F
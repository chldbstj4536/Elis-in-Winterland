// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/Fx_Dissolve"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_NoiseTex("NoiseTex", 2D) = "white" {}
		_timescale("timescale", Float) = 0.12
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Overlay+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		ZTest LEqual
		Blend One One , One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 uv2_tex4coord2;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _timescale;
		uniform sampler2D _NoiseTex;
		uniform float4 _NoiseTex_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float mulTime4 = _Time.y * _timescale;
			float2 temp_cast_0 = (mulTime4).xx;
			float4 uv2_TexCoord6 = i.uv2_tex4coord2;
			uv2_TexCoord6.xy = i.uv2_tex4coord2.xy + temp_cast_0;
			float2 uv_NoiseTex = i.uv_texcoord * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
			float smoothstepResult11 = smoothstep( ( uv2_TexCoord6.z + 0 ) , 1.25 , ( tex2D( _NoiseTex, uv_NoiseTex ).r - 0.0 ));
			float4 temp_output_14_0 = ( i.vertexColor * ( tex2D( _TextureSample0, uv_TextureSample0 ) * smoothstepResult11 ) * i.vertexColor.a );
			o.Emission = temp_output_14_0.rgb;
			o.Alpha = temp_output_14_0.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
83;89;1305;816;1880.391;567.0046;1.618078;True;False
Node;AmplifyShaderEditor.RangedFloatNode;3;-1795.11,221.5421;Inherit;False;Property;_timescale;timescale;3;0;Create;True;0;0;False;0;False;0.12;0.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;4;-1630.984,185.192;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1426.1,98.17222;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;7;-1356.227,341.25;Inherit;False;Constant;_Vector0;Vector 0;4;0;Create;True;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;5;-1451.496,-140.3797;Inherit;True;Property;_NoiseTex;NoiseTex;2;0;Create;True;0;0;False;0;False;-1;30f1c1170de113741bac7aa047b56e4f;30f1c1170de113741bac7aa047b56e4f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;8;-1153.805,-148.6379;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-1156.229,260.0947;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-1002.383,-353.7218;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;False;-1;218b30c1529b85441af7f7aa4cc9be19;218b30c1529b85441af7f7aa4cc9be19;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;11;-857.6226,-19.40199;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;12;-527.0031,-232.7246;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-545.0535,6.633126;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-274.7705,-132.7361;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;2;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Unlit/Fx_Dissolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;2;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;False;Opaque;;Overlay;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;3;0
WireConnection;6;1;4;0
WireConnection;8;0;5;1
WireConnection;9;0;6;3
WireConnection;9;1;7;1
WireConnection;11;0;8;0
WireConnection;11;1;9;0
WireConnection;13;0;10;0
WireConnection;13;1;11;0
WireConnection;14;0;12;0
WireConnection;14;1;13;0
WireConnection;14;2;12;4
WireConnection;2;2;14;0
WireConnection;2;9;14;0
ASEEND*/
//CHKSM=EB1D2E685E1E1861C85D6892F3CE030073E22703
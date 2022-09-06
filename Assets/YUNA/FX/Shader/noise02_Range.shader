// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "noise02_Range"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Noise("Noise", 2D) = "white" {}
		_radial("radial", Range( 0 , 0.5)) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Overlay"  "Queue" = "Overlay+0" "IsEmissive" = "true"  }
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
			float4 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _MainTex;
		uniform sampler2D _Noise;
		uniform float _radial;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 tex2DNode11 = tex2D( _MainTex, i.uv_texcoord.xy );
			float2 panner5 = ( 1.0 * _Time.y * float2( 0,0 ) + i.uv_texcoord.xy);
			float4 tex2DNode9 = tex2D( _Noise, panner5 );
			float temp_output_7_0 = ( i.uv_texcoord.z + (0.0 + (_radial - 0.0) * (1.2 - 0.0) / (1.0 - 0.0)) );
			float smoothstepResult10 = smoothstep( tex2DNode9.r , 1.25 , temp_output_7_0);
			float temp_output_13_0 = ( tex2DNode9.a * smoothstepResult10 * tex2DNode11.a * i.vertexColor.a );
			o.Emission = ( tex2DNode11 * temp_output_13_0 * i.vertexColor ).rgb;
			o.Alpha = temp_output_13_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18933
7;107;1906;904;2233.71;-141.5109;1.163895;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1493.849,297.6969;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;3;-1436.96,436.5048;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;16;-1715.236,861.2554;Inherit;False;Property;_radial;radial;3;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;5;-1224.197,321.5902;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-1298.836,558.786;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;17;-1271.603,863.5613;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-1383.981,-95.91913;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;9;-1007.926,300.8214;Inherit;True;Property;_Noise;Noise;2;0;Create;True;0;0;0;False;0;False;-1;None;407f21a347441e04b89588358213ac17;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-983.8361,644.786;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;10;-766.8607,479.8926;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-1069.891,-107.9037;Inherit;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;0;False;0;False;-1;None;6b544d3fbef240a4a97eaf87d0945379;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;12;-779.332,-336.074;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-532.1721,298.3661;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;14;-356.3093,626.9265;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-481.8625,-48.30527;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;1;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;noise02_Range;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;2;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Overlay;;Overlay;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;2;0
WireConnection;5;2;3;0
WireConnection;17;0;16;0
WireConnection;9;1;5;0
WireConnection;7;0;4;3
WireConnection;7;1;17;0
WireConnection;10;0;7;0
WireConnection;10;1;9;1
WireConnection;11;1;8;0
WireConnection;13;0;9;4
WireConnection;13;1;10;0
WireConnection;13;2;11;4
WireConnection;13;3;12;4
WireConnection;14;0;7;0
WireConnection;14;1;9;1
WireConnection;15;0;11;0
WireConnection;15;1;13;0
WireConnection;15;2;12;0
WireConnection;1;2;15;0
WireConnection;1;9;13;0
ASEEND*/
//CHKSM=7F1403042264DBA6BFA945273F97F08F8C93DDD3
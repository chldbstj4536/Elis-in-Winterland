// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Beam_Add"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_DistortionStrength("DistortionStrength", Float) = 0.27
		_Vector0("Vector 0", Vector) = (3,1,0,0)
		_Float0("Float 0", Float) = 0.36
		_Vector1("Vector 1", Vector) = (3,1,0,0)
		_Float1("Float 1", Float) = 2
		_ScrollSpeed("ScrollSpeed", Float) = 0.3
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Overlay+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend One One , One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform float2 _Vector0;
		uniform float _ScrollSpeed;
		uniform sampler2D _TextureSample2;
		uniform float2 _Vector1;
		uniform float _Float1;
		uniform float _Float0;
		uniform float _DistortionStrength;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float mulTime6 = _Time.y * -1.0;
			float temp_output_27_0 = ( mulTime6 * _ScrollSpeed );
			float2 temp_cast_0 = (temp_output_27_0).xx;
			float2 uv_TexCoord5 = i.uv_texcoord * _Vector0 + temp_cast_0;
			float2 temp_cast_1 = (( temp_output_27_0 * _Float1 )).xx;
			float2 uv_TexCoord25 = i.uv_texcoord * _Vector1 + temp_cast_1;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			o.Emission = ( i.vertexColor * ( tex2D( _TextureSample0, ( uv_TexCoord5 + ( ( tex2D( _TextureSample2, uv_TexCoord25 ).r * _Float0 ) * _DistortionStrength ) ) ).r * tex2D( _TextureSample1, uv_TextureSample1 ).r ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18933
7;12;1906;999;3081.819;477.2256;1.674882;True;True
Node;AmplifyShaderEditor.RangedFloatNode;34;-2539.674,126.0266;Inherit;False;Constant;_Float2;Float 2;10;0;Create;True;0;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;6;-2398.554,93.59846;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-2333.215,283.5041;Inherit;False;Property;_ScrollSpeed;ScrollSpeed;9;0;Create;True;0;0;0;False;0;False;0.3;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-2060.215,113.5041;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-2045.947,400.6515;Inherit;False;Property;_Float1;Float 1;8;0;Create;True;0;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1773.047,303.9515;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;26;-1710.847,175.0515;Inherit;False;Property;_Vector1;Vector 1;7;0;Create;True;0;0;0;False;0;False;3,1;3,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-1510.347,248.8515;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;-1244.036,143.4382;Inherit;True;Property;_TextureSample2;Texture Sample 2;3;0;Create;True;0;0;0;False;0;False;-1;None;59ea3fa69f7e1904a8a81ae0b50938ff;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-1143.036,356.4382;Inherit;False;Property;_Float0;Float 0;6;0;Create;True;0;0;0;False;0;False;0.36;0.36;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-785.3843,442.5988;Inherit;False;Property;_DistortionStrength;DistortionStrength;4;0;Create;True;0;0;0;False;0;False;0.27;0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;22;-1591.347,-196.4485;Inherit;True;Property;_Vector0;Vector 0;5;0;Create;True;0;0;0;False;0;False;3,1;2,0.25;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-925.0361,236.4382;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-640.3843,272.5988;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1278.555,-25.40161;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-552.0361,20.43823;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-329.4737,-80.88969;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;4bef76abd9a1e674688951217036c1ac;f3619ebbcdf98194ea438f4ce1c77a78;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-331.5547,148.5984;Inherit;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;0;False;0;False;-1;2a0c0f10cdff42c4eaf7e2215d0d6974;54f7b1afbce36934ca387841f17bbf6a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;31;64.98474,-131.8092;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-12.55472,62.59843;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;311.5454,-84.13776;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;853.4183,-66.73026;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Custom/Beam_Add;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Overlay;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;34;0
WireConnection;27;0;6;0
WireConnection;27;1;28;0
WireConnection;23;0;27;0
WireConnection;23;1;24;0
WireConnection;25;0;26;0
WireConnection;25;1;23;0
WireConnection;18;1;25;0
WireConnection;19;0;18;1
WireConnection;19;1;20;0
WireConnection;29;0;19;0
WireConnection;29;1;30;0
WireConnection;5;0;22;0
WireConnection;5;1;27;0
WireConnection;21;0;5;0
WireConnection;21;1;29;0
WireConnection;1;1;21;0
WireConnection;8;0;1;1
WireConnection;8;1;7;1
WireConnection;32;0;31;0
WireConnection;32;1;8;0
WireConnection;0;2;32;0
ASEEND*/
//CHKSM=5B01F8D2BC5564202D300372234CF2A397490628
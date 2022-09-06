// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Beam_Blended"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Bias("Bias", Float) = 0
		_Scale("Scale", Float) = 0
		_TextureSample5("Texture Sample 2", 2D) = "white" {}
		_Power("Power", Float) = 0
		_DistortionStrength2("DistortionStrength", Float) = 0.27
		_Vector7("Vector 0", Vector) = (3,1,0,0)
		_Float13("Float 0", Float) = 0.36
		_Vector6("Vector 1", Vector) = (3,1,0,0)
		_Float12("Float 1", Float) = 2
		_ScrollSpeed2("ScrollSpeed", Float) = 0.3
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Front
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _TextureSample0;
		uniform float2 _Vector7;
		uniform float _ScrollSpeed2;
		uniform sampler2D _TextureSample5;
		uniform float2 _Vector6;
		uniform float _Float12;
		uniform float _Float13;
		uniform float _DistortionStrength2;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float _Bias;
		uniform float _Scale;
		uniform float _Power;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float mulTime55 = _Time.y * -2.0;
			float temp_output_57_0 = ( mulTime55 * _ScrollSpeed2 );
			float2 temp_cast_0 = (temp_output_57_0).xx;
			float2 uv_TexCoord68 = i.uv_texcoord * _Vector7 + temp_cast_0;
			float2 temp_cast_1 = (( temp_output_57_0 * _Float12 )).xx;
			float2 uv_TexCoord61 = i.uv_texcoord * _Vector6 + temp_cast_1;
			float4 tex2DNode1 = tex2D( _TextureSample0, ( uv_TexCoord68 + ( ( tex2D( _TextureSample5, uv_TexCoord61 ).r * _Float13 ) * _DistortionStrength2 ) ) );
			o.Emission = ( i.vertexColor * tex2DNode1.r ).rgb;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV42 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode42 = ( _Bias + _Scale * pow( 1.0 - fresnelNdotV42, _Power ) );
			o.Alpha = ( tex2DNode1.a * ( tex2DNode1.r * tex2D( _TextureSample1, uv_TextureSample1 ).r ) * i.vertexColor.b * ( 1.0 - fresnelNode42 ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				surfIN.vertexColor = IN.color;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18933
-130;20;1906;1403;2776.726;365.0663;1.3;True;True
Node;AmplifyShaderEditor.RangedFloatNode;54;-2418.178,101.253;Inherit;False;Constant;_Float11;Float 2;10;0;Create;True;0;0;0;False;0;False;-2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;55;-2277.058,68.82484;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-2211.719,258.7305;Inherit;False;Property;_ScrollSpeed2;ScrollSpeed;11;0;Create;True;0;0;0;False;0;False;0.3;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-1924.451,375.8779;Inherit;False;Property;_Float12;Float 1;10;0;Create;True;0;0;0;False;0;False;2;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-1938.719,88.73048;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;60;-1589.351,150.2779;Inherit;False;Property;_Vector6;Vector 1;9;0;Create;True;0;0;0;False;0;False;3,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-1651.551,279.1779;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;61;-1388.851,224.0779;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;63;-1021.54,331.6646;Inherit;False;Property;_Float13;Float 0;8;0;Create;True;0;0;0;False;0;False;0.36;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;62;-1122.54,118.6646;Inherit;True;Property;_TextureSample5;Texture Sample 2;4;0;Create;True;0;0;0;False;0;False;-1;None;59ea3fa69f7e1904a8a81ae0b50938ff;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-803.5399,211.6646;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;65;-1469.851,-221.2221;Inherit;True;Property;_Vector7;Vector 0;7;0;Create;True;0;0;0;False;0;False;3,1;2,0.25;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;64;-663.8881,417.8252;Inherit;False;Property;_DistortionStrength2;DistortionStrength;6;0;Create;True;0;0;0;False;0;False;0.27;0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-518.8882,247.8252;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;68;-1157.059,-50.17523;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;45;-111.7219,603.3057;Inherit;False;Property;_Bias;Bias;2;0;Create;True;0;0;0;False;0;False;0;-0.53;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;69;-430.54,-4.33538;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-108.7219,838.3057;Inherit;False;Property;_Power;Power;5;0;Create;True;0;0;0;False;0;False;0;0.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-143.7219,701.3057;Inherit;False;Property;_Scale;Scale;3;0;Create;True;0;0;0;False;0;False;0;0.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;52;-528.7894,547.4517;Inherit;False;0;38;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-60.47369,86.11031;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;4bef76abd9a1e674688951217036c1ac;4bef76abd9a1e674688951217036c1ac;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;38;-320.3478,371.9808;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;2a0c0f10cdff42c4eaf7e2215d0d6974;54f7b1afbce36934ca387841f17bbf6a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;42;69.33447,626.9949;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;31;64.98474,-131.8092;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;97.96265,365.6164;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;43;352.2814,571.6604;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;311.5454,-84.13776;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;596.8137,295.4828;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;853.4183,-66.73026;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Custom/Beam_Blended;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Front;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;55;0;54;0
WireConnection;57;0;55;0
WireConnection;57;1;56;0
WireConnection;59;0;57;0
WireConnection;59;1;58;0
WireConnection;61;0;60;0
WireConnection;61;1;59;0
WireConnection;62;1;61;0
WireConnection;66;0;62;1
WireConnection;66;1;63;0
WireConnection;67;0;66;0
WireConnection;67;1;64;0
WireConnection;68;0;65;0
WireConnection;68;1;57;0
WireConnection;69;0;68;0
WireConnection;69;1;67;0
WireConnection;1;1;69;0
WireConnection;38;1;52;0
WireConnection;42;1;45;0
WireConnection;42;2;46;0
WireConnection;42;3;47;0
WireConnection;39;0;1;1
WireConnection;39;1;38;1
WireConnection;43;0;42;0
WireConnection;32;0;31;0
WireConnection;32;1;1;1
WireConnection;41;0;1;4
WireConnection;41;1;39;0
WireConnection;41;2;31;3
WireConnection;41;3;43;0
WireConnection;0;2;32;0
WireConnection;0;9;41;0
ASEEND*/
//CHKSM=E31B5ACA74AE2013EBA380986A0C001D7D100E63
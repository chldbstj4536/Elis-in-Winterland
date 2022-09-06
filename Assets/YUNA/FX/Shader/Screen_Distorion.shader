// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Screen_Distorion"
{
	Properties
	{
		_NoiseTex("NoiseTex", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_DistortAmount("DistortAmount", Range( 0 , 1)) = 0.82
		_PannerSpeed("PannerSpeed", Vector) = (0,0.3,0,0)
		_Vector0("Vector 0", Vector) = (0,0.3,0,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Tiling("Tiling", Vector) = (1,1,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
			float4 vertexColor : COLOR;
		};

		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform sampler2D _NoiseTex;
		uniform float2 _PannerSpeed;
		uniform float _DistortAmount;
		uniform sampler2D _TextureSample1;
		uniform float2 _Vector0;
		uniform float2 _Tiling;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 panner6 = ( 1.0 * _Time.y * _PannerSpeed + i.uv_texcoord);
			float2 uv_TexCoord2 = i.uv_texcoord * _Tiling;
			float2 panner5 = ( 1.0 * _Time.y * _Vector0 + uv_TexCoord2);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor18 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( ( (-1.0 + (tex2D( _NoiseTex, panner6 ).r - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) * (0.0 + (_DistortAmount - 0.0) * (0.1 - 0.0) / (1.0 - 0.0)) * (-1.0 + (tex2D( _TextureSample1, panner5 ).r - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ) + ase_grabScreenPosNorm ).xy);
			o.Emission = screenColor18.rgb;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			o.Alpha = ( i.vertexColor.a * tex2D( _TextureSample0, uv_TextureSample0 ).r );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18933
7;12;1906;999;2390.953;-52.96027;1;True;False
Node;AmplifyShaderEditor.Vector2Node;19;-2449.935,-124.2867;Inherit;False;Property;_Tiling;Tiling;6;0;Create;True;0;0;0;False;0;False;1,1;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;3;-2125.606,495.0564;Inherit;False;Property;_PannerSpeed;PannerSpeed;3;0;Create;True;0;0;0;False;0;False;0,0.3;0,0.01;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-2218.934,-168.9045;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-2129.506,276.6568;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;4;-2215.034,49.49503;Inherit;False;Property;_Vector0;Vector 0;4;0;Create;True;0;0;0;False;0;False;0,0.3;0.01,-0.01;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;6;-1776.749,370.5083;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;5;-1866.177,-75.05301;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;8;-1537.556,289.5232;Inherit;True;Property;_NoiseTex;NoiseTex;0;0;Create;True;0;0;0;False;0;False;-1;None;d9e3398f9bb66c84283b7d52da38f340;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-1626.984,-156.0381;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;c9d39bfc82af052429b1fa1e8bb7649d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-1613.839,586.5454;Inherit;False;Property;_DistortAmount;DistortAmount;2;0;Create;True;0;0;0;False;0;False;0.82;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;10;-1211.95,319.1907;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;20;-1216.412,619.6758;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;9;-1301.377,-126.3706;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-849.5237,444.2948;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;12;-1020.597,907.9069;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;16;-728.6769,-263.2903;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;15;-864.2394,-73.98361;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;0;False;0;False;-1;146c60b5214585b4ab14df39a41ca579;146c60b5214585b4ab14df39a41ca579;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-684.4419,730.2308;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ScreenColorNode;18;-407.9708,547.3121;Inherit;False;Global;_GrabScreen0;Grab Screen 0;3;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-506.8875,-80.88913;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;46.29086,39.67787;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Custom/Screen_Distorion;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;19;0
WireConnection;6;0;1;0
WireConnection;6;2;3;0
WireConnection;5;0;2;0
WireConnection;5;2;4;0
WireConnection;8;1;6;0
WireConnection;7;1;5;0
WireConnection;10;0;8;1
WireConnection;20;0;11;0
WireConnection;9;0;7;1
WireConnection;13;0;10;0
WireConnection;13;1;20;0
WireConnection;13;2;9;0
WireConnection;14;0;13;0
WireConnection;14;1;12;0
WireConnection;18;0;14;0
WireConnection;17;0;16;4
WireConnection;17;1;15;1
WireConnection;0;2;18;0
WireConnection;0;9;17;0
ASEEND*/
//CHKSM=520698F8DFC4E6B7F5A99B67EC3AADC72E04F8A1
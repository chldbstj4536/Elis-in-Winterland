// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Fresnel"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Bias("Bias", Float) = -0.17
		_Scale("Scale", Float) = 0.22
		_Power("Power", Float) = 0.39
		_PannerUV("PannerUV", Vector) = (0,0,0,0)
		_Emission("Emission", Float) = 0.48
		_PannerSpeed("PannerSpeed", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Overlay"  "Queue" = "Overlay+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		ZTest LEqual
		Blend One One , One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _MainTex;
		uniform float2 _PannerSpeed;
		uniform float2 _PannerUV;
		uniform float _Bias;
		uniform float _Scale;
		uniform float _Power;
		uniform float _Emission;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 panner11 = ( 1.0 * _Time.y * _PannerSpeed + ( i.uv_texcoord * _PannerUV ));
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV1 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode1 = ( _Bias + _Scale * pow( 1.0 - fresnelNdotV1, _Power ) );
			float clampResult20 = clamp( ( tex2D( _MainTex, panner11 ).r * fresnelNode1 ) , -1.0 , 0.3 );
			float4 temp_output_8_0 = ( i.vertexColor * clampResult20 );
			o.Emission = ( ( temp_output_8_0 * temp_output_8_0.a ) * _Emission ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18933
417;310;1314;1045;1417.271;173.1522;1;True;False
Node;AmplifyShaderEditor.Vector2Node;12;-2362.98,51.08171;Inherit;False;Property;_PannerUV;PannerUV;5;0;Create;True;0;0;0;False;0;False;0,0;0.3,0.3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-2438.399,-129.3173;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;17;-2030.604,149.2865;Inherit;False;Property;_PannerSpeed;PannerSpeed;7;0;Create;True;0;0;0;False;0;False;0,0;0.02,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-2180.727,-2.236748;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1469.2,472.1544;Inherit;False;Property;_Bias;Bias;2;0;Create;True;0;0;0;False;0;False;-0.17;-0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1445.2,634.1543;Inherit;False;Property;_Power;Power;4;0;Create;True;0;0;0;False;0;False;0.39;2.19;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1477.2,551.8544;Inherit;False;Property;_Scale;Scale;3;0;Create;True;0;0;0;False;0;False;0.22;2.87;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;11;-1833.376,25.26771;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FresnelNode;1;-1218.729,336.7356;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-1549.267,14.54562;Inherit;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;0;False;0;False;-1;407f21a347441e04b89588358213ac17;433264f9aa12fe640a898d1331e06835;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-955.3279,272.9739;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;5;-1005.595,-141.4106;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;20;-741.6912,204.3158;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-568.729,28.09339;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;9;-430.8016,179.9464;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;14;-227.171,274.0667;Inherit;False;Property;_Emission;Emission;6;0;Create;True;0;0;0;False;0;False;0.48;4.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-221.9993,88.21243;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-22.17102,179.0667;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;270,36;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Custom/Fresnel;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;2;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Overlay;;Overlay;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;0;13;0
WireConnection;16;1;12;0
WireConnection;11;0;16;0
WireConnection;11;2;17;0
WireConnection;1;1;4;0
WireConnection;1;2;3;0
WireConnection;1;3;2;0
WireConnection;6;1;11;0
WireConnection;7;0;6;1
WireConnection;7;1;1;0
WireConnection;20;0;7;0
WireConnection;8;0;5;0
WireConnection;8;1;20;0
WireConnection;9;0;8;0
WireConnection;10;0;8;0
WireConnection;10;1;9;3
WireConnection;15;0;10;0
WireConnection;15;1;14;0
WireConnection;0;2;15;0
ASEEND*/
//CHKSM=BD0D15F96B92FCFDF9EE57CA548B1A0EBF865E1C
// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/Ice"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_opacity("opacity", Float) = 1.33
		_FresnelPower("FresnelPower", Float) = 2.36
		_Brightness("Brightness", Float) = 0.64
		_Color0("Color 0", Color) = (0.5613208,1,0.9455767,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _Color0;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _FresnelPower;
		uniform float _Brightness;
		uniform float _opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV4 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode4 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV4, _FresnelPower ) );
			o.Emission = ( _Color0 * ( ( tex2D( _TextureSample0, uv_TextureSample0 ) + fresnelNode4 ) * _Brightness ) ).rgb;
			o.Alpha = _opacity;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18933
7;54;1906;998;1157.983;144.0885;1.209587;True;False
Node;AmplifyShaderEditor.RangedFloatNode;5;-636.3608,618.4327;Inherit;True;Property;_FresnelPower;FresnelPower;2;0;Create;True;0;0;0;False;0;False;2.36;1.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-483.5277,223.5286;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;03c5e6a8f0eb15e4187b83b23b5e854e;03c5e6a8f0eb15e4187b83b23b5e854e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;4;-378.7982,514.1766;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;-53.35867,271.5531;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-64.7098,493.2306;Inherit;False;Property;_Brightness;Brightness;3;0;Create;True;0;0;0;False;0;False;0.64;0.61;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;134.7693,426.5454;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;11;159.5543,216.4573;Inherit;False;Property;_Color0;Color 0;4;0;Create;True;0;0;0;False;0;False;0.5613208,1,0.9455767,0;0.6666667,0.8859591,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;406.4979,345.9953;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;12;647.1019,381.1083;Inherit;False;Property;_opacity;opacity;1;0;Create;True;0;0;0;False;0;False;1.33;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;2;866.0686,100.9966;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Unlit/Ice;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;3;5;0
WireConnection;6;0;3;0
WireConnection;6;1;4;0
WireConnection;7;0;6;0
WireConnection;7;1;8;0
WireConnection;9;0;11;0
WireConnection;9;1;7;0
WireConnection;2;2;9;0
WireConnection;2;9;12;0
ASEEND*/
//CHKSM=BF4E33354BA7FEF437C577AEECA8D783A8ED240B
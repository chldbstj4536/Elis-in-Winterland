// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Screen_Ice"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Ice("Ice", Range( 0 , 1)) = 0.7047552
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Overlay+0" "IsEmissive" = "true"  }
		Cull Off
		Blend One One , One One
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Ice;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode1 = tex2D( _TextureSample0, uv_TextureSample0 );
			float clampResult19 = clamp( (0.0 + (_Ice - 0.0) * (1.0 - 0.0) / (0.3 - 0.0)) , 0.0 , 0.3 );
			float lerpResult16 = lerp( 0.0 , tex2DNode1.r , clampResult19);
			float clampResult13 = clamp( (0.0 + (_Ice - 0.25) * (1.0 - 0.0) / (0.7 - 0.25)) , 0.0 , 0.7 );
			float lerpResult3 = lerp( 0.0 , tex2DNode1.g , clampResult13);
			float clampResult21 = clamp( (0.0 + (_Ice - 0.65) * (1.0 - 0.0) / (1.0 - 0.65)) , 0.0 , 1.0 );
			float lerpResult5 = lerp( 0.0 , tex2DNode1.b , clampResult21);
			o.Emission = ( i.vertexColor * ( lerpResult16 + lerpResult3 + lerpResult5 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18933
0;0;1920;1011;1493.511;287.5438;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;4;-1615.8,351.097;Inherit;True;Property;_Ice;Ice;2;0;Create;True;0;0;0;False;0;False;0.7047552;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;18;-1202.764,190.6105;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.3;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;12;-1051.969,403.1136;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.25;False;2;FLOAT;0.7;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;20;-1029.735,656.0106;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.65;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1268.699,-232.8131;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;6b544d3fbef240a4a97eaf87d0945379;6b544d3fbef240a4a97eaf87d0945379;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;19;-930.2573,74.80008;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;13;-810.8167,338.7181;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.7;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;21;-769.607,738.5137;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;3;-541.6323,289.7305;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;16;-520.8693,22.97696;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;5;-497.1252,733.2275;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-262.8425,363.9196;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;22;-213.5109,64.45624;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;28.48914,246.4562;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;244.073,231.051;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Custom/Screen_Ice;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Opaque;;Overlay;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;18;0;4;0
WireConnection;12;0;4;0
WireConnection;20;0;4;0
WireConnection;19;0;18;0
WireConnection;13;0;12;0
WireConnection;21;0;20;0
WireConnection;3;1;1;2
WireConnection;3;2;13;0
WireConnection;16;1;1;1
WireConnection;16;2;19;0
WireConnection;5;1;1;3
WireConnection;5;2;21;0
WireConnection;11;0;16;0
WireConnection;11;1;3;0
WireConnection;11;2;5;0
WireConnection;23;0;22;0
WireConnection;23;1;11;0
WireConnection;0;2;23;0
ASEEND*/
//CHKSM=C207896B3C4AAB727785CC61142317D3202D74A0
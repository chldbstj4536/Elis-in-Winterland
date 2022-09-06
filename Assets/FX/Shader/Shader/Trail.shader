// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/Trial"
{
	Properties
	{
		_Noise("Noise", 2D) = "white" {}
		_Trail("Trail", 2D) = "white" {}
		_Float0("Float 0", Range( 0 , 0.1)) = 0.05889925
		_Mask("Mask", 2D) = "white" {}
		_Color0("Color 0", Color) = (0,1,0.8040824,0)
		_Color1("Color 1", Color) = (1,1,1,0)
		_Float1("Float 1", Float) = 1
		_Vector0("Vector 0", Vector) = (1.86,0,0,0)
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
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Trail;
		uniform float _Float1;
		uniform float _Float0;
		uniform sampler2D _Noise;
		uniform float2 _Vector0;
		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;
		uniform float4 _Color0;
		uniform float4 _Color1;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float mulTime15 = _Time.y * _Float1;
			float2 panner5 = ( 1.0 * _Time.y * _Vector0 + i.uv_texcoord);
			float temp_output_7_0 = ( _Float0 * (0.0 + (tex2D( _Noise, panner5 ).r - 0.0) * (1.0 - 0.0) / (1.0 - 0.0)) );
			float2 panner14 = ( mulTime15 * float2( 0.5,0 ) + ( i.uv_texcoord + temp_output_7_0 ));
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float4 lerpResult20 = lerp( _Color0 , _Color1 , ( i.uv_texcoord.x + 0.0 ));
			o.Emission = ( ( tex2D( _Trail, panner14 ).r * tex2D( _Mask, uv_Mask ).r ) * lerpResult20 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
59;209;952;810;913.4001;66.64601;2.04141;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1841.065,-195.9259;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;26;-1794.746,-57.43787;Inherit;False;Property;_Vector0;Vector 0;8;0;Create;True;0;0;False;0;False;1.86,0;3.07,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;5;-1575.89,-114.3843;Inherit;False;3;0;FLOAT2;0.5,0;False;2;FLOAT2;2,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;4;-1346.983,-138.0749;Inherit;True;Property;_Noise;Noise;1;0;Create;True;0;0;False;0;False;-1;47ae85ae30923d04ba40b3a619be1e81;47ae85ae30923d04ba40b3a619be1e81;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-1012.255,-334.3242;Inherit;False;Property;_Float0;Float 0;3;0;Create;True;0;0;False;0;False;0.05889925;0.042;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;6;-1004.961,-66.28107;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-713.2135,-206.6847;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1443.755,586.83;Inherit;False;Property;_Float1;Float 1;7;0;Create;True;0;0;False;0;False;1;3.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-1256.394,322.0606;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;15;-1215.055,566.859;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-956.6359,339.028;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;24;-805.4589,1013.461;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;14;-973.6661,519.0471;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.5,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;18;-765.7703,572.3061;Inherit;True;Property;_Mask;Mask;4;0;Create;True;0;0;False;0;False;-1;8c9b4c2c70160f44f9a3c37021b5b162;8c9b4c2c70160f44f9a3c37021b5b162;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-536.7969,995.1434;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-730.7437,310.8273;Inherit;True;Property;_Trail;Trail;2;0;Create;True;0;0;False;0;False;-1;f844648db6b374f49bdbb2ac490b0149;f844648db6b374f49bdbb2ac490b0149;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;21;-535.2704,786.0145;Inherit;False;Property;_Color0;Color 0;5;0;Create;True;0;0;False;0;False;0,1,0.8040824,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;22;-329.1945,1036.359;Inherit;False;Property;_Color1;Color 1;6;0;Create;True;0;0;False;0;False;1,1,1,0;0,0.947295,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-324.7196,496.903;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;20;-272.7144,792.1205;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-969.3392,-656.0327;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-56.45616,592.7797;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-472.4579,-571.0964;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;2;187.3509,535.2884;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Unlit/Trial;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;2;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Overlay;;Overlay;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;3;0
WireConnection;5;2;26;0
WireConnection;4;1;5;0
WireConnection;6;0;4;1
WireConnection;7;0;8;0
WireConnection;7;1;6;0
WireConnection;15;0;16;0
WireConnection;11;0;12;0
WireConnection;11;1;7;0
WireConnection;14;0;11;0
WireConnection;14;1;15;0
WireConnection;23;0;24;1
WireConnection;13;1;14;0
WireConnection;17;0;13;1
WireConnection;17;1;18;1
WireConnection;20;0;21;0
WireConnection;20;1;22;0
WireConnection;20;2;23;0
WireConnection;19;0;17;0
WireConnection;19;1;20;0
WireConnection;9;0;10;0
WireConnection;9;1;7;0
WireConnection;2;2;19;0
ASEEND*/
//CHKSM=ED8B88853D115EC729873D3CE8C8BB7C3BC6B10A
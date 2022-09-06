// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Fx_nOISE"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_DistortTex("DistortTex", 2D) = "white" {}
		_MainTexPanner("MainTexPanner", Vector) = (0,0,0,0)
		_DistortAmount("DistortAmount", Range( 0 , 0.1)) = 0.04304536
		_DistortPannerXY("DistortPanner X/Y", Vector) = (0.25,0,0,0)
		_NoiseTex("NoiseTex", 2D) = "white" {}
		_tint("tint", Color) = (0.3679245,0.3679245,0.3679245,0)
		_DistortPanner("DistortPanner", Vector) = (0.35,0,0,0)
		_MaskTex("MaskTex", 2D) = "white" {}
		_custom1("custom1", Vector) = (0,0,0,0)
		_Color0("Color 0", Color) = (1,0,0,0)
		_Color1("Color 1", Color) = (0.3410021,0.8268145,0.9150943,0)
		_Float0("Float 0", Range( 0 , 1)) = 1
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Overlay"  "Queue" = "Overlay+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
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
			float4 uv_texcoord;
			float4 uv2_texcoord2;
		};

		uniform float4 _Color0;
		uniform float4 _Color1;
		uniform float _Float0;
		uniform sampler2D _MainTex;
		uniform float2 _MainTexPanner;
		uniform float _DistortAmount;
		uniform sampler2D _DistortTex;
		uniform float2 _DistortPannerXY;
		uniform float4 _tint;
		uniform float2 _custom1;
		uniform sampler2D _NoiseTex;
		uniform float2 _DistortPanner;
		uniform sampler2D _MaskTex;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 lerpResult99 = lerp( _Color0 , _Color1 , _Float0);
			float2 panner78 = ( 1.0 * _Time.y * _MainTexPanner + i.uv_texcoord.xy);
			float2 panner70 = ( 1.0 * _Time.y * _DistortPannerXY + i.uv_texcoord.xy);
			float temp_output_85_0 = ( i.uv_texcoord.w + _custom1.y );
			float2 panner82 = ( 1.0 * _Time.y * _DistortPanner + i.uv_texcoord.xy);
			float4 tex2DNode83 = tex2D( _NoiseTex, panner82 );
			float smoothstepResult95 = smoothstep( temp_output_85_0 , 1.0 , tex2DNode83.r);
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float smoothstepResult111 = smoothstep( ( i.uv2_texcoord2.z + 0 ) , 1.0 , ( ( tex2D( _TextureSample0, uv_TextureSample0 ).r * i.uv_texcoord.xy.x ) - ( 1.0 - i.uv_texcoord.xy.x ) ));
			o.Emission = ( lerpResult99 * ( i.vertexColor * tex2D( _MainTex, ( panner78 + ( _DistortAmount * (-1.0 + (tex2D( _DistortTex, panner70 ).r - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ) ) ) * i.vertexColor.a * _tint * smoothstepResult95 * tex2D( _MaskTex, i.uv_texcoord.xy ).r * ( _custom1.x + i.uv_texcoord.z ) ) * ( smoothstepResult111 - 0.0 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18933
-364;1030;1906;1022;1766.486;-454.445;1.6;True;False
Node;AmplifyShaderEditor.Vector2Node;68;-2542.533,394.5213;Inherit;False;Property;_DistortPannerXY;DistortPanner X/Y;5;0;Create;True;0;0;0;False;0;False;0.25,0;0.3,-0.3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;69;-2576.532,259.5212;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;70;-2327.533,336.5213;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;71;-2075.177,299.9478;Inherit;True;Property;_DistortTex;DistortTex;2;0;Create;True;0;0;0;False;0;False;-1;None;d9e3398f9bb66c84283b7d52da38f340;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;75;-1761.313,328.0059;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;86;-467.0283,488.7926;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;115;-466.3821,243.96;Inherit;True;Property;_TextureSample0;Texture Sample 0;14;0;Create;True;0;0;0;False;0;False;-1;None;9e127858e69537d49bc9b816086b706d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;117;-279.9059,637.9243;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;76;-1814.302,-21.60561;Inherit;False;Property;_MainTexPanner;MainTexPanner;3;0;Create;True;0;0;0;False;0;False;0,0;-1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;77;-2311.519,871.9786;Inherit;False;Property;_DistortPanner;DistortPanner;8;0;Create;True;0;0;0;False;0;False;0.35,0;0.15,-0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;72;-2355.475,660.3461;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;74;-1889.3,161.0471;Inherit;False;Property;_DistortAmount;DistortAmount;4;0;Create;True;0;0;0;False;0;False;0.04304536;0.0248;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;73;-1885.802,-172.4056;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;80;-1653.535,938.8373;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-1558.499,294.0472;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;82;-1988.475,724.3461;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;81;-1592.074,804.7111;Inherit;False;Property;_custom1;custom1;10;0;Create;True;0;0;0;False;0;False;0,0;2.86,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;114;-159.6155,1385.406;Inherit;False;Constant;_Vector1;Vector 1;13;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;112;-200.5862,1184.789;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;-73.28931,365.818;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;121;-45.74316,611.0357;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;78;-1650.505,-130.8057;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;113;120.3669,1261.633;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;85;-1156.59,893.2454;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;87;-952.8443,788.3297;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;118;198.9941,689.5243;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;84;-1315.566,77.26341;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;83;-1719.615,628.22;Inherit;True;Property;_NoiseTex;NoiseTex;6;0;Create;True;0;0;0;False;0;False;-1;None;74220fa582b60b24586cb98c58235dce;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;96;-638.8896,-930.0861;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;91;-154.7932,-601.38;Inherit;False;Property;_Color0;Color 0;11;0;Create;True;0;0;0;False;0;False;1,0,0,0;0.8396226,0,0.2104349,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;98;-704.0775,651.0372;Inherit;True;Property;_MaskTex;MaskTex;9;0;Create;True;0;0;0;False;0;False;-1;None;e69c7bb96b854fe408e44d9a11ebeeef;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;97;-269.7932,-442.3799;Inherit;False;Property;_Color1;Color 1;12;0;Create;True;0;0;0;False;0;False;0.3410021,0.8268145,0.9150943,0;0.25,0.6768888,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;95;-1236.461,376.3323;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;111;569.4803,893.2627;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;92;-714.9077,240.8398;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-197.8828,-234.389;Inherit;False;Property;_Float0;Float 0;13;0;Create;True;0;0;0;False;0;False;1;0.387;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;94;-1057.09,50.01467;Inherit;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;0;False;0;False;-1;None;e9007bafa57ba7044ae7b55ea510ecff;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;93;-829.3693,-712.0371;Inherit;False;Property;_tint;tint;7;0;Create;True;0;0;0;False;0;False;0.3679245,0.3679245,0.3679245,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;101;-282.3254,-115.9357;Inherit;True;7;7;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;99;17.69805,-313.4173;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;100;526.9603,412.8965;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;104;-1028.246,961.7833;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.StepOpNode;105;-1030.63,366.3859;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;-673.0843,-330.306;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;438.3998,-37.06379;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;106;-1248.079,1024.769;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;807.4658,-6.341097;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Custom/Fx_nOISE;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;2;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Overlay;;Overlay;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;70;0;69;0
WireConnection;70;2;68;0
WireConnection;71;1;70;0
WireConnection;75;0;71;1
WireConnection;79;0;74;0
WireConnection;79;1;75;0
WireConnection;82;0;72;0
WireConnection;82;2;77;0
WireConnection;116;0;115;1
WireConnection;116;1;86;1
WireConnection;121;0;117;1
WireConnection;78;0;73;0
WireConnection;78;2;76;0
WireConnection;113;0;112;3
WireConnection;113;1;114;1
WireConnection;85;0;80;4
WireConnection;85;1;81;2
WireConnection;118;0;116;0
WireConnection;118;1;121;0
WireConnection;84;0;78;0
WireConnection;84;1;79;0
WireConnection;83;1;82;0
WireConnection;98;1;87;0
WireConnection;95;0;83;1
WireConnection;95;1;85;0
WireConnection;111;0;118;0
WireConnection;111;1;113;0
WireConnection;92;0;81;1
WireConnection;92;1;80;3
WireConnection;94;1;84;0
WireConnection;101;0;96;0
WireConnection;101;1;94;0
WireConnection;101;2;96;4
WireConnection;101;3;93;0
WireConnection;101;4;95;0
WireConnection;101;5;98;1
WireConnection;101;6;92;0
WireConnection;99;0;91;0
WireConnection;99;1;97;0
WireConnection;99;2;90;0
WireConnection;100;0;111;0
WireConnection;104;0;106;0
WireConnection;105;0;83;1
WireConnection;105;1;85;0
WireConnection;107;0;99;0
WireConnection;107;1;101;0
WireConnection;107;2;100;0
WireConnection;0;2;107;0
ASEEND*/
//CHKSM=433668ECAEA513537F6619B9F6D5486588CF1B86
// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Fx_Alter_CoreSmoke"
{
	Properties
	{
		_NoiseTex("NoiseTex", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_Float0("Float 0", Range( 0 , 0.1)) = 0.1
		_NoiseSpeed("NoiseSpeed", Vector) = (0,-0.57,0,0)
		_Float2("Float 2", Float) = 1.41
		_Float1("Float 1", Float) = -0.38
		_Gradient("Gradient", Range( 0 , 1)) = 0.2
		_Float4("Float 4", Range( 0 , 0.1)) = 0.07369869
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Overlay"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
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
			float4 vertexColor : COLOR;
		};

		uniform float _Float1;
		uniform float _Float2;
		uniform sampler2D _Mask;
		uniform sampler2D _NoiseTex;
		uniform float2 _NoiseSpeed;
		uniform float4 _NoiseTex_ST;
		uniform float _Float0;
		uniform float4 _Mask_ST;
		uniform float _Gradient;
		uniform float _Float4;


		struct Gradient
		{
			int type;
			int colorsLength;
			int alphasLength;
			float4 colors[8];
			float2 alphas[8];
		};


		Gradient NewGradient(int type, int colorsLength, int alphasLength, 
		float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
		float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
		{
			Gradient g;
			g.type = type;
			g.colorsLength = colorsLength;
			g.alphasLength = alphasLength;
			g.colors[ 0 ] = colors0;
			g.colors[ 1 ] = colors1;
			g.colors[ 2 ] = colors2;
			g.colors[ 3 ] = colors3;
			g.colors[ 4 ] = colors4;
			g.colors[ 5 ] = colors5;
			g.colors[ 6 ] = colors6;
			g.colors[ 7 ] = colors7;
			g.alphas[ 0 ] = alphas0;
			g.alphas[ 1 ] = alphas1;
			g.alphas[ 2 ] = alphas2;
			g.alphas[ 3 ] = alphas3;
			g.alphas[ 4 ] = alphas4;
			g.alphas[ 5 ] = alphas5;
			g.alphas[ 6 ] = alphas6;
			g.alphas[ 7 ] = alphas7;
			return g;
		}


		float4 SampleGradient( Gradient gradient, float time )
		{
			float3 color = gradient.colors[0].rgb;
			UNITY_UNROLL
			for (int c = 1; c < 8; c++)
			{
			float colorPos = saturate((time - gradient.colors[c-1].w) / (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1);
			color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
			}
			#ifndef UNITY_COLORSPACE_GAMMA
			color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
			#endif
			float alpha = gradient.alphas[0].x;
			UNITY_UNROLL
			for (int a = 1; a < 8; a++)
			{
			float alphaPos = saturate((time - gradient.alphas[a-1].y) / (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1);
			alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
			}
			return float4(color, alpha);
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			Gradient gradient31 = NewGradient( 0, 5, 2, float4( 0.6650944, 0.7020897, 1, 0 ), float4( 0, 0.2011557, 1, 0.06471352 ), float4( 0, 0.8857895, 0.9019608, 0.40589 ), float4( 0, 0.3771887, 0.4352941, 0.520592 ), float4( 0, 0, 0, 0.997055 ), 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float clampResult17 = clamp( ( ( 1.0 - i.uv_texcoord.y ) + _Float1 ) , 0.0 , 1.0 );
			float2 uv0_NoiseTex = i.uv_texcoord * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
			float2 panner5 = ( 1.0 * _Time.y * _NoiseSpeed + uv0_NoiseTex);
			float4 tex2DNode1 = tex2D( _NoiseTex, panner5 );
			float2 uv0_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float4 tex2DNode12 = tex2D( _Mask, ( ( (-1.0 + (tex2DNode1.r - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) * _Float0 ) + uv0_Mask ) );
			float temp_output_23_0 = ( ( ( clampResult17 * _Float2 ) * tex2DNode12.r ) + ( tex2DNode12.r * tex2DNode1.r ) );
			o.Emission = ( SampleGradient( gradient31, ( 1.0 - ( ( temp_output_23_0 + _Gradient ) * ( 1.0 - step( temp_output_23_0 , _Float4 ) ) ) ) ) * i.vertexColor.a * i.vertexColor ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
7;161;1906;814;1552.989;-400.7877;1;True;False
Node;AmplifyShaderEditor.Vector2Node;6;-3315.193,1226.958;Inherit;False;Property;_NoiseSpeed;NoiseSpeed;4;0;Create;True;0;0;False;0;False;0,-0.57;0,-0.57;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-3321.26,988.7813;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;5;-3058.193,1102.958;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-2634.326,1154.264;Inherit;True;Property;_NoiseTex;NoiseTex;1;0;Create;True;0;0;False;0;False;-1;None;f42ed942948e9174bbb0c71261dbbeae;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-3093.479,-252.9632;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-2810.922,18.96828;Inherit;False;Property;_Float1;Float 1;6;0;Create;True;0;0;False;0;False;-0.38;-0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-2889.39,881.4327;Inherit;True;Property;_Float0;Float 0;3;0;Create;True;0;0;False;0;False;0.1;0.062;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;8;-3043.358,650.1855;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;14;-2814.922,-239.0317;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-2701.785,361.8882;Inherit;False;0;12;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-2650.692,724.816;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;15;-2576.922,-120.0318;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-2338.252,585.9094;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;17;-2285.922,-28.03172;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-2123.665,257.4405;Inherit;False;Property;_Float2;Float 2;5;0;Create;True;0;0;False;0;False;1.41;8.68;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1942.184,141.3649;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;-2260.04,759.8779;Inherit;True;Property;_Mask;Mask;2;0;Create;True;0;0;False;0;False;-1;None;e1cbedc852a6080418e297b104592257;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1770.559,454.6681;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1876.501,942.2435;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1537.804,1084.589;Inherit;False;Property;_Float4;Float 4;8;0;Create;True;0;0;False;0;False;0.07369869;0.06896517;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-1641.404,796.68;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1403.623,769.6404;Inherit;False;Property;_Gradient;Gradient;7;0;Create;True;0;0;False;0;False;0.2;0.444;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;33;-1301.67,953.8647;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-1202.023,651.2406;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;35;-1095.043,952.598;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-948.8059,822.167;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;31;-914.4811,663.5959;Inherit;False;0;5;2;0.6650944,0.7020897,1,0;0,0.2011557,1,0.06471352;0,0.8857895,0.9019608,0.40589;0,0.3771887,0.4352941,0.520592;0,0,0,0.997055;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.OneMinusNode;30;-733.3125,932.5757;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;36;-353.8022,977.9202;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GradientSampleNode;29;-621.3125,685.5757;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-80.26596,848.4177;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-1028.988,294.1001;Inherit;False;Constant;_emission;emission;3;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;124.1098,751.9955;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Fx_Alter_CoreSmoke;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;2;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Overlay;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;3;0
WireConnection;5;2;6;0
WireConnection;1;1;5;0
WireConnection;8;0;1;1
WireConnection;14;0;13;2
WireConnection;22;0;8;0
WireConnection;22;1;10;0
WireConnection;15;0;14;0
WireConnection;15;1;16;0
WireConnection;11;0;22;0
WireConnection;11;1;7;0
WireConnection;17;0;15;0
WireConnection;18;0;17;0
WireConnection;18;1;20;0
WireConnection;12;1;11;0
WireConnection;19;0;18;0
WireConnection;19;1;12;1
WireConnection;21;0;12;1
WireConnection;21;1;1;1
WireConnection;23;0;19;0
WireConnection;23;1;21;0
WireConnection;33;0;23;0
WireConnection;33;1;34;0
WireConnection;27;0;23;0
WireConnection;27;1;28;0
WireConnection;35;0;33;0
WireConnection;26;0;27;0
WireConnection;26;1;35;0
WireConnection;30;0;26;0
WireConnection;29;0;31;0
WireConnection;29;1;30;0
WireConnection;37;0;29;0
WireConnection;37;1;36;4
WireConnection;37;2;36;0
WireConnection;0;2;37;0
ASEEND*/
//CHKSM=4EB2496C10408E1FF654DB961070EBE71C739932
// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Fx_Particle_Shader_add"
{
	Properties
	{
		_DistortTex("DistortTex", 2D) = "white" {}
		_DistortAmount("DistortAmount", Range( 0 , 0.1)) = 0.06543055
		_DistortPannerXY("DistortPanner X/Y", Vector) = (0.25,0,0,0)
		_TexturePanner("TexturePanner", Vector) = (0,0,0,0)
		_MainTex("MainTex", 2D) = "white" {}
		_NoiseTEx("NoiseTEx", 2D) = "white" {}
		_DistortPanner("DistortPanner", Vector) = (0.35,0,0,0)
		_MaskTex("MaskTex", 2D) = "white" {}
		_custom1("custom1", Vector) = (0.2,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Overlay+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
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
			float2 uv_texcoord;
			float4 uv_tex4coord;
		};

		uniform sampler2D _MainTex;
		uniform float2 _TexturePanner;
		uniform float _DistortAmount;
		uniform sampler2D _DistortTex;
		uniform float2 _DistortPannerXY;
		uniform float4 _DistortTex_ST;
		uniform float2 _custom1;
		uniform sampler2D _NoiseTEx;
		uniform float2 _DistortPanner;
		uniform float4 _NoiseTEx_ST;
		uniform sampler2D _MaskTex;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 panner9 = ( 1.0 * _Time.y * _TexturePanner + i.uv_texcoord);
			float2 uv0_DistortTex = i.uv_texcoord * _DistortTex_ST.xy + _DistortTex_ST.zw;
			float2 panner3 = ( 1.0 * _Time.y * _DistortPannerXY + uv0_DistortTex);
			float temp_output_22_0 = ( i.uv_tex4coord.w + _custom1.y );
			float2 uv0_NoiseTEx = i.uv_texcoord * _NoiseTEx_ST.xy + _NoiseTEx_ST.zw;
			float2 panner14 = ( 1.0 * _Time.y * _DistortPanner + uv0_NoiseTEx);
			float4 tex2DNode17 = tex2D( _NoiseTEx, panner14 );
			float smoothstepResult18 = smoothstep( temp_output_22_0 , 1.0 , tex2DNode17.r);
			float4 color29 = IsGammaSpace() ? float4(0.4245283,0.3984959,0.3984959,0) : float4(0.1507122,0.1318166,0.1318166,0);
			o.Emission = ( i.vertexColor * tex2D( _MainTex, ( panner9 + ( _DistortAmount * (-1.0 + (tex2D( _DistortTex, panner3 ).r - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ) ) ) * i.vertexColor.a * smoothstepResult18 * ( _custom1.x + i.uv_tex4coord.z ) * tex2D( _MaskTex, i.uv_texcoord ).r * color29 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
59;209;952;810;266.8171;281.6254;1;True;False
Node;AmplifyShaderEditor.Vector2Node;2;-1904.817,301.4387;Inherit;False;Property;_DistortPannerXY;DistortPanner X/Y;3;0;Create;True;0;0;False;0;False;0.25,0;0.61,-0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1929.3,136.8295;Inherit;False;0;4;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;3;-1664.817,164.4387;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;4;-1415.346,136.5859;Inherit;True;Property;_DistortTex;DistortTex;1;0;Create;True;0;0;False;0;False;-1;47ae85ae30923d04ba40b3a619be1e81;47ae85ae30923d04ba40b3a619be1e81;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1917.911,558.1828;Inherit;False;0;17;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;16;-1836.1,774.994;Inherit;False;Property;_DistortPanner;DistortPanner;7;0;Create;True;0;0;False;0;False;0.35,0;-0.5,-1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;11;-1009.781,-162.8201;Inherit;False;Property;_TexturePanner;TexturePanner;4;0;Create;True;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TFHCRemapNode;5;-1096.179,166.0319;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1116.868,-11.47498;Inherit;False;Property;_DistortAmount;DistortAmount;2;0;Create;True;0;0;False;0;False;0.06543055;0.035;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1075.075,-314.2608;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;9;-847.5437,-253.5132;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-1096.456,829.7585;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-839.254,131.5228;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;14;-1631.911,630.1828;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;23;-1063.45,705.8647;Inherit;False;Property;_custom1;custom1;9;0;Create;True;0;0;False;0;False;0.2,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-753.2802,825.4318;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-613.9904,-194.2085;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-424.435,834.617;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;-1422.589,602.8553;Inherit;True;Property;_NoiseTEx;NoiseTEx;6;0;Create;True;0;0;False;0;False;-1;f42ed942948e9174bbb0c71261dbbeae;745a835104673ce4b8d5edfebe206cc0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;26;-169.8159,718.6764;Inherit;True;Property;_MaskTex;MaskTex;8;0;Create;True;0;0;False;0;False;-1;cb75b00174766a843805258e7d449c22;7e73bcc0044126f4789afb72a392921e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;28;-234.7879,-673.8169;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-500.5132,493.6682;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;18;-1038.999,457.7856;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;-479.5234,-376.4594;Inherit;False;Constant;_Tint;Tint;4;0;Create;True;0;0;False;0;False;0.4245283,0.3984959,0.3984959,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;-315.2643,-220.6151;Inherit;True;Property;_MainTex;MainTex;5;0;Create;True;0;0;False;0;False;-1;d276dd224fea3054189af6b91423a154;c166ea0b54f50784f9bf36759aad7451;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;21;-783.5029,589.4836;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;144.3831,-37.97699;Inherit;True;7;7;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;31;491.0984,29.40708;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Fx_Particle_Shader_add;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;2;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Overlay;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;0
WireConnection;3;2;2;0
WireConnection;4;1;3;0
WireConnection;5;0;4;1
WireConnection;9;0;10;0
WireConnection;9;2;11;0
WireConnection;6;0;7;0
WireConnection;6;1;5;0
WireConnection;14;0;13;0
WireConnection;14;2;16;0
WireConnection;22;0;20;4
WireConnection;22;1;23;2
WireConnection;8;0;9;0
WireConnection;8;1;6;0
WireConnection;17;1;14;0
WireConnection;26;1;27;0
WireConnection;24;0;23;1
WireConnection;24;1;20;3
WireConnection;18;0;17;1
WireConnection;18;1;22;0
WireConnection;12;1;8;0
WireConnection;21;0;17;1
WireConnection;21;1;22;0
WireConnection;25;0;28;0
WireConnection;25;1;12;0
WireConnection;25;2;28;4
WireConnection;25;3;18;0
WireConnection;25;4;24;0
WireConnection;25;5;26;1
WireConnection;25;6;29;0
WireConnection;31;2;25;0
ASEEND*/
//CHKSM=E28D8D0B179B8419F3366D50C1C4EF71CEF0B66F
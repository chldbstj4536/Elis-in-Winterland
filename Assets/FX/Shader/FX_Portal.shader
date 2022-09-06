// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX_Portal"
{
	Properties
	{
		_Power("Power", Float) = 1.35
		_Twirl_Center("Twirl_Center", Vector) = (0.5,0.5,0,0)
		[HDR]_Color0("Color 0", Color) = (5.189172,0.5140218,5.119394,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_nosie_Scale("nosie_Scale", Float) = 3.95
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
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color0;
		uniform sampler2D _TextureSample0;
		uniform float _nosie_Scale;
		uniform float2 _Twirl_Center;
		uniform float _Power;


		float2 voronoihash3( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi3( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash3( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F1;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float time3 = 1.0;
			float2 center45_g1 = _Twirl_Center;
			float2 delta6_g1 = ( i.uv_texcoord - center45_g1 );
			float angle10_g1 = ( length( delta6_g1 ) * 7.0 );
			float x23_g1 = ( ( cos( angle10_g1 ) * delta6_g1.x ) - ( sin( angle10_g1 ) * delta6_g1.y ) );
			float2 break40_g1 = center45_g1;
			float2 temp_cast_0 = (( _Time.y * 0.5 )).xx;
			float2 break41_g1 = temp_cast_0;
			float y35_g1 = ( ( sin( angle10_g1 ) * delta6_g1.x ) + ( cos( angle10_g1 ) * delta6_g1.y ) );
			float2 appendResult44_g1 = (float2(( x23_g1 + break40_g1.x + break41_g1.x ) , ( break40_g1.y + break41_g1.y + y35_g1 )));
			float2 coords3 = appendResult44_g1 * _nosie_Scale;
			float2 id3 = 0;
			float2 uv3 = 0;
			float voroi3 = voronoi3( coords3, time3, id3, uv3, 0 );
			float4 temp_output_15_0 = ( ( _Color0 * tex2D( _TextureSample0, i.uv_texcoord ) ) * pow( voroi3 , _Power ) );
			o.Emission = temp_output_15_0.rgb;
			o.Alpha = temp_output_15_0.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
7;995;1434;1169;2137.637;301.261;1.731955;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;5;-2068.968,-46.92064;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1919.403,-243.2849;Inherit;False;Constant;_Twirl_Strength;Twirl_Strength;1;0;Create;True;0;0;False;0;False;7;7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-1888.795,-139.8657;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;7;-1887.403,-380.2847;Inherit;False;Property;_Twirl_Center;Twirl_Center;2;0;Create;True;0;0;False;0;False;0.5,0.5;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;2;-1715.057,-306.1449;Inherit;True;Twirl;-1;;1;90936742ac32db8449cd21ab6dd337c8;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT;0;False;4;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1567.345,-176.4131;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1564.345,-85.41303;Inherit;False;Property;_nosie_Scale;nosie_Scale;7;0;Create;True;0;0;False;0;False;3.95;5.93;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1874.27,-599.2114;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;3;-1413.211,-236.372;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SamplerNode;13;-1606.116,-561.3495;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;False;-1;94934e8eff542f442a37a1875649bf4d;ac3bcf758fdbeef419ca2f014de21313;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-1255.058,-123.5521;Inherit;False;Property;_Power;Power;1;0;Create;True;0;0;False;0;False;1.35;1.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;11;-1555.801,-810.6874;Inherit;False;Property;_Color0;Color 0;3;1;[HDR];Create;True;0;0;False;0;False;5.189172,0.5140218,5.119394,0;10.37834,1.032401,10.26967,0.8941177;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;6;-1104.287,-260.7891;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-1022.093,-490.9704;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;57;-1975.527,942.3239;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-1957.395,690.9031;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;55;-1715.788,697.8374;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;56;-1696.459,945.4788;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.25,0.25;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;63;-1444.036,440.8985;Inherit;True;Property;_Splash;Splash;12;0;Create;True;0;0;False;0;False;-1;811c8a206691dea47892d1bab8cd6b23;811c8a206691dea47892d1bab8cd6b23;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;43;-2328.761,2247.652;Inherit;True;Property;_TextureSample2;Texture Sample 2;8;0;Create;True;0;0;False;0;False;-1;None;f1c609a8a37159a4fb12a887dd5728f0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;54;-1448.946,918.7842;Inherit;True;Property;_noiseTex2;noiseTex2;10;0;Create;True;0;0;False;0;False;-1;None;50a4d7e96624ad4489b3758009dbed51;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-763.2834,-131.012;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;61;-875.3503,1051.81;Inherit;False;Property;_Color1;Color 1;11;1;[HDR];Create;True;0;0;False;0;False;1,0.6084906,0.9869148,1;16,0.4392157,15.56078,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-839.9862,591.9085;Inherit;True;5;5;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;52;-2759.907,2430.506;Inherit;False;Constant;_Vector1;Vector 1;8;0;Create;True;0;0;False;0;False;0,0.3;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;53;-1479.226,700.5522;Inherit;True;Property;_noiseTex1;noiseTex1;9;0;Create;True;0;0;False;0;False;-1;None;59ea3fa69f7e1904a8a81ae0b50938ff;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;62;-1362.695,204.1878;Inherit;True;Property;_TextureSample3;Texture Sample 3;6;0;Create;True;0;0;False;0;False;-1;94934e8eff542f442a37a1875649bf4d;1597de68e61fd094381732461b79b51e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;28;-2211.103,1952.371;Inherit;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;False;0;False;-1;94934e8eff542f442a37a1875649bf4d;1597de68e61fd094381732461b79b51e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;35;-2091.378,2606.184;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;30;-1720.652,2413.506;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;40;-1708.951,2678.036;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-597.7942,883.847;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;49;-2510.207,2293.207;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1949.893,2206.356;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;42;-1138.08,2542.155;Inherit;False;Global;_GrabScreen0;Grab Screen 0;7;0;Create;True;0;0;False;0;False;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-1388.971,2493.333;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-2851.647,1739.23;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;51;-2833.207,2252.207;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;38;-2311.068,2615.667;Inherit;False;Constant;_Vector0;Vector 0;7;0;Create;True;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;1;-145.88,91.61501;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;FX_Portal;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;2;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Overlay;;Overlay;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;5;0
WireConnection;2;2;7;0
WireConnection;2;3;9;0
WireConnection;2;4;4;0
WireConnection;3;0;2;0
WireConnection;3;1;17;0
WireConnection;3;2;16;0
WireConnection;13;1;14;0
WireConnection;6;0;3;0
WireConnection;6;1;10;0
WireConnection;12;0;11;0
WireConnection;12;1;13;0
WireConnection;55;0;58;0
WireConnection;56;0;57;0
WireConnection;43;1;49;0
WireConnection;54;1;56;0
WireConnection;15;0;12;0
WireConnection;15;1;6;0
WireConnection;59;0;53;0
WireConnection;59;1;53;1
WireConnection;59;2;54;1
WireConnection;59;3;63;1
WireConnection;59;4;15;0
WireConnection;53;1;55;0
WireConnection;28;1;23;0
WireConnection;35;0;38;2
WireConnection;30;0;31;0
WireConnection;30;1;35;0
WireConnection;60;0;59;0
WireConnection;60;1;61;0
WireConnection;60;2;15;0
WireConnection;49;0;51;0
WireConnection;49;2;52;0
WireConnection;31;0;28;0
WireConnection;31;1;43;0
WireConnection;42;0;39;0
WireConnection;39;0;30;0
WireConnection;39;1;40;0
WireConnection;1;2;15;0
WireConnection;1;9;15;0
ASEEND*/
//CHKSM=0F1DA155F16E45C75DDCEDA9D45DC0830752ABED
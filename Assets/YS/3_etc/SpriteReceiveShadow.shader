Shader "Sprite/ReceiveShadow"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "Queue"="AlphaTest" "RenderType"="Sprite" "AlphaDepth"="False" "CanUseSpriteAtlas"="True" "IgnoreProjector"="True" }

		Pass
		{
			Name "FORWARD"
			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM
				#pragma target 3.0
				#pragma multi_compile_fwdbase

				#pragma vertex vert
				#pragma fragment fragBase

				#include "Packages/com.esotericsoftware.spine.spine-unity/Runtime/spine-unity/Shaders/Sprite/CGIncludes/ShaderShared.cginc"
				#include "Packages/com.esotericsoftware.spine.spine-unity/Runtime/spine-unity/Shaders/Sprite/CGIncludes/SpriteLighting.cginc"
				#include "AutoLight.cginc"
			
				////////////////////////////////////////
				// Defines
				//
			
				////////////////////////////////////////
				// Vertex output struct
				//
			
					#define _VERTEX_LIGHTING_INDEX TEXCOORD3
					#define _LIGHT_COORD_INDEX_0 4
					#define _LIGHT_COORD_INDEX_1 5
			
				struct VertexOutput
				{
					float4 pos : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					LIGHTING_COORDS(_LIGHT_COORD_INDEX_0, _LIGHT_COORD_INDEX_1)
			
					UNITY_VERTEX_OUTPUT_STEREO
				};
			
				////////////////////////////////////////
				// Vertex program
				//
			
				VertexOutput vert(VertexInput v)
				{
					VertexOutput output;
			
					UNITY_SETUP_INSTANCE_ID(input);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
			
					output.pos = calculateLocalPos(v.vertex);
					output.color = calculateVertexColor(v.color);
					output.texcoord = calculateTextureCoord(v.texcoord);
			
					TRANSFER_VERTEX_TO_FRAGMENT(output)
					return output;
				}
			
				////////////////////////////////////////
				// Fragment programs
				//
				fixed4 fragBase(VertexOutput input) : SV_Target
				{
					fixed4 texureColor = calculateTexturePixel(input.texcoord);
					RETURN_UNLIT_IF_ADDITIVE_SLOT(texureColor, input.color) // shall be called before ALPHA_CLIP
					ALPHA_CLIP(texureColor, input.color)
					fixed3 diffuse = LIGHT_ATTENUATION(input);
					fixed4 pixel = calculateLitPixel(texureColor, 1, diffuse);
			
					return pixel;
				}
			ENDCG
		}
	}

	FallBack "Spine/Sprite/Unlit"
}

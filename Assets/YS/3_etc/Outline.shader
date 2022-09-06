Shader "Sprite/Outline"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_OutlineColor("Outline Color", Color) = (1,0,0,1)
		_OutlineSize("Outline Size", int) = 5
	}
	
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};
		
			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
			float _Outline;
			fixed4 _OutlineColor;
			int _OutlineSize;
			
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;

				return OUT;
			}
			
			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float4 _MainTex_TexelSize;
			fixed4 SampleSpriteTexture(float2 uv)
			{
				fixed4 color = tex2D(_MainTex, uv);
#if ETC1_EXTERNAL_ALPHA
				color.a = tex2D(_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA
				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
				// If outline is enabled and there is a pixel, try to draw an outline.
				if (c.a != 0)
				{
					float totalAlpha = 1.0;
					[unroll(16)]
					for (int i = 1; i < _OutlineSize + 1; i++)
					{
						float4 offset = float4(i * _MainTex_TexelSize.y, -i * _MainTex_TexelSize.y, -i * _MainTex_TexelSize.x, i * _MainTex_TexelSize.x);
						float4 uplr = IN.texcoord.yyxx;
						float4 a, b;
						uplr += offset;
						a = float4(uplr.x, 0.0f, 0.0f, uplr.w);
						b = float4(1.0f, uplr.y, uplr.z, 1.0f);
						totalAlpha *= step(a.x, b.x) * tex2D(_MainTex, float2(IN.texcoord.x, uplr.x)).a;
						totalAlpha *= step(a.y, b.y) * tex2D(_MainTex, float2(IN.texcoord.x, uplr.y)).a;
						totalAlpha *= step(a.z, b.z) * tex2D(_MainTex, float2(uplr.z, IN.texcoord.y)).a;
						totalAlpha *= step(a.w, b.w) * tex2D(_MainTex, float2(uplr.w, IN.texcoord.y)).a;
					}

					if (totalAlpha == 0)
						c.rgba = _OutlineColor;
				}
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
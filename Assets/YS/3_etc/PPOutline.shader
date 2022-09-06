Shader "PP/PreOutline"
{
	Properties
	{
		_MainTex("Screen Texture", 2D) = "white" {}
		_OutlineSize("Outline Size", int) = 5
	}
	
	SubShader
	{
		Cull Off
		Lighting Off
		ZWrite Off
		ZTest Always

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;
			};
		
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert(appdata i)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(i.vertex);
				o.uv = i.texcoord;

				return o;
			}
			
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			fixed4 _OutlineColor;
			int _OutlineSize;

			fixed4 frag(v2f o) : SV_Target
			{
				float radAngle = 3.14159265 / 8;
				fixed mainTex = tex2D(_MainTex, o.uv).a;
				
				if (mainTex != 0)
					return 1;

				[unroll(16)]
				for (int j = 1; j < _OutlineSize; ++j)
				{
					[unroll(16)]
					for (int i = 0; i < 16; ++i)
					{
						float2 offset = float2(sin(radAngle * i), cos(radAngle * i));
						offset *= j * _MainTex_TexelSize;
						offset += o.uv;

						mainTex += tex2D(_MainTex, offset).a;
					}
				}

				clamp(mainTex, 0, 1);

				return 1 - mainTex;
			}
		ENDCG
		}
	}
}
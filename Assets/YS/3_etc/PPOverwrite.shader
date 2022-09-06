Shader "PP/Outline"
{
	Properties
	{
		_MainTex("Screen Texture", 2D) = "white" {}
		_OWTex("Overwrite Texture", 2D) = "while" {}
		_OutlineColor("Outline Color", Color) = (1,0,0,1)
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
			sampler2D _OWTex;
			fixed4 _OutlineColor;

			fixed4 frag(v2f o) : SV_Target
			{
				fixed4 mainTex = tex2D(_MainTex, o.uv);
				fixed4 owTex = tex2D(_OWTex, o.uv);

				if (owTex.a == 1)
					return mainTex;

				return _OutlineColor;
			}
		ENDCG
		}
	}
}
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/StencilMask" {
	Properties {
		_Color ("Tint", Color) = (1,1,1,1)
		_MainTex ("Sprite texture", 2D) = "white" {}
	}
	SubShader {
		Tags {
			"RenderType"="Transparent"
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull off
		Lighting off
		ZWrite off
		Fog{ Mode off }
		Blend One OneMinusSrcAlpha

		Pass{
			Stencil{
				Ref 2
				Comp always
				Pass replace
			}
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float3 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord : TEXCOORD0;
			};

			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;

				return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
				c.rgb *= c.a;

				if(c.a < 0.1)
					discard;

				return c;
			}

			ENDCG
		}
	}
}

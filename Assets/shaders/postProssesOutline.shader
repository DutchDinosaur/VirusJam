Shader "LilyShaders/postProsses/Outline" {

    Properties {
        _MainTex ("Texture", 2D) = "white" {}
		_SceneTex("Scene Texture",2D)="black"{}
		_Color ("Color", Color) = (0,0,0,1)
		_Size( "Size", Float) = 1
		//_PixelDensity( "Pixel Density", Float) = 1
		//_AspectRatioMultiplier( "Aspect Ratio Multiplier", Float) = 1
    }

    SubShader {
        Tags { "RenderType"="Opaque" }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"


            struct appdata {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
			sampler2D _SceneTex;
			float2 _MainTex_TexelSize;
			float4 _Color;
			float _Size;
			float _PixelDensity;
			float2 _AspectRatioMultiplier;

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
				float TX_x = _MainTex_TexelSize.x * _Size;
                float TX_y = _MainTex_TexelSize.y * _Size;

				float2 pixelScaling = _PixelDensity * _AspectRatioMultiplier;
				float2 uv = round(i.uv * pixelScaling)/ pixelScaling;


                float col = saturate(
				tex2D(_MainTex, uv + float2(TX_x,-TX_x)).r +
				tex2D(_MainTex, uv + float2(-TX_x,-TX_x)).r +
				tex2D(_MainTex, uv + float2(TX_x,TX_x)).r +
				tex2D(_MainTex, uv + float2(-TX_x,TX_x))).r;
				col -= tex2D(_MainTex, uv).r;
                return lerp(tex2D(_SceneTex, uv), _Color,saturate(col));
            }
            ENDCG
        }
    }
}

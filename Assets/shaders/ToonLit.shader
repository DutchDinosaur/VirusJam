Shader "LilyShaders/ToonLit" {

    Properties {
		_Color ("Color", Color) = (0,0,0,1)
        _MainTex ("Texture", 2D) = "white" {}
		_ShadowColor ("Shadow Color", Color) = (0,0,0,1)
		//[Toggle(OUTLINE)] _Outline ("Outline", Float) = 0
    }

    SubShader {
        Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase" }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase

            struct appdata {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
				float4 normal : NORMAL;
            };

            struct v2f {
                half4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
				half3 normal : TEXCOORD1;
				LIGHTING_COORDS(0,2)
            };

            uniform sampler2D _MainTex;
			float4 _Color;

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = v.uv;
				o.normal = UnityObjectToWorldNormal(v.normal);
				TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

            float4 frag (v2f i) : SV_Target {
				half GlobalLight = SHADOW_ATTENUATION(i);
				float SelfLight = smoothstep(0,.01f, dot(_WorldSpaceLightPos0.xyz, i.normal));
				float Lighting = SelfLight * GlobalLight;

				float3 output = lerp(unity_AmbientSky,tex2D(_MainTex,i.uv),saturate(Lighting + .5f)) * _Color;
                return float4(output,0);
            }

            ENDCG
        }
    }
	Fallback "Diffuse"
}
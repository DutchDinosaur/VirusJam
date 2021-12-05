Shader "LilyShaders/Billboard" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
		_billboardSize( "billboard Size", Float) = 1
    }

    SubShader {
        Tags { "DisableBatching" = "True" }

        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag      
            #include "UnityCG.cginc"
 
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
 
            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _billboardSize;
           
            v2f vert (appdata v) {
                v2f o;
				o.vertex = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0)) + float4(v.vertex.x, v.vertex.y, 0.0, 0.0) * float4(_billboardSize, _billboardSize, 1.0, 1.0));
                o.uv = v.uv;
                return o;
            }
           
            fixed4 frag (v2f i) : SV_Target {
				half4 tex = tex2D(_MainTex, i.uv);
				clip(tex.a-.5f);
				return tex;
            }
            ENDCG
        }
    }
}
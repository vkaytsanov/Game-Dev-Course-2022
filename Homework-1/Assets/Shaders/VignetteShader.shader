Shader "Hidden/VignetteShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Radius;
            float _Smoothing;
            float4 _TintColor;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float distFromCenter = length(i.uv * 2.f - 1.f);
                float mask = smoothstep(_Radius, _Radius + _Smoothing, distFromCenter);
                float intensity = abs(_SinTime.w * mask);

                float3 vignetteColor = _TintColor * intensity;
                float3 mainTexColor = col.rgb * (1.f - intensity);
                float3 finalColor = vignetteColor + mainTexColor;

                return fixed4(finalColor, col.a);
                //return fixed4(intensity.xxx, col.a);
            }
            ENDCG
        }
    }
}

Shader "Custom/TowerOutline"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0,1,1,1)
        _OutlineWidth ("Outline Width", Range(0.001,0.05)) = 0.02
        _Glow ("Glow Strength", Range(0,5)) = 2
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }

        Pass
        {
            Name "Outline"
            Cull Front
            ZWrite Off
            ZTest Less

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _OutlineWidth;
            float4 _OutlineColor;
            float _Glow;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;

                float3 norm = normalize(v.normal);
                float3 offset = norm * _OutlineWidth;

                v.vertex.xyz += offset;

                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor * _Glow;
            }
            ENDCG
        }
    }
}
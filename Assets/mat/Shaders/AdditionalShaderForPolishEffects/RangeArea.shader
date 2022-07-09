// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/RangeArea"
{
	Properties
	{
		_Color("Color",Color) = (1,0,0,0)
		_Radius("Radius", Float) = 0
		_Limit("Limit", FLoat) = 0
		_Thickness("Thickness", Range(0.0,0.5)) = 0.05
        _Dropoff("Dropoff", Range(0.01, 4)) = 0.1
		_MainTex ("Texture", 2D) = "white" {}

	}
	SubShader
	{
		// No culling or depth
		ZWrite off
        Blend SrcAlpha OneMinusSrcAlpha // Alpha blending

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
			fixed4 _Color;
			float _Radius;
			float _Limit;
			float _Thickness;
			float _Dropoff;

         	float antialias(float r, float d, float t, float p) {
                if( d < (r - 0.85*t))
                   	return - pow( d - r + 0.5*t,2)/ pow(p*t, 2) + 1.0;
                else if ( d > (r + 0.85*t))
                     return - pow( d - r - 0.5*t,2)/ pow(p*t, 2) + 1.0; 
                 else
                    return 1.0;
            }

			fixed4 frag (v2f i) : SV_Target
			{
				float d = sqrt(pow(i.uv.x - 0.5, 2)+pow(i.uv.y - 0.5, 2));
				fixed4 col = tex2D(_MainTex, i.uv);

				float aa = (antialias(_Radius, d, _Thickness, _Dropoff));
				col = fixed4(col.r, col.g, col.b, col.a*aa);
				// just add the colors
				col *= _Color;

				// if radius >= d: col.a = 0.5 else: col.a = 0
				//col.a = step(d, _Radius) * _Color.a;

				// if col.a >= 0.5 && d >= limit
				//col.a = step(_Color.a,col.a)*step(_Limit, d)*col.a;
				col.r = col.r * col.a;
				col.g = col.g * col.a;
				return col;

			}
			ENDCG
		}
	}
}

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Polish"
{
	 	Properties 
	 	{
        	_Color ("Color", Color) = (1,0,0,0)
        	_OtherColor ("OtherColor", Color) = (1,0,0,0)
          	_Thickness("Thickness", Range(0.0,0.5)) = 0.05
          	_Radius("Radius", Range(0.0, 0.5)) = 0.4
          	_Dropoff("Dropoff", Range(0.01, 4)) = 0.1
        	_MainTex("Main Texture", 2D) = "white" {}
        	_PolishModifier("PolishModifier", Float) = 0
        	//_PI("PI",Range(3.1415926, 3.1415926)) = 3.1415926
      	}
    	SubShader 
      	{
        	Pass 
          	{
          		ZWrite off
            	Blend SrcAlpha OneMinusSrcAlpha // Alpha blending
            	LOD 100
            	Tags
            	{		
           			"Queue"="Transparent"
				}
            	CGPROGRAM
             
            	#pragma vertex vert
            	#pragma fragment frag
            	#include "UnityCG.cginc"

                // low precision type is usually enough for colors
                fixed4 _Color;
          		fixed4 _OtherColor;
             	sampler2D _MainTex;
             	float _PolishModifier;

             	float _Thickness;
             	float _Radius;
             	float _Dropoff;
             	
              	struct appdata {
              		float4 vertex : POSITION;
                	float2 uv : TEXCOORD0;
              	};

              	struct v2f
              	{
              		float2 uv : TEXCOORD0;
              		float4 vertex : SV_POSITION;
              	};

              	v2f vert(appdata v)
              	{
              		v2f o;
              		o.vertex = UnityObjectToClipPos(v.vertex);
              		o.uv = v.uv;//-fixed2(0.5,0.5);

              		return o;

              	}

         		float antialias(float r, float d, float t, float p) {
                	if( d < (r - 0.85*t))
                   		return - pow( d - r + 0.5*t,2)/ pow(p*t, 2) + 1.0;
                	else if ( d > (r + 0.85*t))
                     	return - pow( d - r - 0.5*t,2)/ pow(p*t, 2) + 1.0; 
                 	else
                    	return 1.0;
                }
              

              	fixed4 frag(v2f IN) : SV_Target
              	{
              		float distance = sqrt(pow(IN.uv.x-0.5, 2) + pow(IN.uv.y-0.5,2));
              		float _r = _Radius ;//+ sin(_Time[1]);
                 	fixed4 result = fixed4(_Color.r, _Color.g, _Color.b, _Color.a*antialias(_r, distance, _Thickness, _Dropoff));
                 	//fixed4 result2 = fixed4(_OtherColor.r, _OtherColor.g, _OtherColor.b, _OtherColor.a*antialias(_r, distance, _Thickness, _Dropoff));
                    fixed4 result2 = fixed4(_OtherColor.r, _OtherColor.g, _OtherColor.b, _OtherColor.a);
              		fixed4 col = result;
     
              		float theAlphaResult = result2.a;
              		col.r = col.r * theAlphaResult;
              		col.g = col.g * theAlphaResult;

              		return col;
              	}
              
              
             	ENDCG
         	}
    	}
}

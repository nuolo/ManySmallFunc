// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/NormalUnlitShader"
{
	   Properties
	{
	    _MainTex("Texture", 2D) = "white" {}
	    _Outline("Out Line", Range(0.001, 0.005)) = 0.002
	        _Color("Color", Color) = (1, 1, 1, 1)
	    }
	
	 CGINCLUDE
	 #include "UnityCG.cginc"
	 struct v2f
	 {
	     float4 pos:POSITION;
	     float4 color:COLOR;
	 };
	
	    sampler2D _MainTex;
	 float _Outline;
	 fixed4 _Color;
	
	    v2f vert(appdata_base v)
	    {
	        v2f o;
	        o.pos = UnityObjectToClipPos(v.vertex);
	        float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
	        float2 offset = TransformViewToProjection(norm.xy);
	        o.pos.xy += offset * o.pos.z * _Outline;
	        o.color = _Color;
	        return o;
	    }
	 ENDCG
	
	    SubShader
	    {
	        Cull Front
		   Pass
		   {
		       CGPROGRAM
		           #pragma vertex vert
		           #pragma fragment frag
		           fixed4 frag(v2f i) : COLOR
		           {
		               return i.color;
		           }
		       ENDCG
	  }
	
	      CGPROGRAM
	      #pragma surface surf Lambert
	      struct Input {
	          float2 uv_MainTex;
	
	};
	  void surf(Input IN, inout SurfaceOutput o) {
	          fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	          o.Albedo = c.rgb;
	          o.Alpha = c.a;
	
	}
	   ENDCG
	   }
	}

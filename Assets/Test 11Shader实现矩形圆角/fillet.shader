// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/fillet" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200
			 Blend SrcAlpha OneMinusSrcAlpha
			pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;

			struct Input {
					fixed4 vertex : POSITION;
					fixed2 texcoord : TEXCOORD0;
			};

			struct Output {
				fixed4 pos : POSITION;
				fixed2 uv : TEXCOORD0;
			};
			Output vert(Input i)
			{
				Output o;
				o.pos = UnityObjectToClipPos(i.vertex);
				o.uv = i.texcoord;
				return o;
			}

			fixed4 frag(Output o) :COLOR
			{
				fixed4 c = tex2D(_MainTex,o.uv);
				float2 uv = o.uv.xy - float2(0.5,0.5);//UV 坐标默认是(0 0)左下角,减0.5 是把UV的中心点移到(0 0) 这样纹理坐标的范围就是 （-0.5，-0.5)~(0.5,0.5)
				float rx = fmod(uv.x, 0.4); //fmod 求余函数,从原点开始 矩形的顶点坐标为0.4
				float ry = fmod(uv.y, 0.4);
				float mx = step(0.4, abs(uv.x)); // if 0.4>abs(uv.x)?0:1
				float my = step(0.4, abs(uv.y));
				float filletAlpha = 1 - mx * my * step(0.1, length(half2(rx,ry)));  //如果在圆角矩形内，alpha值为1 否则为0。 和0.1做比较 因为圆角半径为0.1 
				c = fixed4(c.r,c.g,c.b,filletAlpha);
				return c;
			}
			ENDCG
			}
	}
}
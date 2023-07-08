Shader "zclShader/whirShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Radius ("_Radius",Float) =1
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
			float _Radius;   //扭曲的弧度

			float2 whirl(float2 uv) {

				//坐标系从左下角变换到中心
				uv += float2(-0.5, -0.5);   

				//弧度值 X UV点到中心的距离 X 时间变化
				float tempRad=_Radius*_Time.y*length(uv); 

				//旋转矩阵
				float2x2 TransMatrix = {
					cos(tempRad),sin(tempRad),-sin(tempRad),cos(tempRad)
				};

				//对UV进行旋转变换
				float2 uv2 = mul(TransMatrix, uv);

				//映射回原来的坐标空间
				uv2 += float2(0.5, 0.5);
				
				return uv2;
			}

            fixed4 frag (v2f i) : SV_Target
            {			
                fixed4 col = tex2D(_MainTex, whirl(i.uv));
                return col;
            }
            ENDCG
        }
    }
}



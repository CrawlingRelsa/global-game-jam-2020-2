Shader "ShaderBuoyz/Outline Surface"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        [Toggle(ENABLE_OUTLINE)] _EnableOutline("Enable Outline" , float) = 0
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth ("Outline Width", Range(0, 200)) = 0.03
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        Stencil {
            Ref 1
            Comp Always
            Pass Replace
        }    

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;     

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG

        Pass {
            Cull Front
            ZTest Off
            ZWrite Off

            Stencil {
                Ref 1
                Comp NotEqual
                Pass Keep
            }


            Tags {
                "Queue" = "Transparent+1010"
                "RenderType" = "Transparent"
                "DisableBatching" = "True"
            }


            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature_local ENABLE_OUTLINE

            half _OutlineWidth;
            half4 _OutlineColor;

            struct VertexData
			{
			    float4 vertex : POSITION;
			    float2 uv : TEXCOORD0;
			    float3 normal : NORMAL;
                float4 tangent : TANGENT;
			};

            float4 vert(VertexData v) : SV_POSITION {
                //float3 vertex = v.vertex + v.normal * _OutlineWidth;
                
                // clip space position
                float4 posCS = UnityObjectToClipPos(v.vertex);

                #ifdef ENABLE_OUTLINE
                // clip normal, counting resize
                float3 normalCS = mul((float3x3)UNITY_MATRIX_VP, mul(v.normal, (float3x3)unity_WorldToObject));

                // extrude in clip space along the normal
                // premultiply w before perspective division
                float2 offset = normalize(normalCS.xy) / _ScreenParams.xy * _OutlineWidth * posCS.w * 2.0;

                posCS.xy += offset;
                #endif

                return posCS;
            }

            half4 frag() : SV_TARGET {
                //return _OutlineColor;

                //discard;

                return _OutlineColor;
            }

            ENDCG
        }           
    
    }
    FallBack "Diffuse"
}

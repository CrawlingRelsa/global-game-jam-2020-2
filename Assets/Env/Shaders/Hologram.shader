Shader "ShaderBuoyz/Hologram"
{
    Properties
    {
        _DisplaceTex ("Displace Tex", 2D) = "white" {}
        _DisplaceIntensity("Displace Intensity", Float) = 1.
        _MaskTex ("Mask Tex", 2D) = "white" {}
        _MaskSpeed("Mask Speed (X,Y)", Vector) = (0.5, 0.5, 0, 0)
        _HologramIntensity("hologram intensity", Float) = 1.
        _HologramSharpness("hologram sharpness", Range(0, 10)) = 1
        _HologramColor("Hologram Color", Color) = (0.1, 0.1, 1., 1.)
        _IntersectionThreshold("Highlight of intersection threshold", range(0,1)) = .1 //Max difference for intersections
        
        // Scanline
		_ScanTiling ("Scan Tiling", Range(0.01, 200.0)) = 1.0
		_ScanSpeed ("Scan Speed", Range(-2.0, 2.0)) = 1.0        

        _Brightness("Brightness", Range(0.1, 6.0)) = 3.0
		_Alpha ("Alpha", Range (0.0, 1.0)) = 1.0
		_Direction ("Direction", Vector) = (0,1,0,0)    
        _Blur ("Blur", Float) = 1

        // Glow
        [Toggle(GLOW_ON)]_GlowOn("Show Glow" , float) = 0
		_GlowTiling ("Glow Tiling", Range(0.01, 1.0)) = 0.05
		_GlowSpeed ("Glow Speed", Range(-10.0, 10.0)) = 1.0
        _GlowColor ("Glow Color", Color) = (1,1,1,1)

		// Glitch
        [Toggle(GLITCH_ON)]_GlitchOn("Show Glitch" , float) = 0
		_GlitchSpeed ("Glitch Speed", Range(0, 50)) = 1.0
		_GlitchIntensity ("Glitch Intensity", Float) = 0

		// Alpha Flicker
		_FlickerTex ("Flicker Control Texture", 2D) = "white" {}
		_FlickerSpeed ("Flicker Speed", Range(0.01, 100)) = 1.0

    }
    SubShader
    {
        // Draw ourselves after all opaque geometry
        Tags { "Queue" = "Transparent" }

        // Grab the screen behind the object into _BackgroundTexture
        GrabPass
        {
            "_BackgroundTexture"
        }

        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #pragma shader_feature_local GLOW_ON
            #pragma shader_feature_local GLITCH_ON


            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {                
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 screenPos : TEXCOORD2;
                float4 vertex : SV_POSITION;
                float3 normalWS : TEXCOORD3;
                float3 posWS : TEXCOORD4;
            };

            uniform sampler2D 
                _MaskTex, 
                _DisplaceTex,
                _CameraDepthTexture;
            uniform sampler2D _BackgroundTexture;
            uniform float4 _MaskTex_ST, _DisplaceTex_ST, _BackgroundTexture_TexelSize;
            uniform float2 _MaskSpeed;
            uniform float3 _Direction;
            uniform float 
                _HologramIntensity, 
                _HologramSharpness, 
                _IntersectionThreshold, 
                _DisplaceIntensity,
                _ScanTiling,
                _ScanSpeed,
                _GlowTiling,
                _GlowSpeed,
                _GlitchSpeed,
                _FlickerTex,
                _FlickerSpeed,
                _GlitchIntensity,
                _Brightness,
                _Blur;
            uniform fixed4 _HologramColor, _GlowColor;

            float3 overlay(float3 base, float3 blend, float alpha) {
                float r = base.r < 0.5 ? 2.0 * base.r * blend.r : (1.0 - 2.0 * (1.0 - base.r) * (1.0 - blend.r));
                float g = base.g < 0.5 ? 2.0 * base.g * blend.g : (1.0 - 2.0 * (1.0 - base.g) * (1.0 - blend.g));
                float b = base.b < 0.5 ? 2.0 * base.b * blend.b : (1.0 - 2.0 * (1.0 - base.b) * (1.0 - blend.b));
                return float3(r,g,b);
            }

            float3 overlay2(float3 base, float3 blend) {
                return base < 0.5 ? 2.*base*blend : 1.-2*(1-blend)*(1-base);                
            }

            float ramp(float y, float start, float end) {
                float inside = step(start,y) - step(end,y);
                float fact = (y-start)/(end-start)*inside;
                return (1.-fact) * inside;
            }            

            float stripes(float2 uv, float start, float end) {
                return lerp(start, end, fmod(uv.y, 1.));
            }

            float stripes2(float2 uv, float start, float end) {
                return ramp(fmod(uv.y, 1.), start, end);
            }            

            v2f vert (appdata v)
            {

				// Glitches
				#if GLITCH_ON
					v.vertex.x += _GlitchIntensity * (step(0.5, sin(_Time.y * 2.0 + v.vertex.y * 1.0)) * step(0.99, sin(_Time.y*_GlitchSpeed * 0.5)));
				#endif

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MaskTex);

                o.screenPos = ComputeGrabScreenPos(o.vertex);
                

                o.posWS = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normalWS = normalize(UnityObjectToWorldNormal(v.normal));
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i,fixed face : VFACE) : SV_Target
            {
                //intersection
				//fixed intersect = saturate((abs(LinearEyeDepth(tex2Dproj(_CameraDepthTexture,i.screenPos).r) - i.screenPos.z)) / _IntersectionThreshold);

                float time = _Time.y;

                float3 eye = normalize(_WorldSpaceCameraPos - i.posWS);
                float3 normal = normalize(i.normalWS);
                float rim = (1. - saturate(dot(eye, normal)));

                // sample the texture
                fixed mask = tex2D(_MaskTex, i.uv + _MaskSpeed).r;
                fixed2 displace = tex2D(_DisplaceTex, i.uv).rg;

                half dirVertex = (dot(i.posWS, normalize(float4(_Direction.xyz, 1.0))) + 1) / 2;

				// Scanlines
				float scan = 0.0;
				//#ifdef _SCAN_ON
					scan = step(frac(dirVertex * _ScanTiling + _Time.w * _ScanSpeed), 0.5) * 0.65;
				//#endif

// Glow
				float glow = 0.0;
				#ifdef GLOW_ON
					glow = frac(dirVertex * _GlowTiling - _Time.x * _GlowSpeed);
				#endif                

                //i.screenPos.xy += (displace * 2 - 1) * _DisplaceIntensity;
                half4 top = tex2Dproj(_BackgroundTexture, i.screenPos + float4(0, -_Blur, 0, 0));
                half4 bot = tex2Dproj(_BackgroundTexture, i.screenPos + float4(0, _Blur, 0, 0));
                half4 left = tex2Dproj(_BackgroundTexture, i.screenPos + float4(-_Blur, 0, 0, 0));
                half4 right = tex2Dproj(_BackgroundTexture, i.screenPos + float4(_Blur,0, 0, 0));
                half4 center = tex2Dproj(_BackgroundTexture, i.screenPos );         
                fixed3 distortColor = (top + bot + left + right + center + center) / 6.0f;
                        
                //rim *= intersect * clamp(0,1,face) * _HologramColor;
                rim = pow(rim, _HologramSharpness) * _HologramIntensity * mask;
				fixed3 col = rim * _HologramColor;

                fixed stripeInt = stripes2(i.uv*_Direction + time, 0.3, 0.4);
                //col += abs(stripeInt) * _HologramColor * 2.;
                col += scan * _HologramColor;
                //col += glow * _GlowColor * _GlowColor.a;

                //col = lerp(distortColor, col.rgb, rim);
                col *= _Brightness;
				//color += (1 - intersect) * (face > 0 ? .03:.3) * _MainColor * _Fresnel;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return float4(distortColor + col*_HologramColor.a, 1.);
            }

            ENDCG
        }
    }
}
Shader "ShaderBuoyz/Glitch"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _NoiseTex("Noise Tex", 2D) = "white" {}
        _NoiseGlitch("Noise Glitch", 2D) = "white" {}
        _Speed("Speed", Float) = 1.
        _Amplitude("Amplitude", Float) = 1.
        _NoiseAmt("Noise Amt", Float) = 1.
        _NoiseSpd("Noise Speed", Float) = 1.

        [Toggle(VIGNETTE_ON)]_VignetteOn("Show Vignette" , float) = 0
        _SpeedVignette("Speed Vignette", Float) = 1.
        _AmountVignette("Amount Vignette", Range(0., 0.4)) = 0.19

        [Toggle(LINES_ON)]_LinesOn("Lines Noise" , float) = 0
        _AmountLines("Amount Lines", Float) = 1.
			_IntensityLines("Intensity Line", Float) = 1.
        _SizeLines("Size Lines", Float) = 1.
        _LineSpeed("Line Speed", Float) = 1.
			_WhiteLinesIntensity("White Lines Intensity", Float) = 1.
			_WhiteLinesSpeed("White Lines Speed", Float) = 1.
			_WhiteLinesNumber("White Linex Number", Float) = 1.

		[Toggle(WHITE_LINES)]_WhiteLines("Lines White" , float) = 0


    }

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma shader_feature_local VIGNETTE_ON
            #pragma shader_feature_local LINES_ON
			#pragma shader_feature_local WHITE_LINES
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;

            uniform float4x4 _FrustumCornersES;
            uniform float4 _MainTex_TexelSize;
            uniform float4x4 _CameraInvViewMatrix;
            uniform float3 _CameraWS;
            uniform float3 _LightDir;

            // Properties
            uniform sampler2D _NoiseTex, _NoiseGlitch;
            uniform float _Speed, _NoiseAmt, _NoiseSpd, _IntensityLines, _WhiteLinesIntensity, _WhiteLinesNumber;
            uniform float _SpeedVignette;
            uniform float _AmountVignette;
            uniform float _Amplitude;
            uniform float _SizeLines;
            uniform float _AmountLines, _LineSpeed, _WhiteLinesSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 ray : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;

                half index = v.vertex.z;
                v.vertex.z = 0.1;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv.xy;

                #if UNITY_UV_STARTS_AT_TOP
                if (_MainTex_TexelSize.y < 0)
                    o.uv.y = 1 - o.uv.y;
                #endif

                return o;
            }

            float4 vec4pow(in float4 v, in float p ) {
                return float4(pow(v.x,p),pow(v.y,p),pow(v.z,p),v.w);
            }

            float4 rgbShift(sampler2D tex, in float2 p, in float4 shift) {
                shift *= 2.0*shift.w - 1.0;
                float2 rs = float2(shift.x,-shift.y);
                float2 gs = float2(shift.y,-shift.z);
                float2 bs = float2(shift.z,-shift.x);
                float r = tex2D(tex, p+rs).x;
                float g = tex2D(tex, p+gs).y;
                float b = tex2D(tex, p+bs).z;
                return float4(r,g,b,1.0);
            }

            float4 noise(float2 p)
            {
                float timeS = _Time.x;

                float s = tex2D(_NoiseTex,float2(1.,2.*cos(timeS))*timeS*8. + p*1.).xyzw;
                s *= s;
                return s;
            }

            float ramp(float y, float start, float end)
            {
                float inside = step(start,y) - step(end,y);
                float fact = (y-start)/(end-start)*inside;
                return (1.-fact) * inside;
            }

            float stripes(float2 uv)
            {
                float noi = noise(uv*float2(0.5,1.) + float2(1.,3.)).x;
                                float timeS = _Time.x * _NoiseSpd;

                return ramp(fmod(uv.y*4. + timeS/2.+sin(timeS + sin(timeS*0.63)),1.),0.5,0.6)*noi;
            }

            float4 noise2(float2 p)
            {
                float timeS = _Time.x * _NoiseSpd;

                float s = tex2D(_NoiseTex,float2(1.,2.*cos(timeS))*timeS*8. + p*1.).rgba;
                s *= s;
                return s;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 bg = float4(0., 0. ,0., 1.);

                float iTime = _Time.y;

                float vspeed = _SpeedVignette;
                float vtime = iTime * vspeed;
                float vamt = _AmountVignette;

                float amplitude = _Amplitude;

                float4 noiseVal = tex2D(_NoiseGlitch, float2(_Speed*iTime,2.0*_Speed*iTime/25.0));
                float linesAmt = (12.+fmod(i.uv.y*_SizeLines+iTime*_LineSpeed,1.))/13.;

                float4 shift = vec4pow(noiseVal*linesAmt,8.0) * float4(amplitude,amplitude,amplitude,1.0);


                float2 noiseVal2 = tex2D(_NoiseTex, i.uv*10.+float2(0.1,1.)*iTime*_LineSpeed).xy;
                float noiseVal3 = noise2(i.uv.xy*10.).x*_NoiseAmt/2.;
                float2 noiseVal4 = noise2(i.uv.xy*.1).rg*_NoiseAmt/2.;

                float2 lineDist2 = float2(1. - pow(linesAmt, _AmountLines)*_IntensityLines, 1.);
                bg += rgbShift(_MainTex, i.uv*lineDist2, shift);

#ifdef WHITE_LINES
                bg += stripes((i.uv + _WhiteLinesSpeed*iTime)*float2(1. - linesAmt*linesAmt*linesAmt*_AmountLines, 1. + _WhiteLinesNumber))*_WhiteLinesIntensity;
#endif
                bg += noiseVal3;

                #ifdef VIGNETTE_ON
                float vigAmt = vamt*(3.+.3*sin(vtime + 5.*cos(vtime*5.)));
                float2 cuv = i.uv*2. - 1.;
                float vignette = (1.-vigAmt*(cuv.y)*(cuv.y))*(1.-vigAmt*(cuv.x)*(cuv.x));
                bg *= vignette;
                #endif

                #ifdef LINES_ON
                bg *= linesAmt;
                #endif

                return bg;
            }

            ENDCG
        }
    }
}

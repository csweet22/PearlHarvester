Shader "Custom/Stochastic"
{
    Properties
    {
        [Header(Standard Shader)]
    	[Space(10)]
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        [NoScaleOffset] _NormalMap ("Normal Map", 2D) = "bump" {}
    	_NormalStrength ("Normal Strength", float) = 1
    	_Metallic ("Metallic", Range(0,1)) = 0
    	_Smoothness ("Smoothness", Range(0,1)) = 0.5
    	
    	
        [Header(Look Parameters)]
    	[Space(10)]
    	[Enum(Texture,0, UVs,1, Weights,2, Vertex Hash,3)] _DisplayMode ("Display Mode", Float) = 0
        [Toggle] _EnableStochastic ("Enable Stochastic Sampling", Range(0,1)) = 1
    	_Skew ("Skew", Range(0,1)) = 1
        [Toggle] _EnableRotationSampling ("Enable Rotated Sampling", Range(0,1)) = 0
    	_MaxRotationDegrees ("Max Rotation (Degrees)", Range(0,360)) = 360
        [Toggle] _EnableMirrorSampling ("Enable Mirrored Sampling", Range(0,1)) = 0
        [PowerSlider(3)] _Contrast ("Blend Contrast", Range(0,50)) = 2.5
    	_LuminanceBias ("Luminance Bias", Range(0,1)) = 0.0
    	_SampleUVScale ("Sample UV Scale", float) = 1
    	_Seed ("Random Seed", Integer) = 1
    	
        [Header(Debug)]
    	[Space(10)]
        [Toggle] _ShowDebugLines ("Show Debug Lines", Range(0,1)) = 0
    	_DebugLineWidth ("Debug Lines Width", Range(0,0.1)) = 0.02
        [Toggle] _ShowVertices ("Show Debug Vertices", Range(0,1)) = 0
    	_VerticesRadius ("Vertices Radius", Range(0,0.25)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
    	#pragma exclude_renderers gles
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

		#include "blendFunctions.cginc"
        
        sampler2D _MainTex;
        sampler2D _NormalMap;

        struct Input
        {
            float2 uv_MainTex;
        	float3 worldNormal; INTERNAL_DATA
        };

		half _EnableStochastic;
        
        half _NormalStrength;
        half _Metallic;
        half _Smoothness;
        half _Skew;
        half _Contrast;
        half _EnableRotationSampling;
        half _MaxRotationDegrees;
        half _EnableMirrorSampling;
        half _DisplayMode;
        half _Seed;
        half _LuminanceBias;
        half _SampleUVScale;
        
        half _ShowDebugLines;
        half _DebugLineWidth;
        
        half _ShowVertices;
        half _VerticesRadius;

        
		float2 hash2D2D (float2 s) {
			return frac(sin(fmod(float2(dot(s + float2(1,1), float2(127.1,311.7 * _Seed)), dot(s, float2(_Seed * 269.5,183.3))), 3.14159))*43758.5453);
		}
        
		float3 hash2D3D (float2 s) {
			return frac(sin(fmod(float3(dot(s, float2(_Seed * 127.1,311.7)), dot(s, float2(269.5,_Seed * 183.3)), dot(s, float2(65.9,12.3))), 3.14159))*43758.5453);
		}
        
		float4 tex2DStochastic(sampler2D tex, float2 UV, bool isNormal)
		{
			// STAGE 1: Skewing the UVs
			
			// Create a matrix to skew/shear the UV grid
				// This is done to make triangles instead of squares
				// so that we only need to sample for 3 points instead of 4
			float2x2 skewMatrix = float2x2(
				1.0, (_Skew)*-0.57735027,
				0.0, (_Skew)*1.15470054 + (1-(_Skew))
        	);

			float2 skewedUV = mul(skewMatrix, UV);
        	

			// STAGE 2:
			//		Getting the fragments barycentric coordinates
			//		Getting the floored vertex position of the barycentric triangle
			
			// vertex position in UV space and barycentric coordinates
			float2 vertPosUV = floor(skewedUV);
			float3 baryCoord = float3 (frac(skewedUV), 0);

			// This just gives us a value based on if the fragments coordinate is in the first or second triangle that makes up the square
			//       _________ (1,1)
			//       | \   1 |
			//       |   \   |
			//       | -1  \ |
			// (0,0) ---------
			baryCoord.z = 1.0 - baryCoord.x - baryCoord.y;

			// STAGE 3:
			//		Calculating vertex positions, weights, and colors.
			//		This also turns our square into 2 triangles based on the z value in the barycentric coords.
			
        	float2 vert1Pos;
        	float2 vert2Pos;
        	float2 vert3Pos;
			
			float vert1Weight;
			float vert2Weight;
			float vert3Weight;
			
			if (baryCoord.z > 0.0)
			{
				// 2 x
				// v 3
				vert1Pos = vertPosUV;
				vert2Pos = vertPosUV + float2(0, 1);
				vert3Pos = vertPosUV + float2(1, 0);
				vert1Weight = baryCoord.z;
				vert2Weight = baryCoord.y;
				vert3Weight = baryCoord.x;
			}
			else
			{
				// 3 1
				// v 2
				vert1Pos = vertPosUV + float2 (1, 1);
				vert2Pos = vertPosUV + float2 (1, 0);
				vert3Pos = vertPosUV + float2 (0, 1);
				vert1Weight = -baryCoord.z;
				vert2Weight = 1.0 - baryCoord.y;
				vert3Weight = 1.0 - baryCoord.x;
			}


			float2 vert1Hash = hash2D2D(vert1Pos);
			float2 vert2Hash = hash2D2D(vert2Pos);
			float2 vert3Hash = hash2D2D(vert3Pos);
			
			float2 dx = ddx(UV);
			float2 dy = ddy(UV);

			float2 vert1UVs = UV;
			float2 vert2UVs = UV;
			float2 vert3UVs = UV;

			if (_EnableRotationSampling)
			{
				float phi1 = vert1Hash.x*123.34 % ((_MaxRotationDegrees+1) * 3.1415 / 180);
				float phi2 = vert2Hash.x*123.34 % ((_MaxRotationDegrees+1) * 3.1415 / 180);
				float phi3 = vert3Hash.x*123.34 % ((_MaxRotationDegrees+1) * 3.1415 / 180);
				float2x2 vert1Rotation =
					float2x2(cos(phi1),-sin(phi1),sin(phi1),cos(phi1));
				float2x2 vert2Rotation =
					float2x2(cos(phi2),-sin(phi2),sin(phi2),cos(phi2));
				float2x2 vert3Rotation =
					float2x2(cos(phi3),-sin(phi3),sin(phi3),cos(phi3));
				
				vert1UVs = mul(vert1Rotation, vert1UVs);
				vert2UVs = mul(vert2Rotation, vert2UVs);
				vert3UVs = mul(vert3Rotation, vert3UVs);
			}

			vert1UVs *= _SampleUVScale;
			vert2UVs *= _SampleUVScale;
			vert3UVs *= _SampleUVScale;

			if (_EnableMirrorSampling)
			{
				float reflected1x = (floor(vert1Hash.x*100) % 2 == 0) ? 1 : -1;
				float reflected1y = (floor(vert1Hash.y*100) % 2 == 0) ? 1 : -1;
				vert1UVs *= float2(reflected1x, reflected1y);
				
				float reflected2x = (floor(vert2Hash.x*100) % 2 == 0) ? 1 : -1;
				float reflected2y = (floor(vert2Hash.y*100) % 2 == 0) ? 1 : -1;
				vert2UVs *= float2(reflected2x, reflected2y);
				
				float reflected3x = (floor(vert3Hash.x*100) % 2 == 0) ? 1 : -1;
				float reflected3y = (floor(vert3Hash.y*100) % 2 == 0) ? 1 : -1;
				vert3UVs *= float2(reflected3x, reflected3y);
			}
			
			vert1UVs += vert1Hash;
			vert2UVs += vert2Hash;
			vert3UVs += vert3Hash;
			
			float4 vert1Color = tex2D(tex, vert1UVs, dx, dy);
			float4 vert2Color = tex2D(tex, vert2UVs, dx, dy);
			float4 vert3Color = tex2D(tex, vert3UVs, dx, dy);

			float3 FragWeights = float3(vert1Weight,vert2Weight,vert3Weight);
			
			float4 finalColor =
					HLWeightBlend(
						FragWeights,
						vert1Color,
						vert2Color,
						vert3Color,
						_Contrast,
						_LuminanceBias
					);
			
			// STAGE 4:
			//		Summing colors and returning value.
			

			if (!isNormal)
			{
				if (_DisplayMode == 1)
					finalColor = float4(frac(skewedUV * _SampleUVScale), 0.0,1.0);
				else if (_DisplayMode == 2)
					finalColor = float4(FragWeights.x,FragWeights.y,FragWeights.z,1.0);
				else if (_DisplayMode == 3)
					finalColor = float4(
						FragWeights.x * hash2D3D(vert1Hash) + 
						FragWeights.y * hash2D3D(vert2Hash) + 
						FragWeights.z * hash2D3D(vert3Hash),
						1.0
						);
				
				if (_ShowDebugLines)
				{
					if ( abs(baryCoord.x) < _DebugLineWidth)
						finalColor = float4(0.0,0.0,0.0,1.0);
					if ( abs(baryCoord.y) < _DebugLineWidth)
						finalColor = float4(0.0,0.0,0.0,1.0);
					if ( abs(baryCoord.z) < (_DebugLineWidth/2))
						finalColor = float4(0.0,0.0,0.0,1.0);
				}
				
				if (_ShowVertices)
				{
					if ( abs(distance(skewedUV, vert1Pos)) < _VerticesRadius)
						finalColor = float4(1.0,0.0,0.0,1.0);
					
					if ( abs(distance(skewedUV, vert2Pos)) < _VerticesRadius)
						finalColor = float4(0.0,1.0,0.0,1.0);
					
					if ( abs(distance(skewedUV, vert3Pos)) < _VerticesRadius)
						finalColor = float4(0.0,0.0,1.0,1.0);
				}
			}
			
			return finalColor;
		}
        
        void surf (Input IN, inout SurfaceOutputStandard o) {
			if (_EnableStochastic)
			{
	            fixed4 c = tex2DStochastic(_MainTex, IN.uv_MainTex, false);
	            o.Albedo = c.rgb;
				o.Normal = UnpackNormalWithScale(tex2DStochastic(_NormalMap, IN.uv_MainTex, true), _NormalStrength);
				o.Alpha = c.a;
			} else
			{
	            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	            o.Albedo = c.rgb;
				o.Normal = UnpackNormalWithScale(tex2D(_NormalMap, IN.uv_MainTex), _NormalStrength);
			    o.Alpha = c.a;
			}
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

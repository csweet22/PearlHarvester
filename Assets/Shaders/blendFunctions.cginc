// Luminance Bias affects the weight given to a fragment based on its luminance
	// 1.0 favours high values
	// 0.0 favours low values
float adjustLumimanceBias(float3 textureSample, float luminanceBias)
{
	return (1.0 - luminanceBias)*(1.0 - textureSample) + (luminanceBias)*(textureSample); //dot(textureSample, _LuminanceVector);
}

float4 HLWeightBlend(inout half3 weights, float4 tex0, float4 tex1, float4 tex2, half contrast, float luminanceBias)
{
    // Compute weight with height/luminance
    const half epsilon = 1.0f / 1024.0f;
    weights =
    	half3(
    		weights.x * ( adjustLumimanceBias(tex0, luminanceBias) + epsilon),
            weights.y * ( adjustLumimanceBias(tex1, luminanceBias) + epsilon),
            weights.z * ( adjustLumimanceBias(tex2, luminanceBias) + epsilon)
        );
 
    // Contrast weights
    half maxWeight = max(weights.x, max(weights.y, weights.z));
    half transition = (1/(contrast + epsilon)) * maxWeight;
    half threshold = maxWeight - transition;
    half scale = 1.0f / transition;
    weights = saturate((weights - threshold) * scale);

	
    // Normalize weights.
    half weightSum = (weights.x + weights.y + weights.z);
    weights = weights / weightSum;
	
    return (weights.x * tex0) + (weights.y * tex1) + (weights.z * tex2);
}

float3 linearBlend(float3 tex0, float3 tex1, float alpha)
{
	return lerp(tex0, tex1, alpha);
}

float3 smoothstepBlend(float3 tex0, float3 tex1, float alpha)
{
	return lerp(tex0, tex1, alpha);
}
sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;

float4 OktoberfestDye(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);

	if (!any(color))
		return color;

	float3 foamColor = float3(0.9f, 0.825f, 0.75f);
	float3 beerColor = float3(0.8f, 0.5f, 0);
	float foamOffset = 0.2f;

	float blendFactor = 0.6f;

	float highlightBrightness = 0.15f;
	float highlightOffset = 0.5f + (0.2f * uDirection);
	float highlightFalloff = 80;

	if (coords.y < foamOffset) color.rgb = (color.rgb * blendFactor) + foamColor;
	else color.rgb = (color.rgb * blendFactor) + beerColor;

	float distanceFromHighlight = coords.x - highlightOffset;
	distanceFromHighlight *= distanceFromHighlight;
	float highlightFactor = (1 / (distanceFromHighlight * highlightFalloff)) * highlightBrightness;
	color.rgb += highlightFactor;

	return color;
}

technique Technique1
{
	pass OktoberfestDye
	{
		// TODO: set renderstates here.

		PixelShader = compile ps_2_0 OktoberfestDye();
	}
}

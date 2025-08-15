shader_type canvas_item;

uniform vec4 tint : source_color = vec4(1.0);

uniform float grayscaleFactor = 0.0;
//uniform float rows = 5.0;

const float yScaling = (72.0/350.0) / (470.0/250.0);

//varying vec4 modulate;

void vertex()
{
	//modulate = COLOR;
}

void fragment()
{
	vec2 topUV = vec2(UV.x, UV.y * yScaling);
	//float normalizedCellSize = 1.0 / rows;
	//float normalizedHeight = normalizedCellSize * yScaling;

	//vec2 bottomUV = vec2(UV.x, UV.y * yScaling + normalizedCellSize - normalizedHeight);
	COLOR = texture(TEXTURE, topUV);

	//COLOR.rgb = vec3(0.3333 * COLOR.r + 0.3333 * COLOR.g + 0.3333 * COLOR.b);
	vec3 grayscale = vec3(0.2126 * COLOR.r + 0.7152 * COLOR.g + 0.0722 * COLOR.b);

	COLOR.rgb = mix(COLOR.rgb, grayscale, grayscaleFactor);
	//COLOR.a *= tint.a;
	COLOR *= tint;
}

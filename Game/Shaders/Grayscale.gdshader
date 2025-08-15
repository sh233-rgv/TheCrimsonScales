shader_type canvas_item;

uniform vec4 tint : source_color = vec4(1.0);

varying vec4 modulate;

void vertex()
{
	modulate = COLOR;
}

void fragment()
{
	COLOR = texture(TEXTURE, UV);
	//COLOR.rgb = vec3(0.3333 * COLOR.r + 0.3333 * COLOR.g + 0.3333 * COLOR.b);
	COLOR.rgb = vec3(0.2126 * COLOR.r + 0.7152 * COLOR.g + 0.0722 * COLOR.b) * tint.rgb;
	COLOR.a *= tint.a;
	COLOR *= modulate;
}

shader_type canvas_item;

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_linear_mipmap;

varying vec4 modulate;

void vertex()
{
	modulate = COLOR;
}

void fragment()
{
	vec4 color = texture(TEXTURE, UV);
	vec4 screenTex = textureLod(screen_texture, SCREEN_UV, 2.5);
	COLOR = mix(screenTex, color * modulate, modulate.a);
	COLOR.a = color.a;
}

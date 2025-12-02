using System;
using Godot;

public partial class SelectFigureViewFigure : Control
{
	[Export]
	private BetterButton _button;
	[Export]
	private Sprite2D _sprite;

	public Figure Figure { get; private set; }

	public event Action<SelectFigureViewFigure> PressedEvent;

	public override void _Ready()
	{
		base._Ready();

		_button.Pressed += OnPressed;
	}

	public void Init(Figure figure)
	{
		Figure = figure;

		Texture2D mapIconTexture = figure.MapIconTexture;
		_sprite.SetTexture(mapIconTexture);
		float textureWidth = mapIconTexture.GetWidth();
		_sprite.SetScale((250f / textureWidth) * Vector2.One);
	}

	private void OnPressed()
	{
		PressedEvent?.Invoke(this);
	}
}
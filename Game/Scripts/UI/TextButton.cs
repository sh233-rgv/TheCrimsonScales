using System;
using Godot;

public partial class TextButton : Control
{
	public class Parameters
	{
		public string Text { get; set; }
		public Action OnPressed { get; set; }
		public ColorType ColorType { get; set; }
		public float? Width { get; set; }

		public Parameters(string text, Action onPressed, ColorType colorType = ColorType.White, float? width = null)
		{
			Text = text;
			OnPressed = onPressed;
			ColorType = colorType;
			Width = width;
		}
	}

	public enum ColorType
	{
		White = 0,
		Red = 1,
		Green = 2,
	}

	private static readonly Color[] Colors =
	[
		Godot.Colors.White,
		Color.FromHtml("ff5e3a"),
		Color.FromHtml("75ff5f")
	];

	[Export]
	private BetterButton _button;
	[Export]
	private Label _label;

	public event Action PressedEvent;

	public override void _Ready()
	{
		base._Ready();

		_button.Pressed += OnPressed;
	}

	public void Init(Parameters parameters)
	{
		Init(parameters.Text, parameters.OnPressed, parameters.ColorType, parameters.Width);
	}

	public void Init(string text, Action onPressed, ColorType colorType = ColorType.White, float? width = null)
	{
		_label.SetText(text);
		PressedEvent = onPressed;
		SetModulate(Colors[(int)colorType]);
		if(width.HasValue)
		{
			SetCustomMinimumSize(new Vector2(width.Value, CustomMinimumSize.Y));
		}
	}

	private void OnPressed()
	{
		PressedEvent?.Invoke();
	}
}
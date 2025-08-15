using Godot;

public partial class LongPressIndicator : Control
{
	private const float CentimetersToInches = 1.0f / 2.54f;

	[Export]
	private TextureRect _background;
	[Export]
	private TextureRect _fill;

	public override void _Ready()
	{
		base._Ready();

		SetVisible(false);

		const float centimeters = 1.5f;
		float pixels = CentimetersToPixels(centimeters);
		SetSize(pixels);

		if(Platform.DeskTop)
		{
			return;
		}

		AppController.Instance.InputController.PressDurationChangedEvent += OnPressDurationChanged;
	}

	private void SetSize(float pixelSize)
	{
		SetSize(new Vector2(pixelSize, pixelSize));
		_fill.SetPivotOffset(_fill.Size * 0.5f);
	}

	private void OnPressDurationChanged(float pressDuration)
	{
		if(AppController.Instance.InputController.Dragging)
		{
			SetVisible(false);
			return;
		}

		float t = pressDuration / InputController.LongPressDuration;
		_fill.SetScale(Mathf.Clamp(t, 0f, 1f) * Vector2.One);

		SetModulate(new Color(1f, 1f, 1f, Mathf.InverseLerp(0.35f, 1f, t)));
		SetVisible(t > 0.35f && t < 1.05f);

		GlobalPosition = GetViewport().GetMousePosition() - Size * 0.5f;
	}

	private int CentimetersToPixels(float centimeters)
	{
		int dpi = DisplayServer.ScreenGetDpi(DisplayServer.WindowGetCurrentScreen());
		float inches = centimeters * CentimetersToInches / GetTree().Root.ContentScaleFactor;
		return (int)Mathf.Round(inches * dpi);
	}
}
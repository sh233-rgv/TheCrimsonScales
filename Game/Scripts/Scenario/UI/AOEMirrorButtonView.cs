using System;
using Godot;

public partial class AOEMirrorButtonView : Control
{
	[Export]
	private ChoiceButton _mirrorButton;

	public event Action MirrorPressed;

	public override void _Ready()
	{
		base._Ready();

		_mirrorButton.BetterButton.Pressed += OnMirrorPressed;
	}

	public void Open()
	{
		_mirrorButton.SetActive(true);

		_mirrorButton.SetPosition(new Vector2(_mirrorButton.Position.X, GameController.Instance.EffectSelectionView.EffectCount > 0 ? -150f : 0f));
	}

	public void Close()
	{
		_mirrorButton.SetActive(false);
	}

	private void OnMirrorPressed()
	{
		MirrorPressed?.Invoke();
	}
}
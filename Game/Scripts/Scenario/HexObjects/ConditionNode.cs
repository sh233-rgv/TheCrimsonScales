using Godot;
using GTweens.Easings;
using GTweens.Tweens;
using GTweensGodot.Extensions;

public partial class ConditionNode : Node2D
{
	[Export]
	private Sprite2D _sprite;
	[Export]
	private Label _stackLabel;

	private bool _hasMoved;
	private GTween _tween;

	public void Init(ConditionModel condition, bool skipScaleAnimation = false)
	{
		_sprite.Texture = ResourceLoader.Load<Texture2D>(condition.IconPath);

		if(!skipScaleAnimation)
		{
			Scale = Vector2.Zero;
			this.TweenScale(1f, 0.3f).SetEasing(Easing.OutBack).PlayFastForwardable();
		}

		SetStackText(null);
	}

	public void Destroy()
	{
		this.TweenScale(0f, 0.3f).SetEasing(Easing.InBack).OnComplete(QueueFree).PlayFastForwardable();
	}

	public void Move(Vector2 position)
	{
		_tween?.Kill();

		_tween = this.TweenPosition(position, 0.2f).SetEasing(Easing.OutBack).PlayFastForwardable();

		if(!_hasMoved)
		{
			_tween.Complete();
			_hasMoved = true;
		}
	}

	public void Flash()
	{
		this.TweenPulse(1.4f, 0.2f).PlayFastForwardable();
	}

	public void SetStackText(string text)
	{
		_stackLabel.Visible = text != null;
		_stackLabel.Text = text;
	}
}
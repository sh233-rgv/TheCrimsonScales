using Godot;
using GTweens.Builders;
using GTweens.Tweens;
using GTweensGodot.Extensions;

public partial class CardSelectionPopupPreview : Control
{
	[Export]
	private CardView _cardView;

	private CardSelectionPopupCard _focus;

	private GTween _tween;

	public override void _Ready()
	{
		base._Ready();

		Hide();
	}

	public void Focus(CardSelectionPopupCard card)
	{
		_focus = card;
		_cardView.SetCard(card.AbilityCardModel);
		_cardView.SetCardMaterial(_focus.Grayscale ? CardState.Discarded : CardState.Hand);

		Control parentContainer = GetParent<Control>();
		float targetY = Mathf.Clamp(
			card.GlobalPosition.Y + card.Size.Y * 0.5f - Size.Y * 0.5f,
			parentContainer.GlobalPosition.Y,
			parentContainer.GlobalPosition.Y + parentContainer.Size.Y - Size.Y);

		if(!Visible)
		{
			GlobalPosition = new Vector2(GlobalPosition.X, targetY);
		}

		Show();

		_tween?.Kill();
		float animationDuration = 0.2f * (1f - _cardView.Modulate.A);
		_tween = GTweenSequenceBuilder.New()
			//.Append(_cardView.TweenPositionX(0f, animationDuration))
			.Join(_cardView.TweenModulateAlpha(1f, animationDuration))
			.Join(this.TweenGlobalPositionY(targetY, 0.03f))
			.Build().Play();
	}

	public void Unfocus(CardSelectionPopupCard card)
	{
		if(_focus == card)
		{
			_tween?.Kill();
			float animationDuration = 0.2f * _cardView.Modulate.A;
			_tween = GTweenSequenceBuilder.New()
				.AppendTime(0.05f)
				//.Append(_cardView.TweenPositionX(-100f, animationDuration))
				.Join(_cardView.TweenModulateAlpha(0f, animationDuration))
				.AppendCallback(Hide)
				.Build().Play();
		}
	}
}
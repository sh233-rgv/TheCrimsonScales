using Godot;
using GTweens.Builders;
using GTweens.Tweens;
using GTweensGodot.Extensions;

public partial class CardSelectionCardPreview : Control
{
	[Export]
	private CardView _cardView;

	private CardSelectionCard _focus;

	private GTween _tween;
	private float _originOffset;

	public override void _Ready()
	{
		base._Ready();

		_cardView.SetModulate(Colors.Transparent);
		Hide();
	}

	public void Focus(CardSelectionCard card)
	{
		_focus = card;
		_cardView.SetCard(card.SavedAbilityCard.Model);
		//_cardView.SetCardMaterial(card.SavedAbilityCard.CardState);

		Control parentContainer = GetParent<Control>();
		float targetY = Mathf.Clamp(
			card.GlobalPosition.Y + CardSelectionCard.Size.Y * 0.5f - Size.Y * 0.5f,
			parentContainer.GlobalPosition.Y,
			parentContainer.GlobalPosition.Y + parentContainer.Size.Y - Size.Y);

		if(!Visible)
		{
			GlobalPosition = new Vector2(GlobalPosition.X, targetY);
		}
		
		_originOffset = card.GlobalPosition.X > GlobalPosition.X ? 100f : -100f;

		if(!Visible)
		{
			_cardView.SetPosition(new Vector2(_originOffset, _cardView.Position.Y));
		}

		Show();

		_tween?.Kill();
		float animationDuration = 0.2f * (1f - _cardView.Modulate.A);
		_tween = GTweenSequenceBuilder.New()
			.Append(_cardView.TweenPositionX(0f, animationDuration))
			.Join(_cardView.TweenModulateAlpha(1f, animationDuration))
			.Join(this.TweenGlobalPositionY(targetY, 0.03f))
			.Build().Play();
	}

	public void Unfocus(CardSelectionCard card)
	{
		if(_focus == card)
		{
			_tween?.Kill();
			float animationDuration = 0.2f * _cardView.Modulate.A;
			_tween = GTweenSequenceBuilder.New()
				.AppendTime(0.05f)
				.Append(_cardView.TweenPositionX(_originOffset, animationDuration))
				.Join(_cardView.TweenModulateAlpha(0f, animationDuration))
				.AppendCallback(Hide)
				.Build().Play();
		}
	}
}
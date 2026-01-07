using System.Collections.Generic;
using System.Linq;
using Godot;
using GTweens.Builders;
using GTweens.Easings;
using GTweensGodot.Extensions;

public partial class Enhancer : BetweenScenariosAction
{
	[Export]
	private Node3D _3dRoot;
	[Export]
	private Node3D _crystalBall;

	[Export]
	private Control _cardListContainer;
	[Export]
	private CardSelectionList _cardSelectionList;
	[Export]
	private Control _cardContainer;
	[Export]
	private Control _cardRotationContainer;
	[Export]
	private CardView _cardView;

	protected override bool SelectCharacter => true;

	public override void _Ready()
	{
		base._Ready();

		_3dRoot.SetVisible(false);

		BetweenScenariosController.Instance.CharacterPortraitManager.SelectedPortraitChangedEvent += OnSelectedPortraitChanged;
	}

	protected override void AnimateIn(GTweenSequenceBuilder sequenceBuilder, BetweenScenariosAction previousActiveAction)
	{
		base.AnimateIn(sequenceBuilder, previousActiveAction);

		_3dRoot.SetVisible(true);
		_crystalBall.SetVisible(false);

		_crystalBall.SetPosition(new Vector3(0f, 5f, 0f));
		_cardListContainer.SetPosition(new Vector2(-400f, _cardListContainer.Position.Y));
		_cardContainer.SetPosition(new Vector2(0f, 800f));
		_cardRotationContainer.SetRotationDegrees(30f);

		sequenceBuilder
			.AppendTime(previousActiveAction is ItemShop ? 0.5f : 0.2f)
			.AppendCallback((() =>
			{
				_crystalBall.SetVisible(true);

				_cardListContainer.TweenPositionX(120f, 0.5f).SetEasing(Easing.OutBack).Play();
				_cardContainer.TweenPositionY(0f, 0.6f).SetEasing(Easing.OutCubic).Play();
				_cardRotationContainer.TweenRotationDegrees(0f, 0.6f).SetEasing(Easing.OutCubic).Play();

				UpdateCardList();
			}))
			.Append(_crystalBall.TweenPositionY(0f, 0.7f).SetEasing(Easing.InQuad))
			.Append(_crystalBall.TweenPositionY(0.1f, 0.12f))
			.Append(_crystalBall.TweenPositionY(0f, 0.08f))
			.AppendTime(0.2f);
	}

	protected override void AfterAnimateIn()
	{
		base.AfterAnimateIn();
	}

	protected override void AnimateOut(GTweenSequenceBuilder sequenceBuilder)
	{
		sequenceBuilder
			.Append(_crystalBall.TweenPositionY(5f, 0.5f))
			.Join(_cardListContainer.TweenPositionX(-400f, 0.5f).SetEasing(Easing.InBack))
			.Join(_cardContainer.TweenPositionY(800f, 0.5f).SetEasing(Easing.InQuad))
			.Join(_cardRotationContainer.TweenRotationDegrees(30f, 0.5f).SetEasing(Easing.OutQuad))
			.AppendTime(0.2f);

		base.AnimateOut(sequenceBuilder);
	}

	protected override void AfterAnimateOut()
	{
		base.AfterAnimateOut();

		_3dRoot.SetVisible(false);
	}

	private void UpdateCardList()
	{
		List<SavedAbilityCard> cards =
			BetweenScenariosController.Instance.CharacterPortraitManager.SelectedPortrait?.SavedCharacter.AvailableAbilityCards;

		if(cards == null)
		{
			_cardSelectionList.Close();
			_cardContainer.SetVisible(false);
		}
		else
		{
			_cardSelectionList.Open(cards, OnCardPressed, null,
				(cardA, cardB) => cardA.Model.Initiative.CompareTo(cardB.Model.Initiative));
			_cardContainer.SetVisible(true);
			OnCardPressed(_cardSelectionList.Cards.First());
		}
	}

	private void OnCardPressed(CardSelectionCard cardSelectionCard)
	{
		foreach(CardSelectionCard card in _cardSelectionList.Cards)
		{
			card.SetSelected(false);
		}

		cardSelectionCard.SetSelected(true);
		_cardView.SetCard(cardSelectionCard.SavedAbilityCard);
	}

	private void OnSelectedPortraitChanged(BetweenScenariosCharacterPortrait portrait)
	{
		UpdateCardList();
	}
}
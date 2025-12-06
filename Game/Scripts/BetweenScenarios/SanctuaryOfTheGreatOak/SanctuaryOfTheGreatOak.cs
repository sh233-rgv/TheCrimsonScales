using System.Collections.Generic;
using System.Linq;
using Godot;
using GTweens.Builders;
using GTweens.Easings;
using GTweensGodot.Extensions;

public partial class SanctuaryOfTheGreatOak : BetweenScenariosAction
{
	[Export]
	private Node3D _3dRoot;

	[Export]
	private AnimationPlayer _animationPlayer;

	[Export]
	private StringName _moveInAnimationName;
	[Export]
	private StringName _moveOutAnimationName;

	[Export]
	private PackedScene _donationCoinScene;
	[Export]
	private Node3D _donationCoinContainer;
	[Export]
	private SyncingBody _syncingBody;
	[Export]
	private Node3D _bowlVisualContainer;

	[Export]
	private ChoiceButton _donateButton;

	private bool _donationButtonAvailable;

	protected override bool SelectCharacter => true;

	private readonly List<DonationCoin> _donationCoins = new List<DonationCoin>();

	public override void _Ready()
	{
		base._Ready();

		_3dRoot.SetVisible(false);
		_bowlVisualContainer.SetVisible(false);

		_donateButton.SetActive(false);
		_donateButton.BetterButton.Pressed += OnDonatePressed;

		BetweenScenariosController.Instance.CharacterPortraitManager.SelectedPortraitChangedEvent += OnSelectedPortraitChanged;
	}

	protected override void AnimateIn(GTweenSequenceBuilder sequenceBuilder, BetweenScenariosAction previousActiveAction)
	{
		base.AnimateIn(sequenceBuilder, previousActiveAction);

		sequenceBuilder
			.AppendTime(previousActiveAction is ItemShop ? 0.5f : 0f)
			.AppendCallback((() =>
			{
				this.DelayedCall(() =>
				{
					_3dRoot.SetVisible(true);
				});
				this.DelayedCall(() =>
				{
					_bowlVisualContainer.SetVisible(true);
				}, 0.1f);
				this.DelayedCall(() =>
				{
					for(int i = 0; i < 3; i++)
					{
						CreateDonationCoin(0.1f, 0.2f);
					}
				}, 0.2f);
				_animationPlayer.Play(_moveInAnimationName);
			}))
			.AppendCallback(() =>
			{
				this.DelayedCall(() =>
				{
					_donationButtonAvailable = true;
					UpdateDonateButton();
				}, 1f);

				this.DelayedCall(() =>
				{
					foreach(DonationCoin coin in _donationCoins)
					{
						coin.ApplyImpulse((float)GD.RandRange(0.1f, 1f) * Vector3.Right * 0.3f, Vector3.Zero);
					}
				}, 0.6f);

				this.DelayedCall(() =>
				{
					foreach(DonationCoin coin in _donationCoins)
					{
						coin.SetSleeping(true);
					}
				}, 2f);
			})
			.AppendTime(1f);
	}

	protected override void AfterAnimateIn()
	{
		base.AfterAnimateIn();
	}

	protected override void AnimateOut(GTweenSequenceBuilder sequenceBuilder)
	{
		_donationButtonAvailable = false;
		UpdateDonateButton();

		foreach(DonationCoin coin in _donationCoins)
		{
			coin.SetSleeping(false);
		}

		sequenceBuilder.AppendTime(1f).AppendCallback(() =>
		{
			_3dRoot.SetVisible(false);
			_bowlVisualContainer.SetVisible(false);
			_animationPlayer.Play("RESET");
		});

		_animationPlayer.Play(_moveOutAnimationName);

		_donateButton.SetActive(false);

		base.AnimateOut(sequenceBuilder);
	}

	protected override void AfterAnimateOut()
	{
		base.AfterAnimateOut();

		_3dRoot.SetVisible(false);
		_bowlVisualContainer.SetVisible(false);

		foreach(DonationCoin coin in _donationCoins)
		{
			coin.QueueFree();
		}

		_donationCoins.Clear();
	}

	private DonationCoin CreateDonationCoin(float yOffset, float maxRandomOffset)
	{
		DonationCoin coin = _donationCoinScene.Instantiate<DonationCoin>();
		_donationCoinContainer.AddChild(coin);
		coin.SetGlobalPosition(
			_syncingBody.GlobalPosition + Vector3.Up * yOffset +
			maxRandomOffset * new Vector3(GD.Randf() * 2 - 1, 0.3f * GD.Randf(), GD.Randf() * 2 - 1));
		_donationCoins.Add(coin);
		return coin;
	}

	private void UpdateDonateButton()
	{
		SavedCharacter selectedCharacter = BetweenScenariosController.Instance.CharacterPortraitManager.SelectedPortrait?.SavedCharacter;
		_donateButton.SetActive(
			_donationButtonAvailable &&
			selectedCharacter != null &&
			BetweenScenariosController.Instance.SavedCampaign.SanctuaryOfTheGreatOak.CanDonate(selectedCharacter));
	}

	private void OnDonatePressed()
	{
		SavedSanctuaryOfTheGreatOak savedSanctuaryOfTheGreatOak = BetweenScenariosController.Instance.SavedCampaign.SanctuaryOfTheGreatOak;
		SavedCharacter selectedCharacter = BetweenScenariosController.Instance.CharacterPortraitManager.SelectedPortrait?.SavedCharacter;
		if(selectedCharacter == null || !savedSanctuaryOfTheGreatOak.CanDonate(selectedCharacter))
		{
			return;
		}

		savedSanctuaryOfTheGreatOak.Donate(selectedCharacter);

		for(int i = 0; i < 5; i++)
		{
			this.DelayedCall(() =>
			{
				DonationCoin coin = CreateDonationCoin(1f, 0.4f);
				coin.Visual.SetScale(0.01f * Vector3.One);
				coin.Visual.TweenScale(1f, 0.2f).SetEasing(Easing.OutBack).Play();
				coin.SetGlobalRotation(new Vector3(GD.Randf(), GD.Randf(), GD.Randf()) * Mathf.Tau);
			}, 0.04f * i + GD.Randf() * 0.04f);
		}

		this.DelayedCall(() =>
		{
			AppController.Instance.PopupManager.RequestPopup(new TemporaryAMDCardsPopup.Request()
			{
				Title = "Donation to the Sanctuary",
				Cards = selectedCharacter.DonationAMDCardIds.Select(ModelDB.GetById<AMDCardModel>).ToArray(),
				Receiver = selectedCharacter
			});
		}, 0.8f);

		UpdateDonateButton();
	}

	private void OnSelectedPortraitChanged(BetweenScenariosCharacterPortrait portrait)
	{
		UpdateDonateButton();
	}
}
using System.Collections.Generic;
using Godot;
using GTweens.Builders;

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

	protected override bool SelectCharacter => true;

	private List<DonationCoin> _donationCoins;

	public override void _Ready()
	{
		base._Ready();

		_3dRoot.SetVisible(false);
		_bowlVisualContainer.SetVisible(false);

		_donationCoins = _3dRoot.GetChildrenOfType<DonationCoin>();
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
						CreateDonationCoin();
					}
				}, 0.2f);
				_animationPlayer.Play(_moveInAnimationName);
			}))
			.AppendCallback(() =>
			{
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

	private void CreateDonationCoin()
	{
		DonationCoin coin = _donationCoinScene.Instantiate<DonationCoin>();
		_donationCoinContainer.AddChild(coin);
		coin.SetGlobalPosition(
			_syncingBody.GlobalPosition + Vector3.Up * 0.1f +
			0.2f * new Vector3(GD.Randf(), GD.Randf(), 0.3f * GD.Randf()));
		_donationCoins.Add(coin);
	}
}
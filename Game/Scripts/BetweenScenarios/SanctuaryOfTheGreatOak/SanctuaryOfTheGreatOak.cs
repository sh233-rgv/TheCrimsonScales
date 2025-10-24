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
			//.AppendTime(0.4f)
			.AppendCallback((() =>
			{
				this.DelayedCall(() =>
				{
					_3dRoot.SetVisible(true);
				});
				this.DelayedCall(() =>
				{
					_bowlVisualContainer.SetVisible(true);
					for(int i = 0; i < 3; i++)
					{
						CreateDonationCoin();
					}
				}, 0.1f);
				_animationPlayer.Play(_moveInAnimationName);
				// this.DelayedCall(() =>
				// {
				// 	_crystalBall.SetVisible(true);
				// });
			}))
			.AppendCallback(() =>
			{
				this.DelayedCall(() =>
				{
					foreach(DonationCoin coin in _donationCoins)
					{
						coin.Launch();
					}
				}, 0.8f);
			})
			.AppendTime(1f)
			.AppendCallback(() =>
			{
				// foreach(DonationCoin coin in _donationCoins)
				// {
				// 	coin.Launch();
				// }
			});
	}

	protected override void AfterAnimateIn()
	{
		base.AfterAnimateIn();
	}

	protected override void AnimateOut(GTweenSequenceBuilder sequenceBuilder)
	{
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
			_syncingBody.GlobalPosition + Vector3.Up * 0.05f +
			0.1f * new Vector3(GD.Randf(), GD.Randf(), 0.3f * GD.Randf()));
		_donationCoins.Add(coin);
	}
}
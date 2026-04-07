using System.Collections.Generic;
using Godot;
using GTweens.Builders;
using GTweens.Easings;
using GTweensGodot.Extensions;

public partial class MerchantsGuildHall : BetweenScenariosAction
{
	[Export]
	private Control _container;
	[Export]
	private PackedScene _rewardScene;
	[Export]
	private Control _leftParent;
	[Export]
	private Control _rightParent;

	[Export]
	private ExclamationMark _exclamationMark;

	private readonly List<MerchantsGuildHallReward> _rewards = new List<MerchantsGuildHallReward>();

	protected override bool SelectCharacter => false;

	public override void _Ready()
	{
		base._Ready();

		for(int i = 0; i < BetweenScenariosController.Instance.SavedCampaign.SavedMerchantsGuildHall.Rewards.Count; i++)
		{
			SavedMerchantsGuildHallReward savedReward = BetweenScenariosController.Instance.SavedCampaign.SavedMerchantsGuildHall.Rewards[i];
			MerchantsGuildHallReward reward = _rewardScene.Instantiate<MerchantsGuildHallReward>();
			Control parent = i < 7 ? _leftParent : _rightParent;
			parent.AddChild(reward);
			reward.Init(savedReward);
			_rewards.Add(reward);
		}
	}

	protected override void AnimateIn(GTweenSequenceBuilder sequenceBuilder, BetweenScenariosAction previousActiveAction)
	{
		base.AnimateIn(sequenceBuilder, previousActiveAction);

		_exclamationMark.SetActive(false);

		_container.SetPosition(new Vector2(0, -1000));

		sequenceBuilder
			.AppendTime(previousActiveAction is ItemShop ? 0.6f : 0.4f)
			.Append(_container.TweenPosition(Vector2.Zero, 0.6f).SetEasing(Easing.OutBack));
	}

	protected override void AnimateOut(GTweenSequenceBuilder sequenceBuilder)
	{
		sequenceBuilder
			.Append(_container.TweenPosition(new Vector2(0, -1000), 0.4f).SetEasing(Easing.InQuad));

		base.AnimateOut(sequenceBuilder);
	}
}
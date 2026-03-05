using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class PowerModulator : CSAddonQA
{
	public override string Name => "Power Modulator";
	public override int ItemNumber => 1;
	public override int ShopCount => 1;
	public override int Cost => 50;
	public override ItemType ItemType => ItemType.TwoHands;
	public override ItemUseType ItemUseType => ItemUseType.Consume;

	protected override int AtlasIndex => 3;

	protected override void Subscribe()
	{
		base.Subscribe();

		//TODO: wait for Brightspark to be merged (forgoing top action)
		SubscribeDuringTurn(
			canApply: character => character == Owner,
			apply: async character =>
			{
				await Use(async user =>
				{
					await GDTask.CompletedTask;
				});
			}
		);
	}
}
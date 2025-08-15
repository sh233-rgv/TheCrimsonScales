using Fractural.Tasks;

public class OrbOfDespair : CS1Item
{
	public override string Name => "Orb of Despair";
	public override int ItemNumber => 10;
	public override int ShopCount => 2;
	public override int Cost => 30;
	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Consume;
	public override int MaxUseCount => 3;

	protected override int AtlasIndex => 18;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringAttack(
			canApply: state => state.Performer == Owner,
			apply: async state =>
			{
				await Use(async user =>
				{
					state.SingleTargetAddCondition(Conditions.Curse);

					await GDTask.CompletedTask;
				});
			},
			canApplyMultipleTimesDuringAbility: false
		);
	}
}
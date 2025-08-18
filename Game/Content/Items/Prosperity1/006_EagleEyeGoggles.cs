using Fractural.Tasks;

public class EagleEyeGoggles : Prosperity1Item
{
	public override string Name => "Eagle-Eye Goggles";
	public override int ItemNumber => 6;
	public override int ShopCount => 2;
	public override int Cost => 30;
	public override ItemType ItemType => ItemType.Head;
	public override ItemUseType ItemUseType => ItemUseType.Spend;

	protected override int AtlasIndex => 10;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringAttack(
			canApply: state => state.Performer == Owner,
			apply: async state =>
			{
				await Use(async user =>
				{
					state.AbilitySetHasAdvantage();

					await GDTask.CompletedTask;
				});
			}
		);
	}
}
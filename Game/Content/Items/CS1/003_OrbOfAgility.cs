using Fractural.Tasks;

public class OrbOfAgility : CS1Item
{
	public override string Name => "Orb of Agility";
	public override int ItemNumber => 3;
	public override int ShopCount => 2;
	public override int Cost => 15;
	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Consume;
	public override int MaxUseCount => 3;

	protected override int AtlasIndex => 4;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringMove(
			canApply: state => state.Performer == Owner,
			apply: async state =>
			{
				await Use(async user =>
				{
					state.AddJump();
					//state.AdjustMoveType(MoveType.Jump);

					await GDTask.CompletedTask;
				});
			}
		);
	}
}
using System.Linq;
using Fractural.Tasks;

public class FalconFigurine : Prosperity4Item
{
	public override string Name => "Falcon Figurine";
	public override int ItemNumber => 35;
	public override int ShopCount => 2;
	public override int Cost => 50;
	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Consume;
	public override bool Persistent => true;

	protected override int AtlasIndex => 14;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringTurn(
			canApply: character =>
				character == Owner &&
				character.Cards.Count(card => card.CardState == CardState.Discarded) > 0,
			apply: async character =>
			{
				await Use(async user =>
				{
					ActionState actionState = new ActionState(this, user,
						[
							SummonAbility.Builder()
								.WithName("Jade Falcon")
								.WithTexturePath("res://Content/Items/Prosperity4/JadeFalcon.tres")
								.WithHealth(2)
								.WithMove(3)
								.WithAttack(2)
								.Build()
						],
						onDiscardOrLoseRequested: async state =>
						{
							await GDTask.CompletedTask;
						}
					);
					await actionState.Perform();
				});
			}
		);
	}
}
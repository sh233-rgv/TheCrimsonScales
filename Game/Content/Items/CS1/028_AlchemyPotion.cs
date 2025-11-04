using System.Data;
using System.Linq;

public class AlchemyPotion : CS1Item
{
	public override string Name => "AlchemyPotion";
	public override int ItemNumber => 28;
	public override int ShopCount => 2;
	public override int Cost => 20;
	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Consume;

	protected override int AtlasIndex => 47;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringTurn(
			canApply: character => character == Owner,
			apply: async character =>
			{
				await Use(async user =>
				{
					Hex hex = await AbilityCmd.SelectHex(character, list =>
						list.AddRange(RangeHelper.GetHexesInRange(character.Hex, 1)
							.Where(hex => hex.HasHexObjectOfType<Obstacle>())));
                 
					if(hex == null)
					{
						return;
					}
					if(await AbilityCmd.DestroyObstacle(hex.GetHexObjectOfType<Obstacle>()))
                    {
						await AbilityCmd.SpawnCoin(hex);
                    }
				});
			}
		);
	}
}
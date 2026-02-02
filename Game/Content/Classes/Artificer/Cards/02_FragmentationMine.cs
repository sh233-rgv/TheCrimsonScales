using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class FragmentationMine : ArtificerCardModel<FragmentationMine.CardTop, FragmentationMine.CardBottom>
{
	public override string Name => "Fragmentation Mine";
	public override int Level => 1;
	public override int Initiative => 45;
	protected override int AtlasIndex => 2;

	public class CardTop : ArtificerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(CreateTrapAbility.Builder()
				.WithDamage(3)
				.WithRange(2)
				.WithAbilityStartedSubscription(
					ScenarioEvents.AbilityStarted.Subscription.New())
				.Build())
		];
	}

	public class CardBottom : ArtificerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3, new MoveCircle(this, new Vector2(0.62138146f, 0.70201635f)))
				.Build()),
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					//TODO: Change to directly selecting the overlay tile
					Hex hex = await AbilityCmd.SelectHex(state,
						list => list.AddRange(RangeHelper.GetHexesInRange(state.Performer.Hex, 1).Where(hex =>
							hex.HasHexObjectOfType<Trap>() || (hex.TryGetHexObjectOfType(out Obstacle obstacle) &&
							                                   obstacle.HexObjectShape == HexObjectShape.Single && !obstacle.CannotBeDestroyed))),
						hintText: "Select a trap or 1-hex obstacle to destroy");
					if(hex == null)
					{
						return;
					}

					if(hex.TryGetHexObjectOfType(out Obstacle obstacle))
					{
						await obstacle.Destroy();
					}
					else
					{
						await hex.GetHexObjectOfType<Trap>().Destroy();
					}
					
					await GainScrapToken(state);
					await AbilityCmd.GainXP(state.Performer, 1);
					state.SetPerformed();
				})
				.Build())
		];
	}
}
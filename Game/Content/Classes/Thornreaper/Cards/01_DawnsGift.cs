using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class DawnsGift : ThornreaperCardModel<DawnsGift.CardTop, DawnsGift.CardBottom>
{
	public override string Name => "Dawn's Gift";
	public override int Level => 1;
	public override int Initiative => 56;
	protected override int AtlasIndex => 1;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(CreateThornsAbilityBuilder()
				.WithRange(1)
				.Build()),
			new AbilityCardAbility(LootAbility.Builder()
				.WithRange(1)
				.WithAbilityStartedSubscription(
					ScenarioEvents.AbilityStarted.Subscription.New(
						parameters => LightStrongOrWaning && parameters.AbilityState.ActionState
							.GetAbilityState<CreateOverlayTileAbility<ThornsThornreaper>.State>(0).Performed,
						async parameters =>
						{
							((LootAbility.State)parameters.AbilityState).SetPerformHex(parameters.AbilityState.ActionState
								.GetAbilityState<CreateOverlayTileAbility<ThornsThornreaper>.State>(0).CreatedOverlayTiles[0].Hex);
							await GDTask.CompletedTask;
						}, EffectType.Selectable,
						effectButtonParameters: new IconEffectButton.Parameters("res://Content/Classes/Thornreaper/toa-thorns.png"),
						effectInfoViewParameters: new TextEffectInfoView.Parameters(
							"Perform the loot as if you were occupying the hex with the created hazardous terrain"))
				)
				.WithOnAbilityStarted(async state =>
				{
					if(LightStrongOrWaning)
					{
						await AbilityCmd.InfuseElement(state, Element.Earth);
					}
				})
				.Build())
		];
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(2, new MoveSquare(this, new Vector2(0.62163085f, 0.72102964f)))
				.Build())
		];

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Light)];
	}
}
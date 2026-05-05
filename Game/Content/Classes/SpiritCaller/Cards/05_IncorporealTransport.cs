using System.Collections.Generic;

public class IncorporealTransport : SpiritCallerCardModel<IncorporealTransport.CardTop, IncorporealTransport.CardBottom>
{
	public override string Name => "Incorporeal Transport";
	public override int Level => 1;
	public override int Initiative => 74;
	protected override int AtlasIndex => 28 - 5;

	public class CardTop : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SpawnAbility.Builder()
				.WithName("Ghost Carriage")
				.WithTexturePath("res://Content/Classes/SpiritCaller/Summons/ghost_carriage.png")
				.WithHealth(2)
				.WithMove(4)
				.WithTraits(new ForceMoveAlongTrait())
				.Build()
			)
		];

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Air)];
		public override int XP => 1;
		public override bool Persistent => true;
	}

	public class CardBottom : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(4)
				.Build()),

			new AbilityCardAbility(GrantAbility.Builder()
				.WithAbilities(MoveAbility.Builder().WithDistance(1).Build())
				.WithCustomGetTargets((state, list) =>
				{
					MoveAbility.State moveAbilityState = state.ActionState.GetAbilityState<MoveAbility.State>(0);
					foreach(Hex hex in moveAbilityState.Hexes)
					{
						foreach(Figure figure in hex.GetHexObjectsOfType<Figure>())
						{
							list.Add(figure);
						}
					}
				})
				.WithTarget(Target.Any | Target.TargetAll)
				.WithCanTargetNonFigures()
				.WithConditionalAbilityCheck(async state =>
				{
					if(!await AbilityCmd.HasPerformedAbility(state, 0))
					{
						return false;
					}

					bool hasPotentialTarget = false;
					MoveAbility.State moveAbilityState = state.ActionState.GetAbilityState<MoveAbility.State>(0);
					foreach(Hex hex in moveAbilityState.Hexes)
					{
						foreach(Figure figure in hex.GetHexObjectsOfType<Figure>())
						{
							hasPotentialTarget = true;
							break;
						}
					}

					if(!hasPotentialTarget)
					{
						return false;
					}

					if(!await AbilityCmd.AskConsumeElement(state.Performer, Element.Air))
					{
						return false;
					}

					return true;
				})
				.Build())
		];
	}
}
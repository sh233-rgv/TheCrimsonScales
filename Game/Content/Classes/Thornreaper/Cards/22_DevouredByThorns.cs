using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;
using Range = Godot.Range;

public class DevouredByThorns : ThornreaperCardModel<DevouredByThorns.CardTop, DevouredByThorns.CardBottom>
{
	public override string Name => "Devoured by Thorns";
	public override int Level => 6;
	public override int Initiative => 72;
	protected override int AtlasIndex => 22;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2, new AttackSquare(this, new Vector2(0.34612656f, 0.3157895f)))
				.WithTargets(3)
				.WithRange(3, new RangeSquare(this, new Vector2(0.6472411f, 0.31458148f)))
				.WithPull(2)
				.Build())
		];

		public override Func<Figure, GDTask<bool>> OnCardSideStarted => ActionConsumeEarth;

		public override int XP => 1;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(PullAbility.Builder()
				.WithPull(2)
				.WithRange(2)
				.WithConditionalAbilityCheck(async state =>
				{
					Hex hex = await AbilityCmd.SelectHex(state,
						hexes => hexes.AddRange(GameController.Instance.Map.GetChildrenOfType<HazardousTerrain>()
							.SelectMany(hazardousTerrain => hazardousTerrain.Hexes)), true);

					state.SetCustomValue(this, "PerformHex", hex);
					return hex != null;
				})
				.WithCustomGetPerformHex(state => state.GetCustomValue<Hex>(this, "PerformHex"))
				.Build()),
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					List<Figure> figures = state.ActionState.GetAbilityState<PullAbility.State>(0).UniqueTargetedFigures
						.Where(target => target.IsDead).ToList();
					foreach(Figure figure in figures)
					{
						await AbilityCmd.LootHex(state.Performer, figure.Hex);
						await AbilityCmd.GainXP(state.Performer, 1);
					}
				})
				.Build())
		];

		public override bool Round => true;
	}
}
using System.Collections.Generic;
using Fractural.Tasks;

public class WhistlingWinds : SpiritCallerCardModel<WhistlingWinds.CardTop, WhistlingWinds.CardBottom>
{
	public override string Name => "Whistling Winds";
	public override int Level => 1;
	public override int Initiative => 82;
	protected override int AtlasIndex => 28 - 9;

	public class CardTop : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3)
				.WithDuringAttackSubscriptions(
				[
					ScenarioEvents.DuringAttack.Subscription.ConsumeElement(Element.Air,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AbilityAdjustPush(2);

							await GDTask.CompletedTask;
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.Push)}2")
					),
					ScenarioEvents.DuringAttack.Subscription.ConsumeElement(Element.Dark,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AbilityAddCondition(Conditions.Curse);

							await GDTask.CompletedTask;
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.GetCondition(Conditions.Curse))}")
					)
				])
				.WithCustomGetPerformHex(state => state.GetCustomValue<Hex>(this, "Hex"))
				.WithConditionalAbilityCheck(async state =>
				{
					Figure figure = await AbilityCmd.SelectFigure(state, list =>
					{
						foreach(Figure figure in GameController.Instance.Map.Figures)
						{
							if(figure is Spirit)
							{
								list.Add(figure);
							}
						}
					}, hintText: () => $"Choose a Spirit");

					if(figure == null)
					{
						return false;
					}

					state.SetCustomValue(this, "Hex", figure.Hex);
					return true;
				})
				.Build()),
		];
	}

	public class CardBottom : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(2)
				.Build()),

			new AbilityCardAbility(GrantAbility.Builder()
				.WithAbilities([
					MoveAbility.Builder()
						.WithDistance(1)
						.Build()
				])
				.WithCustomGetTargets((state, list) =>
				{
					foreach(Figure figure in GameController.Instance.Map.Figures)
					{
						if(figure is Spirit spirit)
						{
							list.Add(spirit);
						}
					}
				})
				.WithTarget(Target.Any | Target.TargetAll)
				.WithCanTargetNonFigures()
				.Build()),
		];

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Dark)];
	}
}
using System.Collections.Generic;
using Fractural.Tasks;

public class EtherealCanine : SpiritCallerCardModel<EtherealCanine.CardTop, EtherealCanine.CardBottom>
{
	public override string Name => "Ethereal Canine";
	public override int Level => 1;
	public override int Initiative => 27;
	protected override int AtlasIndex => 28 - 3;

	public class CardTop : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SpawnAbility.Builder()
				.WithName("Phantom Hound")
				.WithTexturePath("res://Content/Classes/SpiritCaller/Summons/phantom_hound.png")
				.WithHealth(1)
				.WithMove(3)
				.WithAttack(2)
				.WithTraits(new SetAttackCustomTargetsTrait((state, list) =>
				{
					foreach(AbilityState abilityState in state.ActionState.AbilityStates)
					{
						if(abilityState is MoveAbility.State moveAbilityState && moveAbilityState.Performed)
						{
							foreach(Hex hex in moveAbilityState.Hexes)
							{
								foreach(Figure figure in hex.GetFigures())
								{
									if(moveAbilityState.Performer.EnemiesWith(figure))
									{
										list.Add(figure);
									}
								}
							}
						}
					}
				}))
				.Build()
			)
		];

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Dark)];
		public override int XP => 1;
		public override bool Persistent => true;
	}

	public class CardBottom : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					Spirit spirit = state.GetCustomValue<Spirit>(this, "Spirit");
					await spirit.AddDamageCounters(1);

					if(spirit.IsDead)
					{
						return;
					}

					bool consumedDark = await AbilityCmd.AskConsumeElement(state.Performer, Element.Dark);
					int attackBonus = consumedDark ? 3 : 2;

					if(consumedDark)
					{
						await GainXP(state);
					}

					await AbilityCmd.AddCharacterToken(state, spirit,
						textParameters =>
							$"This Spirit adds +{attackBonus}{Icons.Inline(Icons.Attack, textParameters)} to its next attack this round.");

					ScenarioEvents.AttackAfterTargetConfirmedEvent.Subscribe(state, this,
						parameters =>
							parameters.AbilityState.Performer == spirit,
						async parameters =>
						{
							parameters.AbilityState.AbilityAdjustAttackValue(attackBonus);

							await state.ActionState.RequestDiscardOrLose();
						}
					);
				})
				.WithOnDeactivate(async state =>
				{
					Spirit spirit = state.GetCustomValue<Spirit>(this, "Spirit");

					await AbilityCmd.RemoveCharacterToken(state, spirit);

					ScenarioEvents.AttackAfterTargetConfirmedEvent.Unsubscribe(state, this);
				})
				.WithSkipConfirmation()
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

					state.SetCustomValue(this, "Spirit", (Spirit)figure);
					return true;
				})
				.Build())
		];

		public override bool Round => true;
	}
}
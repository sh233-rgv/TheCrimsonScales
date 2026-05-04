using System.Collections.Generic;
using Fractural.Tasks;

public class ConfidenceRitual : SpiritCallerCardModel<ConfidenceRitual.CardTop, ConfidenceRitual.CardBottom>
{
	public override string Name => "Confidence Ritual";
	public override int Level => 1;
	public override int Initiative => 30;
	protected override int AtlasIndex => 28 - 1;

	public class CardTop : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(new DynamicInt<AttackAbility.State>(state =>
					3 + (state.Performer is Character characterOwner ? Spirit.GetSpirits(characterOwner).Count : 0)))
				.WithRange(2)
				.Build()),
		];
	}

	public class CardBottom : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					Spirit spirit = state.GetCustomValue<Spirit>(this, "Spirit");
					await spirit.RemoveDamageCounters(1);

					await AbilityCmd.AddCharacterToken(state, spirit,
						textParameters =>
							$"While this Spirit is alive, its owner adds +1{Icons.Inline(Icons.Attack, textParameters)} and advantage to all their attacks.");

					ScenarioEvents.AbilityStartedEvent.Subscribe(state, this,
						parameters =>
							parameters.AbilityState is AttackAbility.State &&
							parameters.AbilityState.Performer == spirit.CharacterOwner,
						async parameters =>
						{
							AttackAbility.State attackAbilityState = (AttackAbility.State)parameters.AbilityState;
							attackAbilityState.AbilityAdjustAttackValue(1);
							attackAbilityState.AbilitySetHasAdvantage();

							await GDTask.CompletedTask;
						}
					);

					ScenarioEvents.HexObjectDestroyedEvent.Subscribe(state, this,
						parameters => parameters.HexObject == spirit,
						async parameters =>
						{
							await state.ActionState.RequestDiscardOrLose();
						}
					);
				})
				.WithOnDeactivate(async state =>
				{
					Spirit spirit = state.GetCustomValue<Spirit>(this, "Spirit");

					await AbilityCmd.RemoveCharacterToken(state, spirit);

					ScenarioEvents.AbilityStartedEvent.Unsubscribe(state, this);
					ScenarioEvents.HexObjectDestroyedEvent.Unsubscribe(state, this);
				})
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

		public override int XP => 2;
		public override bool Persistent => true;
		public override bool Loss => true;
	}
}
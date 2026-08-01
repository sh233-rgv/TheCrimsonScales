using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class NaturesFury : ThornreaperCardModel<NaturesFury.CardTop, NaturesFury.CardBottom>
{
	public override string Name => "Nature's Fury";
	public override int Level => 7;
	public override int Initiative => 37;
	protected override int AtlasIndex => 24;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.FinishElementInfusedEvent.Subscribe(state, this,
						parameters => parameters.);

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.AttackAfterTargetConfirmedEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.Build())
		];

		public override int XP => 2;
		public override bool Persistent => true;
		public override bool Loss => true;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					await AbilityCmd.InfuseElement(state, Element.Earth);
					state.SetPerformed();
				})
				.WithConditionalAbilityCheck(async _ =>
				{
					await GDTask.CompletedTask;

					return GameController.Instance.ElementManager.GetState(Element.Earth) is ElementState.Strong or ElementState.Waning;
				})
				.Build()),
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3, new MoveSquare(this, new Vector2(0.6214308f, 0.8083103f)))
				.Build())
		];
	}
}
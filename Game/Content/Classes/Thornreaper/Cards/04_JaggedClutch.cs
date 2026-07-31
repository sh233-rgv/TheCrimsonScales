using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class JaggedClutch : ThornreaperCardModel<JaggedClutch.CardTop, JaggedClutch.CardBottom>
{
	public override string Name => "Jagged Clutch";
	public override int Level => 1;
	public override int Initiative => 32;
	protected override int AtlasIndex => 4;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(4, new AttackSquare(this, new Vector2(0.39279062f, 0.24497536f)))
				.WithRange(3, new RangeSquare(this, new Vector2(0.6037813f, 0.24376732f)))
				.WithPull(1)
				.Build()),
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					foreach(SingleTargetState singleTargetState in state.ActionState.GetAbilityState<AttackAbility.State>(0).SingleTargetStates)
					{
						if(singleTargetState.PullHexes.Any(hex => hex.HasHexObjectOfType<HazardousTerrain>()))
						{
							await AbilityCmd.AddCondition(state, singleTargetState.Target, Conditions.Immobilize);
							state.SetPerformed();
						}
					}
				})
				.WithConditionalAbilityCheck(state => AbilityCmd.HasPerformedAbility(state, 0))
				.Build())
		];

		public override Func<Figure, GDTask<bool>> OnCardSideStarted => ActionConsumeEarth;

		public override int XP => 1;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3, new MoveSquare(this, new Vector2(0.6221069f, 0.6727762f)))
				.WithAbilityPerformedSubscription(
					ScenarioEvents.AbilityPerformed.Subscription.ConsumeElement(Element.Earth,
						applyFunction: async parameters =>
						{
							await AbilityCmd.InfuseElement(parameters.AbilityState, Element.Earth);
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters(Icons.Inline(Icons.GetElement(Element.Earth)))))
				.Build()),
			new AbilityCardAbility(SufferDamageAbility.Builder()
				.WithDamage(1)
				.WithRange(1)
				.Build())
		];
	}
}
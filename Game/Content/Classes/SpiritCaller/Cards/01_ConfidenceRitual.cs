using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

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
				.Build()
			),
		];
	}

	public class CardBottom : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3, new MoveCircle(this, new Vector2(0.6224222f, 0.723211f))) // TODO
				.WithDuringMovementSubscription(
					ScenarioEvents.DuringMovement.Subscription.ConsumeElement(Element.Dark,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AdjustMoveValue(1);
							parameters.AbilityState.AdjustMoveType(MoveType.Jump);

							await GDTask.CompletedTask;
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"+1{Icons.Inline(Icons.Move)}, {Icons.Inline(Icons.Jump)}")
					)
				)
				.Build()),

			new AbilityCardAbility(SufferDamageAbility.Builder()
				.WithDamage(1)
				.WithCustomGetTargets((state, list) =>
				{
					foreach(Figure figure in GameController.Instance.Map.Figures)
					{
						if(figure is Spirit spirit)
						{
							foreach(Figure otherFigure in spirit.Hex.GetHexObjectsOfType<Figure>())
							{
								list.Add(otherFigure);
							}
						}
					}
				})
				.Build())
		];
	}
}
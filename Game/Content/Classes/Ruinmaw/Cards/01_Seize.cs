using System.Collections.Generic;
using Fractural.Tasks;
using Godot;
using GTweens.Easings;

public class Seize : RuinmawCardModel<Seize.CardTop, Seize.CardBottom>
{
	public override string Name => "Seize";
	public override int Level => 1;
	public override int Initiative => 29;
	protected override int AtlasIndex => 1;

	public class CardTop : RuinmawCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(ConditionAbility.Builder()
				.WithConditions(Conditions.Disarm)
				.WithRange(1)
				.Build()),
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					ConditionAbility.State conditionAbilityState = state.ActionState.GetAbilityState<ConditionAbility.State>(0);
					

					if (conditionAbilityState.UniqueTargetedFigures.Count > 0)
					{
						PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/Scenario/CoinStack.tscn");
						CoinStack coinStack = scene.Instantiate<CoinStack>();
						GameController.Instance.Map.AddChild(coinStack);
						await coinStack.Init(conditionAbilityState.UniqueTargetedFigures[0].Hex);

						coinStack.ZIndex = 100;
						coinStack.TweenGlobalJump(state.Performer.Hex.GlobalPosition, 0.5f * Map.HexSize, 0.3f).PlayFastForwardable();
						coinStack.TweenScale(0f, 0.35f).SetEasing(Easing.InBack).PlayFastForwardable();

						await GDTask.DelayFastForwardable(0.3f);

						await coinStack.Destroy();

						((Character)state.Performer).AddCoin();
                    }

					await GDTask.CompletedTask;
				})
				.Build())
		];
	}

	public class CardBottom : RuinmawCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder().WithDistance(2).Build()),
			new AbilityCardAbility(PullAbility.Builder()
				.WithPull(1)
				.WithRange(2)
				.WithConditions(Conditions.Poison1)
				.Build()),
			new AbilityCardAbility(ConditionAbility.Builder()
				.WithConditions(Conditions.Rupture)
				.WithTarget(Target.Self)
				.WithMandatory(true)
				.Build())
		];
	}
}
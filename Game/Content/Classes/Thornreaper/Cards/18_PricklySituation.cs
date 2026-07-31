using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;
using Range = Godot.Range;

public class PricklySituation : ThornreaperCardModel<PricklySituation.CardTop, PricklySituation.CardBottom>
{
	public override string Name => "Prickly Situation";
	public override int Level => 4;
	public override int Initiative => 20;
	protected override int AtlasIndex => 18;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(6, new AttackSquare(this, new Vector2(0.45011973f, 0.21772854f)))
				.WithRange(2, new RangeSquare(this, new Vector2(0.6598823f, 0.21717452f)))
				.WithAfterAttackPerformedSubscription(
					ScenarioEvents.AfterAttackPerformed.Subscription.New(
						applyFunction: async parameters =>
						{
							Hex hex = parameters.Performer.Hex;
							List<Hex> path = [];
							for(int i = RangeHelper.Distance(parameters.AbilityState.Target.Hex, parameters.Performer.Hex) - 1; i >= 0; i++)
							{
								hex = await AbilityCmd.SelectHex(parameters.AbilityState,
									hexes => hexes.AddRange(RangeHelper.GetHexesInRange(hex, 1, false).Where(possibleHex =>
										RangeHelper.Distance(possibleHex, parameters.AbilityState.Target.Hex) == i)), true,
									"Select the next hex in the path");
								path.Add(hex);
							}

							foreach(Hex hexInPath in path)
							{
								if(hexInPath.IsFeatureless())
								{
									await AbilityCmd.CreateOverlayTile<ThornsThornreaper>(hexInPath,
										SceneLoader.LoadPackedScene("res://Content/OverlayTiles/HazardousTerrain/HotCoals1H.tscn"));
								}
							}
						}))
				.Build())
		];

		public override Func<Figure, GDTask<bool>> OnCardSideStarted => ActionConsumeEarth;

		public override int XP => 1;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(InfuseElementIfLightAbility(Element.Earth)),
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(2, new MoveSquare(this, new Vector2(0.62153083f, 0.6804786f)))
				.Build()),
			new AbilityCardAbility(ShieldAbility.Builder()
				.WithShieldValue(1)
				.Build()),
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioCheckEvents.ShieldCheckEvent.Subscribe(state, this,
						parameters => parameters.Figure == state.Performer && LightStrongOrWaning,
						parameters =>
						{
							parameters.AdjustShield(1);
						}
					);

					ScenarioEvents.SufferDamageEvent.Subscribe(state, this,
						parameters => parameters.Figure == state.Performer && parameters.FromAttack && LightStrongOrWaning,
						async parameters =>
						{
							parameters.AdjustShield(1);

							await GDTask.CompletedTask;
						}
					);

					ScenarioEvents.FinishElementConsumedEvent.Subscribe(state, this,
						canApplyParameters => canApplyParameters.ConsumedElement == Element.Light,
						async _ =>
						{
							ScenarioCheckEvents.ShieldCheckEvent.FireChangedEvent();
							await GDTask.CompletedTask;
						}, EffectType.Visuals);

					ScenarioEvents.FinishElementInfusedEvent.Subscribe(state, this,
						canApplyParameters => canApplyParameters.InfusedElement == Element.Light,
						async _ =>
						{
							ScenarioCheckEvents.ShieldCheckEvent.FireChangedEvent();

							await GDTask.CompletedTask;
						}, EffectType.Visuals);

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioCheckEvents.ShieldCheckEvent.Unsubscribe(state, this);
					ScenarioEvents.SufferDamageEvent.Unsubscribe(state, this);
					ScenarioEvents.FinishElementConsumedEvent.Unsubscribe(state, this);
					ScenarioEvents.FinishElementInfusedEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.Build())
		];

		public override bool Round => true;
	}
}
using System;
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
					bool addedPlusOne = false;
					ScenarioEvents.ElementInfusedEvent.Subscribe(state, this,
						parameters => !addedPlusOne && parameters.PotentialInfuser == state.Performer && parameters.Element == Element.Earth,
						async _ =>
						{
							addedPlusOne = true;

							await GDTask.CompletedTask;
						});

					ScenarioEvents.DuringAttackEvent.Subscribe(state, this,
						parameters => addedPlusOne && parameters.Performer == state.Performer,
						async parameters =>
						{
							parameters.AbilityState.SingleTargetAdjustAttackValue(1);

							await GDTask.CompletedTask;
						});

					ScenarioEvents.RoundEndedEvent.Subscribe(state, this,
						_ => addedPlusOne,
						async _ =>
						{
							addedPlusOne = false;

							await GDTask.CompletedTask;
						});

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.ElementInfusedEvent.Unsubscribe(state, this);
					ScenarioEvents.DuringAttackEvent.Unsubscribe(state, this);
					ScenarioEvents.RoundEndedEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.Build())
		];

		public override int XP => 1;
		public override bool Persistent => true;
		public override bool Loss => true;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(4, new AttackSquare(this, new Vector2(0.38968638f, 0.6709142f)))
				.WithRange(4, new RangeSquare(this, new Vector2(0.600501f, 0.6699601f)))
				.WithPull(3)
				.Build()),
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					ScenarioEvents.AbilityCardSideStartedEvent.Subscribe(state, this,
						parameters => parameters.Performer == state.Performer,
						async _ =>
						{
							ScenarioEvents.ConsumeElementEvent.Subscribe(state, this,
								parameters => !parameters.Consumed && parameters.Elements.Contains(Element.Earth),
								async parameters =>
								{
									parameters.SetConsumed(null);
									await GDTask.CompletedTask;
								});

							await GDTask.CompletedTask;
						}, order: -1000);

					object subscriber = new object();
					ScenarioEvents.AbilityCardSideStartedEvent.Subscribe(state, subscriber,
						parameters => parameters.Performer == state.Performer,
						async _ =>
						{
							ScenarioEvents.ConsumeElementEvent.Unsubscribe(state, this);

							await GDTask.CompletedTask;
						}, order: 1000);

					ScenarioEvents.FigureTurnEndedEvent.Subscribe(state, this,
						parameters => parameters.Figure == state.Performer,
						async _ =>
						{
							ScenarioEvents.AbilityCardSideStartedEvent.Unsubscribe(state, this);
							ScenarioEvents.AbilityCardSideStartedEvent.Unsubscribe(state, subscriber);
							ScenarioEvents.FigureTurnEndedEvent.Unsubscribe(state, this);

							await GDTask.CompletedTask;
						});

					await GDTask.CompletedTask;
				})
				.Build())
		];

		public override Func<Figure, GDTask<bool>> OnCardSideStarted => ActionConsumeEarth;

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Light)];
	}
}
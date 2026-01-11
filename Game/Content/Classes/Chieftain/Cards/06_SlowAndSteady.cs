using System.Collections.Generic;
using Fractural.Tasks;

public class SlowAndSteady : ChieftainCardModel<SlowAndSteady.CardTop, SlowAndSteady.CardBottom>
{
	public override string Name => "Slow and steady";
	public override int Level => 1;
	public override int Initiative => 93;
	protected override int AtlasIndex => 6;

	public class CardTop : ChieftainCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SummonAbility.Builder()
				.WithName("Giant Tortoise")
				.WithTexturePath("res://Content/Classes/Chieftain/Summons/giant_tortoise_AI.png")
				.WithHealth(6)
				.WithMove(1)
				.WithAttack(1)
				.WithTraits(
					new ShieldTrait(1),
					new MountTrait(
						async (owner, mount) =>
						{
							ScenarioCheckEvents.ImmuneToForcedMovementCheckEvent.Subscribe(mount, this,
								canApply: parameters =>
									parameters.Figure == owner ||
									parameters.Figure == mount,
								parameters =>
								{
									parameters.SetImmuneToForcedMovement();
								}
							);
							await GDTask.CompletedTask;
						},
						async (owner, mount) =>
						{
							ScenarioCheckEvents.ImmuneToForcedMovementCheckEvent.Unsubscribe(mount, this);

							await GDTask.CompletedTask;
						}
					)
				)
				.Build()
			),
		];

		public override int XP => 2;
		public override bool Persistent => true;
		public override bool Loss => true;
	}

	public class CardBottom : ChieftainCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder().WithDamage(2).Build()),
		];
	}
}
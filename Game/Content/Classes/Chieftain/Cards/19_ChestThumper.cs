using System.Collections.Generic;

public class ChestThumper : ChieftainCardModel<ChestThumper.CardTop, ChestThumper.CardBottom>
{
	public override string Name => "Chest Thumper";
	public override int Level => 5;
	public override int Initiative => 94;
	protected override int AtlasIndex => 19;

	public class CardTop : ChieftainCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SummonAbility.Builder()
				.WithSummonStats(new SummonStats()
				{
					Health = 7,
					Move = 3,
					Attack = 2,
					Traits =
					[
						new JumpTrait(),
						new HealOnKillTrait(2),
						new MountTrait(),
					]
				})
				.WithName("Lowland Gorilla")
				.WithTexturePath("res://Content/Classes/Chieftain/Summons/lowland_gorilla_AI.png")
				.Build()
			),
		];

		public override int XP => 2;
		public override bool Persistent => true;
		public override bool Loss => true;
		public override bool Unrecoverable => true;
	}

	public class CardBottom : ChieftainCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(ConditionAbility.Builder()
				.WithConditions(Conditions.Strengthen)
				.WithTarget(Target.Allies | Target.TargetAll)
				.WithRange(2)
				.Build()
			),
		];
	}
}
using System.Collections.Generic;

public class SpittingDrake : MonsterModel
{
	public override MonsterStats[] NormalLevelStats =>
	[
		new MonsterStats()
		{
			Health = 5,
			Move = 3,
			Attack = 3,
			Range = 3,
			Traits = [new FlyingTrait()]
		},
		new MonsterStats()
		{
			Health = 6,
			Move = 3,
			Attack = 3,
			Range = 3,
			Traits = [new FlyingTrait()]
		},
		new MonsterStats()
		{
			Health = 8,
			Move = 3,
			Attack = 3,
			Range = 3,
			Traits = [new FlyingTrait(), new ApplyConditionTrait(Conditions.Muddle)]
		},
		new MonsterStats()
		{
			Health = 8,
			Move = 3,
			Attack = 4,
			Range = 4,
			Traits = [new FlyingTrait(), new ApplyConditionTrait(Conditions.Muddle)]
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 4,
			Attack = 4,
			Range = 4,
			Traits = [new FlyingTrait(), new ApplyConditionTrait(Conditions.Muddle)]
		},
		new MonsterStats()
		{
			Health = 12,
			Move = 4,
			Attack = 4,
			Range = 4,
			Traits = [new FlyingTrait(), new ApplyConditionTrait(Conditions.Muddle)]
		},
		new MonsterStats()
		{
			Health = 13,
			Move = 4,
			Attack = 5,
			Range = 4,
			Traits = [new FlyingTrait(), new ApplyConditionTrait(Conditions.Muddle)]
		},
		new MonsterStats()
		{
			Health = 16,
			Move = 4,
			Attack = 5,
			Range = 4,
			Traits = [new FlyingTrait(), new ApplyConditionTrait(Conditions.Muddle)]
		},
	];

	public override MonsterStats[] EliteLevelStats =>
	[
		new MonsterStats()
		{
			Health = 8,
			Move = 3,
			Attack = 4,
			Range = 4,
			Traits = [new FlyingTrait()]
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 3,
			Attack = 4,
			Range = 4,
			Traits = [new FlyingTrait(), new ApplyConditionTrait(Conditions.Muddle)]
		},
		new MonsterStats()
		{
			Health = 10,
			Move = 3,
			Attack = 5,
			Range = 4,
			Traits = [new FlyingTrait(), new ApplyConditionTrait(Conditions.Muddle)]
		},
		new MonsterStats()
		{
			Health = 12,
			Move = 3,
			Attack = 5,
			Range = 5,
			Traits = [new FlyingTrait(), new ApplyConditionTrait(Conditions.Muddle)]
		},
		new MonsterStats()
		{
			Health = 14,
			Move = 4,
			Attack = 5,
			Range = 5,
			Traits = [new FlyingTrait(), new ApplyConditionTrait(Conditions.Muddle)]
		},
		new MonsterStats()
		{
			Health = 16,
			Move = 4,
			Attack = 6,
			Range = 5,
			Traits = [new FlyingTrait(), new ApplyConditionTrait(Conditions.Muddle)]
		},
		new MonsterStats()
		{
			Health = 19,
			Move = 4,
			Attack = 6,
			Range = 5,
			Traits = [new FlyingTrait(), new ApplyConditionTrait(Conditions.Muddle)]
		},
		new MonsterStats()
		{
			Health = 21,
			Move = 4,
			Attack = 7,
			Range = 5,
			Traits = [new FlyingTrait(), new ApplyConditionTrait(Conditions.Muddle)]
		},
	];

	public override string Name => "Spitting Drake";

	public override string AssetPath => "res://Content/Monsters/SpittingDrake";

	public override int MaxStandeeCount => 6;

	public override IEnumerable<MonsterAbilityCardModel> Deck => SpittingDrakeAbilityCard.Deck;
}
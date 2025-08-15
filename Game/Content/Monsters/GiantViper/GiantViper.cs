using System.Collections.Generic;

public class GiantViper : MonsterModel
{
	public override MonsterStats[] NormalLevelStats =>
	[
		new MonsterStats()
		{
			Health = 2,
			Move = 2,
			Attack = 1,
			Traits = [new ApplyConditionTrait(Conditions.Poison1)]
		},
		new MonsterStats()
		{
			Health = 3,
			Move = 2,
			Attack = 1,
			Traits = [new ApplyConditionTrait(Conditions.Poison1)]
		},
		new MonsterStats()
		{
			Health = 4,
			Move = 3,
			Attack = 1,
			Traits = [new ApplyConditionTrait(Conditions.Poison1)]
		},
		new MonsterStats()
		{
			Health = 4,
			Move = 3,
			Attack = 2,
			Traits = [new ApplyConditionTrait(Conditions.Poison1)]
		},
		new MonsterStats()
		{
			Health = 6,
			Move = 3,
			Attack = 2,
			Traits = [new ApplyConditionTrait(Conditions.Poison1)]
		},
		new MonsterStats()
		{
			Health = 7,
			Move = 3,
			Attack = 3,
			Traits = [new ApplyConditionTrait(Conditions.Poison1)]
		},
		new MonsterStats()
		{
			Health = 8,
			Move = 4,
			Attack = 3,
			Traits = [new ApplyConditionTrait(Conditions.Poison1)]
		},
		new MonsterStats()
		{
			Health = 10,
			Move = 4,
			Attack = 3,
			Traits = [new ApplyConditionTrait(Conditions.Poison1)]
		},
	];

	public override MonsterStats[] EliteLevelStats =>
	[
		new MonsterStats()
		{
			Health = 3,
			Move = 2,
			Attack = 2,
			Traits = [new ApplyConditionTrait(Conditions.Poison1)]
		},
		new MonsterStats()
		{
			Health = 5,
			Move = 2,
			Attack = 2,
			Traits = [new ApplyConditionTrait(Conditions.Poison1)]
		},
		new MonsterStats()
		{
			Health = 7,
			Move = 3,
			Attack = 2,
			Traits = [new ApplyConditionTrait(Conditions.Poison1)]
		},
		new MonsterStats()
		{
			Health = 8,
			Move = 3,
			Attack = 3,
			Traits = [new ApplyConditionTrait(Conditions.Poison1)]
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 3,
			Attack = 3,
			Traits = [new ApplyConditionTrait(Conditions.Poison1)]
		},
		new MonsterStats()
		{
			Health = 13,
			Move = 4,
			Attack = 3,
			Traits = [new ApplyConditionTrait(Conditions.Poison1)]
		},
		new MonsterStats()
		{
			Health = 14,
			Move = 4,
			Attack = 4,
			Traits = [new ApplyConditionTrait(Conditions.Poison1)]
		},
		new MonsterStats()
		{
			Health = 17,
			Move = 4,
			Attack = 4,
			Traits = [new ApplyConditionTrait(Conditions.Poison1)]
		},
	];

	public override string Name => "Giant Viper";

	public override string AssetPath => "res://Content/Monsters/GiantViper";

	public override int MaxStandeeCount => 10;

	public override IEnumerable<MonsterAbilityCardModel> Deck => GiantViperAbilityCard.Deck;
}
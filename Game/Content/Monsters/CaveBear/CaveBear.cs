using System.Collections.Generic;

public class CaveBear : MonsterModel
{
	public override MonsterStats[] NormalLevelStats =>
	[
		new MonsterStats()
		{
			Health = 7,
			Move = 3,
			Attack = 3,
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 3,
			Attack = 3,
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 4,
			Attack = 3,
		},
		new MonsterStats()
		{
			Health = 13,
			Move = 4,
			Attack = 4,
		},
		new MonsterStats()
		{
			Health = 16,
			Move = 4,
			Attack = 4,
		},
		new MonsterStats()
		{
			Health = 17,
			Move = 5,
			Attack = 4,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 19,
			Move = 5,
			Attack = 5,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 22,
			Move = 5,
			Attack = 5,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
	];

	public override MonsterStats[] EliteLevelStats =>
	[
		new MonsterStats()
		{
			Health = 11,
			Move = 3,
			Attack = 4,
		},
		new MonsterStats()
		{
			Health = 14,
			Move = 3,
			Attack = 4,
		},
		new MonsterStats()
		{
			Health = 17,
			Move = 4,
			Attack = 4,
		},
		new MonsterStats()
		{
			Health = 20,
			Move = 4,
			Attack = 5,
		},
		new MonsterStats()
		{
			Health = 21,
			Move = 5,
			Attack = 5,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 24,
			Move = 5,
			Attack = 6,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 28,
			Move = 5,
			Attack = 7,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 33,
			Move = 5,
			Attack = 7,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
	];

	public override string Name => "Cave Bear";

	public override string AssetPath => "res://Content/Monsters/CaveBear";

	public override int MaxStandeeCount => 4;

	public override IEnumerable<MonsterAbilityCardModel> Deck => CaveBearAbilityCard.Deck;
}
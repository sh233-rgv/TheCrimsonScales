using System.Collections.Generic;

public class RendingDrake : MonsterModel
{
	public override MonsterStats[] NormalLevelStats =>
	[
		new MonsterStats()
		{
			Health = 5,
			Move = 3,
			Attack = 2,
		},
		new MonsterStats()
		{
			Health = 6,
			Move = 3,
			Attack = 3,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 7,
			Move = 4,
			Attack = 3,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 7,
			Move = 4,
			Attack = 4,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 4,
			Attack = 4,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 10,
			Move = 4,
			Attack = 5,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 5,
			Attack = 5,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 14,
			Move = 5,
			Attack = 5,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
	];

	public override MonsterStats[] EliteLevelStats =>
	[
		new MonsterStats()
		{
			Health = 7,
			Move = 4,
			Attack = 4,
		},
		new MonsterStats()
		{
			Health = 7,
			Move = 4,
			Attack = 5,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 5,
			Attack = 5,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 10,
			Move = 5,
			Attack = 6,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 6,
			Attack = 6,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 14,
			Move = 6,
			Attack = 6,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 15,
			Move = 6,
			Attack = 7,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
		new MonsterStats()
		{
			Health = 18,
			Move = 6,
			Attack = 7,
			Traits = [new ApplyConditionTrait(Conditions.Wound1)]
		},
	];

	public override string Name => "Rending Drake";

	public override string AssetPath => "res://Content/Monsters/RendingDrake";

	public override int MaxStandeeCount => 6;

	public override IEnumerable<MonsterAbilityCardModel> Deck => RendingDrakeAbilityCard.Deck;
}
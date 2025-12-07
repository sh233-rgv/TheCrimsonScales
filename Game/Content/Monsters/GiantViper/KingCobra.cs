using System.Collections.Generic;
public class KingCobra : GiantViper
{
	public override MonsterStats[] BossLevelStats =>
	[
		new MonsterStats()
		{
			Health = 3 * ((CharacterCount * 2) + 2),
			Move = 2,
			Attack = 3,
			Traits = [new ApplyConditionTrait(Conditions.Poison1), ConditionImmunityTrait.PoisonImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 5 * ((CharacterCount * 2) + 2),
			Move = 2,
			Attack = 3,
			Traits = [new ApplyConditionTrait(Conditions.Poison1), ConditionImmunityTrait.PoisonImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 7 * ((CharacterCount * 2) + 2),
			Move = 3,
			Attack = 3,
			Traits = [new ApplyConditionTrait(Conditions.Poison1), ConditionImmunityTrait.PoisonImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 8 * ((CharacterCount * 2) + 2),
			Move = 3,
			Attack = 4,
			Traits = [new ApplyConditionTrait(Conditions.Poison1), ConditionImmunityTrait.PoisonImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 11 * ((CharacterCount * 2) + 2),
			Move = 3,
			Attack = 4,
			Traits = [new ApplyConditionTrait(Conditions.Poison1), ConditionImmunityTrait.PoisonImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 13 * ((CharacterCount * 2) + 2),
			Move = 4,
			Attack = 4,
			Traits = [new ApplyConditionTrait(Conditions.Poison1), ConditionImmunityTrait.PoisonImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 14 * ((CharacterCount * 2) + 2),
			Move = 4,
			Attack = 5,
			Traits = [new ApplyConditionTrait(Conditions.Poison1), ConditionImmunityTrait.PoisonImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 17 * ((CharacterCount * 2) + 2),
			Move = 4,
			Attack = 5,
			Traits = [new ApplyConditionTrait(Conditions.Poison1), ConditionImmunityTrait.PoisonImmunityTrait()]
		},
	];

	public override string Name => "King Cobra";

	public override int MaxStandeeCount => 1;

	public override string AssetPath => "res://Content/Monsters/GiantViper";

	public override IEnumerable<MonsterAbilityCardModel> Deck => GiantViperAbilityCard.Deck;
}
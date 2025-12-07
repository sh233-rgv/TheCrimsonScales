using System.Collections.Generic;
public class GoringGrizzly : CaveBear
{
	public override MonsterStats[] BossLevelStats =>
	[
		new MonsterStats()
		{
			Health = 11,
			Move = 3,
			Attack = 4,
			Traits = [new ShieldTrait(CharacterCount), ConditionImmunityTrait.PoisonImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 14,
			Move = 3,
			Attack = 4,
			Traits = [new ShieldTrait(CharacterCount), ConditionImmunityTrait.PoisonImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 17,
			Move = 4,
			Attack = 4,
			Traits = [new ShieldTrait(CharacterCount), ConditionImmunityTrait.PoisonImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 20,
			Move = 4,
			Attack = 5,
			Traits = [new ShieldTrait(CharacterCount), ConditionImmunityTrait.PoisonImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 21,
			Move = 5,
			Attack = 5,
			Traits = [new ApplyConditionTrait(Conditions.Wound1), new ShieldTrait(CharacterCount),
				ConditionImmunityTrait.PoisonImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 24,
			Move = 5,
			Attack = 6,
			Traits = [new ApplyConditionTrait(Conditions.Wound1), new ShieldTrait(CharacterCount),
				ConditionImmunityTrait.PoisonImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 28,
			Move = 5,
			Attack = 7,
			Traits = [new ApplyConditionTrait(Conditions.Wound1), new ShieldTrait(CharacterCount),
				ConditionImmunityTrait.PoisonImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 33,
			Move = 5,
			Attack = 7,
			Traits = [new ApplyConditionTrait(Conditions.Wound1), new ShieldTrait(CharacterCount),
				ConditionImmunityTrait.PoisonImmunityTrait()]
		},
	];

	public override string Name => "Goring Grizzly";

	public override int MaxStandeeCount => 1;

	public override string AssetPath => "res://Content/Monsters/CaveBear";

	public override IEnumerable<MonsterAbilityCardModel> Deck => CaveBearAbilityCard.Deck;
}
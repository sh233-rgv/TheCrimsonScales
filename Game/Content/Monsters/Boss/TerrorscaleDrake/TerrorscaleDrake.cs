using System.Collections.Generic;

public abstract class TerrorscaleDrake : MonsterModel, IBossMonsterModel
{
	public override MonsterStats[] BossLevelStats =>
	[
		new MonsterStats()
		{
			Health = 9 * CharacterCount,
			Move = 4,
			Attack = 3,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 1
		},
		new MonsterStats()
		{
			Health = 10 * CharacterCount,
			Move = 4,
			Attack = 4,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 1
		},
		new MonsterStats()
		{
			Health = 13 * CharacterCount,
			Move = 5,
			Attack = 4,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 1
		},
		new MonsterStats()
		{
			Health = 14 * CharacterCount,
			Move = 5,
			Attack = 4,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 2
		},
		new MonsterStats()
		{
			Health = 18 * CharacterCount,
			Move = 5,
			Attack = 5,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 2
		},
		new MonsterStats()
		{
			Health = 20 * CharacterCount,
			Move = 6,
			Attack = 5,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 3
		},
		new MonsterStats()
		{
			Health = 25 * CharacterCount,
			Move = 6,
			Attack = 5,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 3
		},
		new MonsterStats()
		{
			Health = 27 * CharacterCount,
			Move = 6,
			Attack = 6,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 4
		},
	];

	public override string Name => "Terrorscale Drake";

	public override string AssetPath => "res://Content/Monsters/Boss/TerrorscaleDrake";
	public override string PortraitTexturePath => $"{AssetPath}/Portrait.tres";
	public override string MapIconTexturePath => $"{AssetPath}/MapIcon.tres";

	public override int MaxStandeeCount => 1;

	public override IEnumerable<MonsterAbilityCardModel> Deck => BossAbilityCard.Deck;

	public abstract string GetSpecial1Description(Monster monster);
	public abstract string GetSpecial2Description(Monster monster);
	public abstract IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster);
	public abstract IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster);
}
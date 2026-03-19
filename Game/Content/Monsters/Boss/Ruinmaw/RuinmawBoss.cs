using System.Collections.Generic;

public abstract class RuinmawBoss : MonsterModel, IBossMonsterModel
{
	public override MonsterStats[] BossLevelStats =>
	[
		new MonsterStats()
		{
			Health = 9 * CharacterCount,
			Move = 3,
			Attack = 2,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait(), ConditionImmunityTrait.WoundImmunityTrait()
			]
		},
		new MonsterStats()
		{
			Health = 10 * CharacterCount,
			Move = 3,
			Attack = 3,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait(), ConditionImmunityTrait.WoundImmunityTrait()
			]
		},
		new MonsterStats()
		{
			Health = 12 * CharacterCount,
			Move = 3,
			Attack = 3,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait(), ConditionImmunityTrait.WoundImmunityTrait()
			]
		},
		new MonsterStats()
		{
			Health = 13 * CharacterCount,
			Move = 3,
			Attack = 4,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait(), ConditionImmunityTrait.WoundImmunityTrait()
			]
		},
		new MonsterStats()
		{
			Health = 15 * CharacterCount,
			Move = 4,
			Attack = 4,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait(), ConditionImmunityTrait.WoundImmunityTrait()
			]
		},
		new MonsterStats()
		{
			Health = 17 * CharacterCount,
			Move = 4,
			Attack = 4,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait(), ConditionImmunityTrait.WoundImmunityTrait()
			]
		},
		new MonsterStats()
		{
			Health = 20 * CharacterCount,
			Move = 4,
			Attack = 5,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait(), ConditionImmunityTrait.WoundImmunityTrait()
			]
		},
		new MonsterStats()
		{
			Health = 24 * CharacterCount,
			Move = 5,
			Attack = 5,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait(), ConditionImmunityTrait.WoundImmunityTrait()
			]
		},
	];

	public override string Name => "Ruinmaw";

	public override string AssetPath => "res://Content/Classes/Ruinmaw";
	public override string PortraitTexturePath => $"{AssetPath}/Portrait.tres";
	public override string MapIconTexturePath => $"{AssetPath}/MapIcon.tres";

	public override int MaxStandeeCount => 1;

	public override IEnumerable<MonsterAbilityCardModel> Deck => BossAbilityCard.Deck;

	public abstract string GetSpecial1Description(Monster monster);
	public abstract string GetSpecial2Description(Monster monster);
	public abstract IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster);
	public abstract IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster);
}
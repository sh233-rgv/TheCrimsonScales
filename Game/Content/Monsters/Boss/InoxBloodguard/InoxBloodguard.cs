using System.Collections.Generic;

public class InoxBloodguard : MonsterModel, IBossMonsterModel
{
	public override MonsterStats[] BossLevelStats =>
	[
		new MonsterStats()
		{
			Health = 8 * CharacterCount,
			Move = 3,
			Attack = 2,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				new ConditionImmunityTrait(Conditions.Muddle)
			],
			CustomValue = 1
		},
		new MonsterStats()
		{
			Health = 9 * CharacterCount,
			Move = 3,
			Attack = 3,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				new ConditionImmunityTrait(Conditions.Muddle), new RetaliateTrait(1)
			],
			CustomValue = 1
		},
		new MonsterStats()
		{
			Health = 11 * CharacterCount,
			Move = 3,
			Attack = 3,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				new ConditionImmunityTrait(Conditions.Muddle), new RetaliateTrait(1)
			],
			CustomValue = 1
		},
		new MonsterStats()
		{
			Health = 13 * CharacterCount,
			Move = 4,
			Attack = 4,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				new ConditionImmunityTrait(Conditions.Muddle), new RetaliateTrait(2)
			],
			CustomValue = 2
		},
		new MonsterStats()
		{
			Health = 16 * CharacterCount,
			Move = 4,
			Attack = 4,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				new ConditionImmunityTrait(Conditions.Muddle), new RetaliateTrait(2)
			],
			CustomValue = 2
		},
		new MonsterStats()
		{
			Health = 18 * CharacterCount,
			Move = 4,
			Attack = 5,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				new ConditionImmunityTrait(Conditions.Muddle), new RetaliateTrait(3)
			],
			CustomValue = 3
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
				new ConditionImmunityTrait(Conditions.Muddle), new RetaliateTrait(3)
			],
			CustomValue = 3
		},
		new MonsterStats()
		{
			Health = 24 * CharacterCount,
			Move = 5,
			Attack = 6,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				new ConditionImmunityTrait(Conditions.Muddle), new RetaliateTrait(4)
			],
			CustomValue = 4
		},
	];

	public override string Name => "Inox Bloodguard";

	public override string AssetPath => "res://Content/Monsters/Boss/InoxBloodguard";
	public override string PortraitTexturePath => $"{AssetPath}/Portrait.tres";
	public override string MapIconTexturePath => $"{AssetPath}/MapIcon.tres";

	public override int MaxStandeeCount => 1;

	public override IEnumerable<MonsterAbilityCardModel> Deck => BossAbilityCard.Deck;

	// IBossMonsterModel
	public string GetSpecial1Description(Monster monster) => $"""
	                                                          {Icons.Inline(Icons.Move)}{monster.Stats.Move + 1}, {Icons.Inline(Icons.Jump)}
	                                                          {Icons.Inline(Icons.Attack)}{monster.Stats.Attack + 1}
	                                                          """;

	public string GetSpecial2Description(Monster monster) => $"""
	                                                          {Icons.Inline(Icons.Move)}{monster.Stats.Move}
	                                                          {Icons.Inline(Icons.Attack)}{monster.Stats.Attack}, {Icons.Inline(Icons.Push)}3
	                                                          {Icons.Inline(Icons.Shield)}{monster.Stats.CustomValue}
	                                                          """;

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.MoveAbility(monster, +1).WithMoveType(MoveType.Jump)),
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, +1))
	];

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.MoveAbility(monster, +0)),
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, +0).WithPush(3)),
		new MonsterAbilityCardAbility(ShieldAbility.Builder().WithShieldValue(monster.Stats.CustomValue))
	];
}
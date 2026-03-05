using System.Collections.Generic;
using Godot;

public class InoxBodyguard : MonsterModel, IBossMonsterModel
{
	public override MonsterStats[] BossLevelStats =>
	[
		new MonsterStats()
		{
			Health = 6 * CharacterCount,
			Move = 2,
			Attack = CharacterCount,
			Traits =
			[
				new ApplyConditionTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Stun),
				new ConditionImmunityTrait(Conditions.Muddle), ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 3
		},
		new MonsterStats()
		{
			Health = 7 * CharacterCount,
			Move = 2,
			Attack = 1 + CharacterCount,
			Traits =
			[
				new ApplyConditionTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Stun),
				new ConditionImmunityTrait(Conditions.Muddle), ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 3
		},
		new MonsterStats()
		{
			Health = 9 * CharacterCount,
			Move = 2,
			Attack = 1 + CharacterCount,
			Traits =
			[
				new ApplyConditionTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Stun),
				new ConditionImmunityTrait(Conditions.Muddle), ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 3
		},
		new MonsterStats()
		{
			Health = 10 * CharacterCount,
			Move = 3,
			Attack = 2 + CharacterCount,
			Traits =
			[
				new ApplyConditionTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Stun),
				new ConditionImmunityTrait(Conditions.Muddle), ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 4
		},
		new MonsterStats()
		{
			Health = 11 * CharacterCount,
			Move = 3,
			Attack = 2 + CharacterCount,
			Traits =
			[
				new ApplyConditionTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Stun),
				new ConditionImmunityTrait(Conditions.Muddle), ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 4
		},
		new MonsterStats()
		{
			Health = 13 * CharacterCount,
			Move = 3,
			Attack = 3 + CharacterCount,
			Traits =
			[
				new ApplyConditionTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Stun),
				new ConditionImmunityTrait(Conditions.Muddle), ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 5
		},
		new MonsterStats()
		{
			Health = 15 * CharacterCount,
			Move = 4,
			Attack = 3 + CharacterCount,
			Traits =
			[
				new ApplyConditionTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Stun),
				new ConditionImmunityTrait(Conditions.Muddle), ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 5
		},
		new MonsterStats()
		{
			Health = 17 * CharacterCount,
			Move = 4,
			Attack = 4 + CharacterCount,
			Traits =
			[
				new ApplyConditionTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Stun),
				new ConditionImmunityTrait(Conditions.Muddle), ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 5
		},
	];

	public override string Name => "Inox Bodyguard";

	public override string AssetPath => "res://Content/Monsters/Boss/InoxBodyguard";

	public override int MaxStandeeCount => 1;

	public override IEnumerable<MonsterAbilityCardModel> Deck => BossAbilityCard.Deck;

	// IBossMonsterModel
	public string GetSpecial1Description(Monster monster) => $"""
	                                                          {Icons.Inline(Icons.Move)}{monster.Stats.Move - 1}
	                                                          {Icons.Inline(Icons.Attack)}{monster.Stats.Attack - 1}, area of effect
	                                                          """;

	public string GetSpecial2Description(Monster monster) => $"""
	                                                          {Icons.Inline(Icons.Move)}{monster.Stats.Move}
	                                                          {Icons.Inline(Icons.Attack)}{monster.Stats.Attack}
	                                                          {Icons.Inline(Icons.Retaliate)}{monster.Stats.CustomValue}
	                                                          """;

	public virtual IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.MoveAbility(monster, -1)),
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, -1).WithAOEPattern(new AOEPattern(
			[
				new AOEHex(Vector2I.Zero, AOEHexType.Gray),
				new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
				new AOEHex(Vector2I.Zero.Add(Direction.SouthEast), AOEHexType.Red),
				new AOEHex(Vector2I.Zero.Add(Direction.East).Add(Direction.East), AOEHexType.Red),
				new AOEHex(Vector2I.Zero.Add(Direction.SouthEast).Add(Direction.East), AOEHexType.Red),
				new AOEHex(Vector2I.Zero.Add(Direction.SouthEast).Add(Direction.SouthEast), AOEHexType.Red),
			]
		)))
	];

	public virtual IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.MoveAbility(monster, +0)),
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, +0)),
		new MonsterAbilityCardAbility(RetaliateAbility.Builder().WithRetaliateValue(monster.Stats.CustomValue))
	];
}
using System.Collections.Generic;

public class RavenousGharial : MonsterModel
{
	public override MonsterStats[] NormalLevelStats =>
	[
		new MonsterStats()
		{
			Health = 6,
			Move = 2,
			Attack = 2,
			Traits = [new IgnoreWaterTrait(), new ShieldRangedAttacksTrait(1)]
		},
		new MonsterStats()
		{
			Health = 8,
			Move = 2,
			Attack = 3,
			Traits = [new IgnoreWaterTrait(), new ShieldRangedAttacksTrait(1)]
		},
		new MonsterStats()
		{
			Health = 10,
			Move = 2,
			Attack = 3,
			Traits = [new IgnoreWaterTrait(), new ShieldRangedAttacksTrait(1)]
		},
		new MonsterStats()
		{
			Health = 12,
			Move = 3,
			Attack = 3,
			Traits = [new IgnoreWaterTrait(), new ShieldRangedAttacksTrait(1)]
		},
		new MonsterStats()
		{
			Health = 12,
			Move = 3,
			Attack = 4,
			Traits = [new IgnoreWaterTrait(), new ShieldRangedAttacksTrait(2)]
		},
		new MonsterStats()
		{
			Health = 15,
			Move = 3,
			Attack = 4,
			Traits = [new IgnoreWaterTrait(), new ShieldRangedAttacksTrait(2)]
		},
		new MonsterStats()
		{
			Health = 16,
			Move = 4,
			Attack = 4,
			Traits = [new IgnoreWaterTrait(), new ShieldRangedAttacksTrait(2)]
		},
		new MonsterStats()
		{
			Health = 18,
			Move = 4,
			Attack = 5,
			Traits = [new IgnoreWaterTrait(), new ShieldRangedAttacksTrait(2)]
		},
	];

	public override MonsterStats[] EliteLevelStats =>
	[
		new MonsterStats()
		{
			Health = 10,
			Move = 3,
			Attack = 3,
			Traits = [new IgnoreWaterTrait(), new ShieldRangedAttacksTrait(1)]
		},
		new MonsterStats()
		{
			Health = 12,
			Move = 3,
			Attack = 4,
			Traits = [new IgnoreWaterTrait(), new ShieldRangedAttacksTrait(1)]
		},
		new MonsterStats()
		{
			Health = 14,
			Move = 3,
			Attack = 4,
			Traits = [new IgnoreWaterTrait(), new ShieldRangedAttacksTrait(2)]
		},
		new MonsterStats()
		{
			Health = 16,
			Move = 4,
			Attack = 4,
			Traits = [new IgnoreWaterTrait(), new ShieldRangedAttacksTrait(2)]
		},
		new MonsterStats()
		{
			Health = 16,
			Move = 4,
			Attack = 5,
			Traits = [new IgnoreWaterTrait(), new ShieldRangedAttacksTrait(3)]
		},
		new MonsterStats()
		{
			Health = 19,
			Move = 4,
			Attack = 5,
			Traits = [new IgnoreWaterTrait(), new ShieldRangedAttacksTrait(3)]
		},
		new MonsterStats()
		{
			Health = 20,
			Move = 5,
			Attack = 5,
			Traits = [new IgnoreWaterTrait(), new ShieldRangedAttacksTrait(4)]
		},
		new MonsterStats()
		{
			Health = 22,
			Move = 5,
			Attack = 6,
			Traits = [new IgnoreWaterTrait(), new ShieldRangedAttacksTrait(4)]
		},
	];

	public override string Name => "Ravenous Gharial";

	public override string AssetPath => "res://Content/Monsters/RavenousGharial";
	public override string PortraitTexturePath => $"{AssetPath}/Portrait.tres";
	public override string MapIconTexturePath => $"{AssetPath}/MapIcon.tres";

	public override int MaxStandeeCount => 6;

	public override IEnumerable<MonsterAbilityCardModel> Deck => RavenousGharialAbilityCard.Deck;
}
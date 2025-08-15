using System.Collections.Generic;

public class AncientArtillery : MonsterModel
{
	public override MonsterStats[] NormalLevelStats =>
	[
		new MonsterStats()
		{
			Health = 4,
			Attack = 2,
			Range = 4,
		},
		new MonsterStats()
		{
			Health = 6,
			Attack = 2,
			Range = 4,
		},
		new MonsterStats()
		{
			Health = 7,
			Attack = 2,
			Range = 5,
		},
		new MonsterStats()
		{
			Health = 8,
			Attack = 3,
			Range = 5,
		},
		new MonsterStats()
		{
			Health = 9,
			Attack = 4,
			Range = 5,
		},
		new MonsterStats()
		{
			Health = 11,
			Attack = 4,
			Range = 6,
		},
		new MonsterStats()
		{
			Health = 14,
			Attack = 4,
			Range = 6,
		},
		new MonsterStats()
		{
			Health = 16,
			Attack = 4,
			Range = 7,
		},
	];

	public override MonsterStats[] EliteLevelStats =>
	[
		new MonsterStats()
		{
			Health = 7,
			Attack = 3,
			Range = 5,
		},
		new MonsterStats()
		{
			Health = 9,
			Attack = 3,
			Range = 5,
		},
		new MonsterStats()
		{
			Health = 11,
			Attack = 3,
			Range = 6,
		},
		new MonsterStats()
		{
			Health = 13,
			Attack = 4,
			Range = 6,
		},
		new MonsterStats()
		{
			Health = 13,
			Attack = 4,
			Range = 6,
			Traits = [new TargetsTrait(2)]
		},
		new MonsterStats()
		{
			Health = 15,
			Attack = 4,
			Range = 7,
			Traits = [new TargetsTrait(2)]
		},
		new MonsterStats()
		{
			Health = 16,
			Attack = 5,
			Range = 7,
			Traits = [new TargetsTrait(2)]
		},
		new MonsterStats()
		{
			Health = 20,
			Attack = 5,
			Range = 7,
			Traits = [new TargetsTrait(2)]
		},
	];

	public override string Name => "Ancient Artillery";

	public override string AssetPath => "res://Content/Monsters/AncientArtillery";

	public override int MaxStandeeCount => 6;

	public override IEnumerable<MonsterAbilityCardModel> Deck => AncientArtilleryAbilityCard.Deck;
}
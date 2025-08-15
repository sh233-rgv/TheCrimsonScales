using System.Collections.Generic;

public class VermlingScout : MonsterModel
{
	public override MonsterStats[] NormalLevelStats { get; } =
	[
		new MonsterStats()
		{
			Health = 2,
			Move = 3,
			Attack = 1,
		},
		new MonsterStats()
		{
			Health = 3,
			Move = 3,
			Attack = 1,
		},
		new MonsterStats()
		{
			Health = 3,
			Move = 3,
			Attack = 2,
		},
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
		},
		new MonsterStats()
		{
			Health = 8,
			Move = 3,
			Attack = 3,
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 4,
			Attack = 3,
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 4,
			Attack = 6,
		},
	];

	public override MonsterStats[] EliteLevelStats { get; } =
	[
		new MonsterStats()
		{
			Health = 4,
			Move = 3,
			Attack = 2,
		},
		new MonsterStats()
		{
			Health = 5,
			Move = 3,
			Attack = 2,
		},
		new MonsterStats()
		{
			Health = 5,
			Move = 4,
			Attack = 3,
		},
		new MonsterStats()
		{
			Health = 7,
			Move = 4,
			Attack = 3,
		},
		new MonsterStats()
		{
			Health = 8,
			Move = 4,
			Attack = 4,
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 4,
			Attack = 4,
		},
		new MonsterStats()
		{
			Health = 12,
			Move = 5,
			Attack = 4,
		},
		new MonsterStats()
		{
			Health = 15,
			Move = 5,
			Attack = 4,
		},
	];

	public override string Name => "Vermling Scout";

	public override string AssetPath => "res://Content/Monsters/VermlingScout";

	public override int MaxStandeeCount => 10;

	public override IEnumerable<MonsterAbilityCardModel> Deck => ScoutAbilityCard.Deck;
}
using System.Collections.Generic;

public class CityArcher : MonsterModel
{
	public override MonsterStats[] NormalLevelStats =>
	[
		new MonsterStats()
		{
			Health = 4,
			Move = 1,
			Attack = 2,
			Range = 3,
		},
		new MonsterStats()
		{
			Health = 5,
			Move = 1,
			Attack = 2,
			Range = 4,
		},
		new MonsterStats()
		{
			Health = 6,
			Move = 1,
			Attack = 3,
			Range = 4,
		},
		new MonsterStats()
		{
			Health = 6,
			Move = 2,
			Attack = 3,
			Range = 4,
			Traits = [new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 8,
			Move = 2,
			Attack = 3,
			Range = 5,
			Traits = [new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 2,
			Attack = 4,
			Range = 5,
			Traits = [new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 3,
			Attack = 4,
			Range = 5,
			Traits = [new ShieldTrait(3)]
		},
		new MonsterStats()
		{
			Health = 10,
			Move = 3,
			Attack = 4,
			Range = 6,
			Traits = [new ShieldTrait(2)]
		},
	];

	public override MonsterStats[] EliteLevelStats =>
	[
		new MonsterStats()
		{
			Health = 6,
			Move = 1,
			Attack = 3,
			Range = 4,
		},
		new MonsterStats()
		{
			Health = 6,
			Move = 1,
			Attack = 3,
			Range = 5,
			Traits = [new ShieldTrait(1), new PierceTrait(1)]
		},
		new MonsterStats()
		{
			Health = 7,
			Move = 1,
			Attack = 4,
			Range = 5,
			Traits = [new ShieldTrait(1), new PierceTrait(2)]
		},
		new MonsterStats()
		{
			Health = 8,
			Move = 2,
			Attack = 4,
			Range = 5,
			Traits = [new ShieldTrait(2), new PierceTrait(2)]
		},
		new MonsterStats()
		{
			Health = 10,
			Move = 2,
			Attack = 4,
			Range = 6,
			Traits = [new ShieldTrait(2), new PierceTrait(2)]
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 2,
			Attack = 5,
			Range = 6,
			Traits = [new ShieldTrait(2), new PierceTrait(3)]
		},
		new MonsterStats()
		{
			Health = 12,
			Move = 3,
			Attack = 6,
			Range = 6,
			Traits = [new ShieldTrait(2), new PierceTrait(3)]
		},
		new MonsterStats()
		{
			Health = 13,
			Move = 3,
			Attack = 6,
			Range = 7,
			Traits = [new ShieldTrait(3), new PierceTrait(3)]
		},
	];

	public override string Name => "City Archer";

	public override string AssetPath => "res://Content/Monsters/CityArcher";

	public override int MaxStandeeCount => 6;

	public override IEnumerable<MonsterAbilityCardModel> Deck => ArcherAbilityCard.Deck;
}
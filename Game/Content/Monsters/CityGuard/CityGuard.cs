using System.Collections.Generic;

public class CityGuard : MonsterModel
{
	public override MonsterStats[] NormalLevelStats =>
	[
		new MonsterStats()
		{
			Health = 5,
			Move = 2,
			Attack = 2,
		},
		new MonsterStats()
		{
			Health = 5,
			Move = 2,
			Attack = 2,
			Traits = [new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 7,
			Move = 2,
			Attack = 2,
			Traits = [new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 8,
			Move = 2,
			Attack = 3,
			Traits = [new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 3,
			Attack = 3,
			Traits = [new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 10,
			Move = 3,
			Attack = 3,
			Traits = [new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 3,
			Attack = 4,
			Traits = [new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 13,
			Move = 3,
			Attack = 4,
			Traits = [new ShieldTrait(2)]
		},
	];

	public override MonsterStats[] EliteLevelStats =>
	[
		new MonsterStats()
		{
			Health = 6,
			Move = 2,
			Attack = 3,
			Traits = [new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 6,
			Move = 2,
			Attack = 3,
			Traits = [new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 2,
			Attack = 3,
			Traits = [new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 2,
			Attack = 4,
			Traits = [new ShieldTrait(2), new RetaliateTrait(1)]
		},
		new MonsterStats()
		{
			Health = 10,
			Move = 3,
			Attack = 4,
			Traits = [new ShieldTrait(2), new RetaliateTrait(2)]
		},
		new MonsterStats()
		{
			Health = 12,
			Move = 3,
			Attack = 4,
			Traits = [new ShieldTrait(3), new RetaliateTrait(2)]
		},
		new MonsterStats()
		{
			Health = 13,
			Move = 3,
			Attack = 5,
			Traits = [new ShieldTrait(3), new RetaliateTrait(3)]
		},
		new MonsterStats()
		{
			Health = 14,
			Move = 3,
			Attack = 6,
			Traits = [new ShieldTrait(3), new RetaliateTrait(3)]
		},
	];

	public override string Name => "City Guard";

	public override string AssetPath => "res://Content/Monsters/CityGuard";

	public override int MaxStandeeCount => 6;

	public override IEnumerable<MonsterAbilityCardModel> Deck => GuardAbilityCard.Deck;
}
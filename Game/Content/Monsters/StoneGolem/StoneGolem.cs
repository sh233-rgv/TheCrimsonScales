using System.Collections.Generic;

public class StoneGolem : MonsterModel
{
	public override MonsterStats[] NormalLevelStats =>
	[
		new MonsterStats()
		{
			Health = 10,
			Move = 1,
			Attack = 3,
		},
		new MonsterStats()
		{
			Health = 10,
			Move = 1,
			Attack = 3,
			Traits = [new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 1,
			Attack = 4,
			Traits = [new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 1,
			Attack = 4,
			Traits = [new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 12,
			Move = 2,
			Attack = 4,
			Traits = [new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 13,
			Move = 2,
			Attack = 5,
			Traits = [new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 16,
			Move = 2,
			Attack = 5,
			Traits = [new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 16,
			Move = 2,
			Attack = 5,
			Traits = [new ShieldTrait(3)]
		},
	];

	public override MonsterStats[] EliteLevelStats =>
	[
		new MonsterStats()
		{
			Health = 10,
			Move = 2,
			Attack = 4,
			Traits = [new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 2,
			Attack = 4,
			Traits = [new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 14,
			Move = 2,
			Attack = 5,
			Traits = [new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 15,
			Move = 2,
			Attack = 5,
			Traits = [new ShieldTrait(3)]
		},
		new MonsterStats()
		{
			Health = 17,
			Move = 2,
			Attack = 6,
			Traits = [new ShieldTrait(3)]
		},
		new MonsterStats()
		{
			Health = 19,
			Move = 3,
			Attack = 6,
			Traits = [new ShieldTrait(3)]
		},
		new MonsterStats()
		{
			Health = 20,
			Move = 3,
			Attack = 7,
			Traits = [new ShieldTrait(3)]
		},
		new MonsterStats()
		{
			Health = 21,
			Move = 3,
			Attack = 7,
			Traits = [new ShieldTrait(4)]
		},
	];

	public override string Name => "Stone Golem";

	public override string AssetPath => "res://Content/Monsters/StoneGolem";

	public override int MaxStandeeCount => 4;

	public override IEnumerable<MonsterAbilityCardModel> Deck => StoneGolemAbilityCard.Deck;
}
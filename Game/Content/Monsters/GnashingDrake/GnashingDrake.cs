using System.Collections.Generic;

public class GnashingDrake : MonsterModel
{
	public override MonsterStats[] NormalLevelStats =>
	[
		new MonsterStats()
		{
			Health = 2,
			Move = 2,
			Attack = 1
		},
		new MonsterStats()
		{
			Health = 3,
			Move = 2,
			Attack = 1
		},
		new MonsterStats()
		{
			Health = 4,
			Move = 2,
			Attack = 2
		},
		new MonsterStats()
		{
			Health = 5,
			Move = 3,
			Attack = 2
		},
		new MonsterStats()
		{
			Health = 6,
			Move = 3,
			Attack = 3,
			Traits = [new JumpTrait()]
		},
		new MonsterStats()
		{
			Health = 7,
			Move = 3,
			Attack = 3,
			Traits = [new JumpTrait()]
		},
		new MonsterStats()
		{
			Health = 8,
			Move = 4,
			Attack = 4,
			Traits = [new JumpTrait()]
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 4,
			Attack = 4,
			Traits = [new JumpTrait()]
		},
	];

	public override MonsterStats[] EliteLevelStats =>
	[
		new MonsterStats()
		{
			Health = 4,
			Move = 2,
			Attack = 1,
			Traits = [new JumpTrait()]
		},
		new MonsterStats()
		{
			Health = 5,
			Move = 2,
			Attack = 2,
			Traits = [new JumpTrait()]
		},
		new MonsterStats()
		{
			Health = 7,
			Move = 3,
			Attack = 2,
			Traits = [new JumpTrait()]
		},
		new MonsterStats()
		{
			Health = 8,
			Move = 3,
			Attack = 3,
			Traits = [new JumpTrait()]
		},
		new MonsterStats()
		{
			Health = 10,
			Move = 4,
			Attack = 4,
			Traits = [new JumpTrait()]
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 4,
			Attack = 4,
			Traits = [new JumpTrait()]
		},
		new MonsterStats()
		{
			Health = 13,
			Move = 5,
			Attack = 5,
			Traits = [new JumpTrait()]
		},
		new MonsterStats()
		{
			Health = 14,
			Move = 5,
			Attack = 5,
			Traits = [new JumpTrait()]
		},
	];

	public override string Name => "Gnashing Drake";

	public override string AssetPath => "res://Content/Monsters/GnashingDrake";
	public override string PortraitTexturePath => $"{AssetPath}/Portrait.tres";
	public override string MapIconTexturePath => $"{AssetPath}/MapIcon.tres";

	public override int MaxStandeeCount => 10;

	public override IEnumerable<MonsterAbilityCardModel> Deck => GnashingDrakeAbilityCard.Deck;
}
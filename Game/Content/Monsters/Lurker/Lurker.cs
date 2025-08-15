using System.Collections.Generic;

public class Lurker : MonsterModel
{
	public override MonsterStats[] NormalLevelStats =>
	[
		new MonsterStats()
		{
			Health = 5,
			Move = 2,
			Attack = 2,
			Traits = [new TargetsTrait(2)]
		},
		new MonsterStats()
		{
			Health = 7,
			Move = 2,
			Attack = 2,
			Traits = [new TargetsTrait(2), new PierceTrait(1)]
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 3,
			Attack = 2,
			Traits = [new TargetsTrait(2), new PierceTrait(1)]
		},
		new MonsterStats()
		{
			Health = 10,
			Move = 3,
			Attack = 3,
			Traits = [new TargetsTrait(2), new PierceTrait(2)]
		},
		new MonsterStats()
		{
			Health = 10,
			Move = 3,
			Attack = 3,
			Traits = [new TargetsTrait(2), new PierceTrait(2), new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 3,
			Attack = 4,
			Traits = [new TargetsTrait(2), new PierceTrait(2), new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 12,
			Move = 4,
			Attack = 4,
			Traits = [new TargetsTrait(2), new PierceTrait(3), new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 14,
			Move = 4,
			Attack = 4,
			Traits = [new TargetsTrait(2), new PierceTrait(3), new ShieldTrait(1)]
		},
	];

	public override MonsterStats[] EliteLevelStats =>
	[
		new MonsterStats()
		{
			Health = 7,
			Move = 2,
			Attack = 3,
			Traits = [new TargetsTrait(2), new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 2,
			Attack = 3,
			Traits = [new TargetsTrait(2), new PierceTrait(1), new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 12,
			Move = 3,
			Attack = 3,
			Traits = [new TargetsTrait(2), new PierceTrait(2), new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 14,
			Move = 3,
			Attack = 4,
			Traits = [new TargetsTrait(2), new PierceTrait(2), new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 14,
			Move = 3,
			Attack = 4,
			Traits = [new TargetsTrait(2), new PierceTrait(3), new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 15,
			Move = 3,
			Attack = 5,
			Traits = [new TargetsTrait(2), new PierceTrait(3), new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 16,
			Move = 4,
			Attack = 5,
			Traits = [new TargetsTrait(2), new PierceTrait(4), new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 18,
			Move = 4,
			Attack = 5,
			Traits = [new TargetsTrait(2), new PierceTrait(4), new ShieldTrait(2)]
		},
	];

	public override string Name => "Lurker";

	public override string AssetPath => "res://Content/Monsters/Lurker";

	public override int MaxStandeeCount => 6;

	public override IEnumerable<MonsterAbilityCardModel> Deck => LurkerAbilityCard.Deck;
}
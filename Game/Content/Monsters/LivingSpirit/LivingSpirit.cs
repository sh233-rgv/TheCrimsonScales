using System.Collections.Generic;

public class LivingSpirit : MonsterModel
{
	public override MonsterStats[] NormalLevelStats =>
	[
		new MonsterStats()
		{
			Health = 2,
			Move = 2,
			Attack = 2,
			Range = 2,
			Traits = [new FlyingTrait(), new ShieldTrait(1)]
		},
		new MonsterStats()
		{
			Health = 2,
			Move = 2,
			Attack = 2,
			Range = 2,
			Traits = [new FlyingTrait(), new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 2,
			Move = 3,
			Attack = 2,
			Range = 3,
			Traits = [new FlyingTrait(), new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 3,
			Move = 3,
			Attack = 3,
			Range = 3,
			Traits = [new FlyingTrait(), new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 3,
			Move = 3,
			Attack = 3,
			Range = 3,
			Traits = [new FlyingTrait(), new ShieldTrait(3)]
		},
		new MonsterStats()
		{
			Health = 4,
			Move = 3,
			Attack = 3,
			Range = 4,
			Traits = [new FlyingTrait(), new ShieldTrait(3)]
		},
		new MonsterStats()
		{
			Health = 4,
			Move = 3,
			Attack = 4,
			Range = 4,
			Traits = [new FlyingTrait(), new ShieldTrait(3)]
		},
		new MonsterStats()
		{
			Health = 6,
			Move = 3,
			Attack = 4,
			Range = 4,
			Traits = [new FlyingTrait(), new ShieldTrait(3)]
		},
	];

	public override MonsterStats[] EliteLevelStats =>
	[
		new MonsterStats()
		{
			Health = 3,
			Move = 3,
			Attack = 3,
			Range = 3,
			Traits = [new FlyingTrait(), new ShieldTrait(2)]
		},
		new MonsterStats()
		{
			Health = 3,
			Move = 3,
			Attack = 3,
			Range = 3,
			Traits = [new FlyingTrait(), new ShieldTrait(3)]
		},
		new MonsterStats()
		{
			Health = 3,
			Move = 4,
			Attack = 3,
			Range = 4,
			Traits = [new FlyingTrait(), new ShieldTrait(3)]
		},
		new MonsterStats()
		{
			Health = 4,
			Move = 4,
			Attack = 4,
			Range = 4,
			Traits = [new FlyingTrait(), new ShieldTrait(3)]
		},
		new MonsterStats()
		{
			Health = 4,
			Move = 4,
			Attack = 4,
			Range = 4,
			Traits = [new FlyingTrait(), new ShieldTrait(4)]
		},
		new MonsterStats()
		{
			Health = 6,
			Move = 4,
			Attack = 4,
			Range = 4,
			Traits = [new FlyingTrait(), new ShieldTrait(4)]
		},
		new MonsterStats()
		{
			Health = 7,
			Move = 4,
			Attack = 5,
			Range = 5,
			Traits = [new FlyingTrait(), new ShieldTrait(4)]
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 4,
			Attack = 5,
			Range = 5,
			Traits = [new FlyingTrait(), new ShieldTrait(4)]
		},
	];

	public override string Name => "Living Spirit";

	public override string AssetPath => "res://Content/Monsters/LivingSpirit";

	public override int MaxStandeeCount => 6;

	public override IEnumerable<MonsterAbilityCardModel> Deck => LivingSpiritAbilityCard.Deck;
}
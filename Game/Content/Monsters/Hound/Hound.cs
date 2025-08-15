using System.Collections.Generic;

public class Hound : MonsterModel
{
	public override MonsterStats[] NormalLevelStats =>
	[
		new MonsterStats()
		{
			Health = 4,
			Move = 3,
			Attack = 2,
		},
		new MonsterStats()
		{
			Health = 4,
			Move = 4,
			Attack = 2,
			Traits = [new RetaliateTrait(1)]
		},
		new MonsterStats()
		{
			Health = 6,
			Move = 4,
			Attack = 2,
			Traits = [new RetaliateTrait(1)]
		},
		new MonsterStats()
		{
			Health = 8,
			Move = 4,
			Attack = 2,
			Traits = [new RetaliateTrait(1)]
		},
		new MonsterStats()
		{
			Health = 8,
			Move = 4,
			Attack = 3,
			Traits = [new RetaliateTrait(1)]
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 4,
			Attack = 3,
			Traits = [new RetaliateTrait(2)]
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 5,
			Attack = 3,
			Traits = [new RetaliateTrait(2)]
		},
		new MonsterStats()
		{
			Health = 15,
			Move = 5,
			Attack = 3,
			Traits = [new RetaliateTrait(2)]
		},
	];

	public override MonsterStats[] EliteLevelStats =>
	[
		new MonsterStats()
		{
			Health = 6,
			Move = 5,
			Attack = 2,
		},
		new MonsterStats()
		{
			Health = 6,
			Move = 5,
			Attack = 2,
			Traits = [new RetaliateTrait(2)]
		},
		new MonsterStats()
		{
			Health = 7,
			Move = 5,
			Attack = 3,
			Traits = [new RetaliateTrait(2)]
		},
		new MonsterStats()
		{
			Health = 8,
			Move = 5,
			Attack = 4,
			Traits = [new RetaliateTrait(2)]
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 5,
			Attack = 4,
			Traits = [new RetaliateTrait(2)]
		},
		new MonsterStats()
		{
			Health = 12,
			Move = 5,
			Attack = 4,
			Traits = [new RetaliateTrait(3)]
		},
		new MonsterStats()
		{
			Health = 15,
			Move = 6,
			Attack = 4,
			Traits = [new RetaliateTrait(3)]
		},
		new MonsterStats()
		{
			Health = 15,
			Move = 6,
			Attack = 5,
			Traits = [new RetaliateTrait(4)]
		},
	];

	public override string Name => "Hound";

	public override string AssetPath => "res://Content/Monsters/Hound";

	public override int MaxStandeeCount => 6;

	public override IEnumerable<MonsterAbilityCardModel> Deck => HoundAbilityCard.Deck;
}
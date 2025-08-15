using System.Collections.Generic;

public class BanditArcher : MonsterModel
{
	public override MonsterStats[] NormalLevelStats =>
	[
		new MonsterStats()
		{
			Health = 3,
			Attack = 2,
		},
		new MonsterStats()
		{
			Health = 4,
			Attack = 2,
			Traits = [new RetaliateTrait(1)]
		},
		new MonsterStats()
		{
			Health = 4,
			Attack = 3,
			Traits = [new RetaliateTrait(1)]
		},
		new MonsterStats()
		{
			Health = 5,
			Attack = 3,
			Traits = [new RetaliateTrait(2)]
		},
		new MonsterStats()
		{
			Health = 6,
			Attack = 4,
			Traits = [new RetaliateTrait(2)]
		},
		new MonsterStats()
		{
			Health = 7,
			Attack = 4,
			Traits = [new RetaliateTrait(3)]
		},
		new MonsterStats()
		{
			Health = 8,
			Attack = 5,
			Traits = [new RetaliateTrait(3)]
		},
		new MonsterStats()
		{
			Health = 9,
			Attack = 5,
			Traits = [new RetaliateTrait(4)]
		},
	];

	public override MonsterStats[] EliteLevelStats =>
	[
		new MonsterStats()
		{
			Health = 5,
			Attack = 3,
		},
		new MonsterStats()
		{
			Health = 6,
			Attack = 3,
			Traits = [new RetaliateTrait(1)]
		},
		new MonsterStats()
		{
			Health = 7,
			Attack = 4,
			Traits = [new RetaliateTrait(1)]
		},
		new MonsterStats()
		{
			Health = 8,
			Attack = 4,
			Traits = [new RetaliateTrait(2)]
		},
		new MonsterStats()
		{
			Health = 9,
			Attack = 5,
			Traits = [new RetaliateTrait(2)]
		},
		new MonsterStats()
		{
			Health = 11,
			Attack = 5,
			Traits = [new RetaliateTrait(3)]
		},
		new MonsterStats()
		{
			Health = 13,
			Attack = 6,
			Traits = [new RetaliateTrait(3)]
		},
		new MonsterStats()
		{
			Health = 15,
			Attack = 6,
			Traits = [new RetaliateTrait(4)]
		},
	];

	public override string Name => "Bandit Archer";

	public override string AssetPath => "res://Content/Monsters/BanditArcher";

	public override int MaxStandeeCount => 10;

	public override IEnumerable<MonsterAbilityCardModel> Deck => ArcherAbilityCard.Deck;
}
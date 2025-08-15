using System.Collections.Generic;

public class InoxGuard : MonsterModel
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
			Health = 8,
			Move = 2,
			Attack = 2,
		},
		new MonsterStats()
		{
			Health = 9,
			Move = 2,
			Attack = 3,
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 3,
			Attack = 3,
		},
		new MonsterStats()
		{
			Health = 12,
			Move = 3,
			Attack = 3,
			Traits = [new RetaliateTrait(1)]
		},
		new MonsterStats()
		{
			Health = 13,
			Move = 3,
			Attack = 4,
			Traits = [new RetaliateTrait(1)]
		},
		new MonsterStats()
		{
			Health = 16,
			Move = 3,
			Attack = 4,
			Traits = [new RetaliateTrait(1)]
		},
		new MonsterStats()
		{
			Health = 19,
			Move = 3,
			Attack = 5,
			Traits = [new RetaliateTrait(2)]
		},
	];

	public override MonsterStats[] EliteLevelStats =>
	[
		new MonsterStats()
		{
			Health = 9,
			Move = 1,
			Attack = 3,
			Traits = [new RetaliateTrait(1)]
		},
		new MonsterStats()
		{
			Health = 10,
			Move = 2,
			Attack = 3,
			Traits = [new RetaliateTrait(2)]
		},
		new MonsterStats()
		{
			Health = 12,
			Move = 2,
			Attack = 4,
			Traits = [new RetaliateTrait(2)]
		},
		new MonsterStats()
		{
			Health = 15,
			Move = 2,
			Attack = 4,
			Traits = [new RetaliateTrait(3)]
		},
		new MonsterStats()
		{
			Health = 17,
			Move = 2,
			Attack = 5,
			Traits = [new RetaliateTrait(3)]
		},
		new MonsterStats()
		{
			Health = 19,
			Move = 2,
			Attack = 5,
			Traits = [new RetaliateTrait(4)]
		},
		new MonsterStats()
		{
			Health = 21,
			Move = 3,
			Attack = 5,
			Traits = [new RetaliateTrait(4)]
		},
		new MonsterStats()
		{
			Health = 23,
			Move = 3,
			Attack = 6,
			Traits = [new RetaliateTrait(4)]
		},
	];

	public override string Name => "Inox Guard";

	public override string AssetPath => "res://Content/Monsters/InoxGuard";

	public override int MaxStandeeCount => 4;

	public override IEnumerable<MonsterAbilityCardModel> Deck => GuardAbilityCard.Deck;
}
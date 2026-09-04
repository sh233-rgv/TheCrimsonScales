using System.Collections.Generic;

public class HarrowerAegis : MonsterModel
{
	public override MonsterStats[] NormalLevelStats =>
	[
		new MonsterStats()
		{
			Health = 8,
			Move = 1,
			Attack = 1,
			Traits = [new RetaliateTrait(1, 2)]
		},
		new MonsterStats()
		{
			Health = 11,
			Move = 1,
			Attack = 2,
			Traits = [new RetaliateTrait(1, 2)]
		},
		new MonsterStats()
		{
			Health = 14,
			Move = 1,
			Attack = 2,
			Traits = [new RetaliateTrait(1, 3)]
		},
		new MonsterStats()
		{
			Health = 17,
			Move = 2,
			Attack = 2,
			Traits = [new RetaliateTrait(1, 3)]
		},
		new MonsterStats()
		{
			Health = 20,
			Move = 2,
			Attack = 3,
			Traits = [new RetaliateTrait(2, 3)]
		},
		new MonsterStats()
		{
			Health = 23,
			Move = 3,
			Attack = 3,
			Traits = [new RetaliateTrait(2, 3)]
		},
		new MonsterStats()
		{
			Health = 26,
			Move = 3,
			Attack = 4,
			Traits = [new RetaliateTrait(3, 3), new ConditionImmunityTrait(Conditions.Stun)]
		},
		new MonsterStats()
		{
			Health = 30,
			Move = 3,
			Attack = 4,
			Traits = [new RetaliateTrait(4, 3), new ConditionImmunityTrait(Conditions.Stun)]
		},
	];

	public override MonsterStats[] EliteLevelStats =>
	[
		new MonsterStats()
		{
			Health = 12,
			Move = 1,
			Attack = 2,
			Traits = [new RetaliateTrait(1, 2)]
		},
		new MonsterStats()
		{
			Health = 16,
			Move = 2,
			Attack = 2,
			Traits = [new RetaliateTrait(1, 2)]
		},
		new MonsterStats()
		{
			Health = 20,
			Move = 2,
			Attack = 2,
			Traits = [new RetaliateTrait(1, 3)]
		},
		new MonsterStats()
		{
			Health = 24,
			Move = 2,
			Attack = 3,
			Traits = [new RetaliateTrait(2, 3)]
		},
		new MonsterStats()
		{
			Health = 28,
			Move = 3,
			Attack = 4,
			Traits = [new RetaliateTrait(2, 3), new ConditionImmunityTrait(Conditions.Stun)]
		},
		new MonsterStats()
		{
			Health = 32,
			Move = 3,
			Attack = 5,
			Traits = [new RetaliateTrait(3, 3), new ConditionImmunityTrait(Conditions.Stun)]
		},
		new MonsterStats()
		{
			Health = 38,
			Move = 3,
			Attack = 6,
			Traits = [new RetaliateTrait(4, 3), new ConditionImmunityTrait(Conditions.Stun)]
		},
		new MonsterStats()
		{
			Health = 42,
			Move = 3,
			Attack = 6,
			Traits = [new RetaliateTrait(4, 3), new ConditionImmunityTrait(Conditions.Stun)]
		},
	];

	public override string Name => "Harrower Aegis";

	public override string AssetPath => "res://Content/Monsters/HarrowerAegis";
	public override string PortraitTexturePath => $"{AssetPath}/Portrait.tres";
	public override string MapIconTexturePath => $"{AssetPath}/Icon.tres";

	public override int MaxStandeeCount => 4;

	public override IEnumerable<MonsterAbilityCardModel> Deck => HarrowerAegisAbilityCard.Deck;
}
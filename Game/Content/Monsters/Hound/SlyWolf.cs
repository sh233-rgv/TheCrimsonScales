using System.Collections.Generic;
public class SlyWolf : Hound
{
	public override MonsterStats[] BossLevelStats =>
	[
		new MonsterStats()
		{
			Health = 6 * (CharacterCount - 1),
			Move = 5,
			Attack = 2,
			Traits = [new PermanentConditionTrait(Conditions.Invisible)]
		},
		new MonsterStats()
		{
			Health = 6 * (CharacterCount - 1),
			Move = 5,
			Attack = 2,
			Traits = [new RetaliateTrait(2), new PermanentConditionTrait(Conditions.Invisible)]
		},
		new MonsterStats()
		{
			Health = 7 * (CharacterCount - 1),
			Move = 5,
			Attack = 3,
			Traits = [new RetaliateTrait(2), new PermanentConditionTrait(Conditions.Invisible)]
		},
		new MonsterStats()
		{
			Health = 8 * (CharacterCount - 1),
			Move = 5,
			Attack = 4,
			Traits = [new RetaliateTrait(2), new PermanentConditionTrait(Conditions.Invisible)]
		},
		new MonsterStats()
		{
			Health = 11 * (CharacterCount - 1),
			Move = 5,
			Attack = 4,
			Traits = [new RetaliateTrait(2), new PermanentConditionTrait(Conditions.Invisible)]
		},
		new MonsterStats()
		{
			Health = 12 * (CharacterCount - 1),
			Move = 5,
			Attack = 4,
			Traits = [new RetaliateTrait(3), new PermanentConditionTrait(Conditions.Invisible)]
		},
		new MonsterStats()
		{
			Health = 15 * (CharacterCount - 1),
			Move = 6,
			Attack = 4,
			Traits = [new RetaliateTrait(3), new PermanentConditionTrait(Conditions.Invisible)]
		},
		new MonsterStats()
		{
			Health = 15 * (CharacterCount - 1),
			Move = 6,
			Attack = 5,
			Traits = [new RetaliateTrait(4), new PermanentConditionTrait(Conditions.Invisible)]
		},
	];

	public override string Name => "Sly Wolf";

	public override int MaxStandeeCount => 1;

	public override string AssetPath => "res://Content/Monsters/Hound";

	public override IEnumerable<MonsterAbilityCardModel> Deck => HoundAbilityCard.Deck;
}
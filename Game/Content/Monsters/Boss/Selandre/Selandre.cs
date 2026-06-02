using System.Collections.Generic;

public class Selandre : MonsterModel, IBossMonsterModel
{
	public override MonsterStats[] BossLevelStats =>
	[
		new MonsterStats()
		{
			Health = 6 * CharacterCount,
			Move = 2,
			Attack = 2,
			Traits =
			[
				new AllNegativeConditionImmunityTrait()
			]
		},
		new MonsterStats()
		{
			Health = 7 * CharacterCount,
			Move = 2,
			Attack = 3,
			Traits =
			[
				new AllNegativeConditionImmunityTrait()
			]
		},
		new MonsterStats()
		{
			Health = 9 * CharacterCount,
			Move = 3,
			Attack = 3,
			Traits =
			[
				new AllNegativeConditionImmunityTrait()
			]
		},
		new MonsterStats()
		{
			Health = 12 * CharacterCount,
			Move = 4,
			Attack = 4,
			Traits =
			[
				new AllNegativeConditionImmunityTrait()
			]
		},
		new MonsterStats()
		{
			Health = 13 * CharacterCount,
			Move = 4,
			Attack = 5,
			Traits =
			[
				new AllNegativeConditionImmunityTrait()
			]
		},
		new MonsterStats()
		{
			Health = 15 * CharacterCount,
			Move = 5,
			Attack = 5,
			Traits =
			[
				new AllNegativeConditionImmunityTrait()
			]
		},
		new MonsterStats()
		{
			Health = 18 * CharacterCount,
			Move = 5,
			Attack = 5,
			Traits =
			[
				new AllNegativeConditionImmunityTrait()
			]
		},
		new MonsterStats()
		{
			Health = 22 * CharacterCount,
			Move = 5,
			Attack = 5,
			Traits =
			[
				new AllNegativeConditionImmunityTrait()
			]
		},
	];

	public override string Name => "Selandre";

	public override string AssetPath => "res://Content/Monsters/Boss/Selandre";

	public override int MaxStandeeCount => 1;

	public override IEnumerable<MonsterAbilityCardModel> Deck => BossAbilityCard.Deck;

	public string GetSpecial1Description(Monster monster, RichTextParameters richTextParameters) =>
		$"""
		 {Icons.Inline(Icons.Move, richTextParameters)}2 towards the hex marked {Icons.InlineMarker(Marker.Type.b, richTextParameters)}.
		 The Ancient Artillery performs:
		 ”{Icons.Inline(Icons.Move, richTextParameters)}2, {Icons.Inline(Icons.Push, richTextParameters, ignoreParametersColor: true)}2, {Icons.Inline(Icons.Targets, richTextParameters)}all enemies within {Icons.Inline(Icons.Range, richTextParameters)}2, Attack +0, {Icons.Inline(Icons.Targets, richTextParameters)}all enemies within range.”
		 """;

	public string GetSpecial2Description(Monster monster, RichTextParameters richTextParameters) =>
		$"""
		 {Icons.Inline(Icons.Move, richTextParameters)}{CharacterCount + 1}, Self.
		 The Ancient Artillery performs:
		 ”{Icons.Inline(Icons.Attack, richTextParameters)}{GetAncientArtillery().Stats.Attack}, {Icons.Inline(Icons.Targets, richTextParameters)}all adjacent enemies, {Icons.Inline(Icons.Attack, richTextParameters)}+X, {Icons.Inline(Icons.Targets, richTextParameters)}all enemies at {Icons.Inline(Icons.Range, richTextParameters)}2 or higher, where X is the number of hexes between the Artillery and the target.”
		 """;

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster) =>
	[
		//TODO
	];

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster) =>
	[
		//TODO
	];

	private Monster GetAncientArtillery()
	{
		Scenario032 scenario = (Scenario032)GameController.Instance.ScenarioModel;
		return scenario.AncientArtillery;
	}
}
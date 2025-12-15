using System.Collections.Generic;
using System.Linq;
public class SlyWolf : Hound
{
	public override MonsterStats[] NamedLevelStats =>
		base.EliteLevelStats
			.Select(stats => stats with
			{
				Health = stats.Health * (CharacterCount - 1),
				Traits = (stats.Traits ?? [])
					.Append(new PermanentConditionTrait(Conditions.Invisible))
					.ToArray()
			})
			.ToArray();

	public override string Name => "Sly Wolf";

	public override int MaxStandeeCount => 1;

	public override string AssetPath => "res://Content/Monsters/Hound";

	public override IEnumerable<MonsterAbilityCardModel> Deck => HoundAbilityCard.Deck;
}
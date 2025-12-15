using System.Collections.Generic;
using System.Linq;

public class GelatinousGiantSecondStage : GelatinousGiant
{
	public override MonsterStats[] BossLevelStats =>
		base.EliteLevelStats
			.Select(stats => stats with
			{
				Health = stats.Health * CharacterCount,
				Traits = (stats.Traits ?? [])
					.ToArray()
			})
			.ToArray();

	public override IEnumerable<MonsterAbilityCardModel> Deck => BloodOozeAbilityCard.Deck;
}
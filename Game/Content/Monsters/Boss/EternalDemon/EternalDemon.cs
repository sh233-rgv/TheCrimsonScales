using System;
using System.Collections.Generic;
using System.Linq;

public class EternalDemon : EarthDemon, IBossMonsterModel
{
	public override MonsterStats[] BossLevelStats =>
		base.EliteLevelStats
			.Select(stats => stats with
			{
				Health = stats.Health * CharacterCount,
				Move = stats.Move + 2,
				Traits = (stats.Traits ?? [])
				.Append(new AllNegativeConditionImmunityTrait())
				.ToArray()
			})
			.ToArray();

	public override string Name => "Eternal Demon";
	public override MonsterModel ParentMonsterModel => ModelDB.Monster<EarthDemon>();

	public string GetSpecial1Description(Monster monster, RichTextParameters richTextParameters) =>
		$"""
		 {Icons.Inline(Icons.Teleport, richTextParameters)} to a map tile occupied by the least amount of characters. {Icons.Inline(Icons.Heal, richTextParameters)} X, Self, where X is equal to the number of characters on different map tiles not occupied by the Eternal Demon.
		 """;

	public string GetSpecial2Description(Monster monster, RichTextParameters richTextParameters) =>
		$"""
		 {Icons.Inline(Icons.Attack, richTextParameters)}+1, Target all enemies sharing the same map tile.
		 """;

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster)
	{
		//TODO
		throw new NotImplementedException();
	}

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster)
	{
		//TODO
		throw new NotImplementedException();
	}
}
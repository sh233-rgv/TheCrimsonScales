using System;
using System.Collections.Generic;

public interface IBossMonsterModel
{
	Action<ScenarioCheckEvents.FigureFocusCheck.Parameters> AdjustFocusSpecial1 => null;
	IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster);
	Action<ScenarioCheckEvents.FigureFocusCheck.Parameters> AdjustFocusSpecial2 => null;
	IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster);
	string GetSpecial1Description(Monster monster);
	string GetSpecial2Description(Monster monster);
}
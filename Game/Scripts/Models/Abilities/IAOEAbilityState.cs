using System.Collections.Generic;

public interface IAOEAbilityState
{
	public AOEPattern AbilityAOEPattern { get; }

	public IEnumerable<Hex> GetRedAOEHexes();
}
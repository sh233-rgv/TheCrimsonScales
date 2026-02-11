using System.Collections.Generic;

public abstract class PerkModel : AbstractModel
{
	public virtual List<AMDCardModel> CardsToRemove => [];
	public virtual List<AMDCardModel> CardsToAdd => [];

	public virtual bool IgnoreNegativeScenarioEffects => false;
	public virtual bool IgnoreNegativeItemEffects => false;
}
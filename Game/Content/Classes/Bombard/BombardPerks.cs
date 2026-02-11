using System.Collections.Generic;

public class BombardPerks
{
	public class BombardPerk : PerkModel
	{
	}

	public class RemoveTwoMinusOnes : BombardPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } = [ModelDB.AMDCard<MinusOneAMDCard>(), ModelDB.AMDCard<MinusOneAMDCard>()];
	}
}
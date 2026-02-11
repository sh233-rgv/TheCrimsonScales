using System.Collections.Generic;

public class BombardPerks
{
	public abstract class BombardPerk : PerkModel
	{
	}

	public class RemoveTwoMinusOnes : BombardPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } = [ModelDB.AMDCard<MinusOneAMDCard>(), ModelDB.AMDCard<MinusOneAMDCard>()];
	}

	public class ReplaceTwoPlusZeroWithTwoRollingPierceThree : BombardPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } = [ModelDB.AMDCard<PlusZeroAMDCard>(), ModelDB.AMDCard<PlusZeroAMDCard>()];

		public override List<AMDCardModel> CardsToAdd { get; } =
			[ModelDB.AMDCard<BombardAMDCards.RollingPierceThree>(), ModelDB.AMDCard<BombardAMDCards.RollingPierceThree>()];
	}

	public class ReplaceOneMinusOneWithOnePlusZeroPlusThreeIfProjectile : BombardPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } = [ModelDB.AMDCard<MinusOneAMDCard>()];

		public override List<AMDCardModel> CardsToAdd { get; } = [ModelDB.AMDCard<BombardAMDCards.PlusZeroPlusThreeIfProjectile>()];
	}
}
using System.Collections.Generic;

public class BombardPerks
{
	public abstract class BombardPerk : PerkModel
	{
	}

	public class RemoveTwoMinusOnes : BombardPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusOneAMDCard>(),
			ModelDB.AMDCard<MinusOneAMDCard>()
		];
	}

	public class ReplaceTwoPlusZeroWithTwoRollingPierceThree : BombardPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<PlusZeroAMDCard>(),
			ModelDB.AMDCard<PlusZeroAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<BombardAMDCards.RollingPierceThree>(),
			ModelDB.AMDCard<BombardAMDCards.RollingPierceThree>()
		];
	}

	public class ReplaceOneMinusOneWithOnePlusZeroPlusThreeIfProjectile : BombardPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusOneAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<BombardAMDCards.PlusZeroPlusThreeIfProjectile>()
		];
	}

	public class AddTwoPlusTwoImmobilize : BombardPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<BombardAMDCards.PlusTwoImmobilize>(),
			ModelDB.AMDCard<BombardAMDCards.PlusTwoImmobilize>()
		];
	}

	public class ReplaceOnePlusOneWithTwoPlusOneRetaliateOne : BombardPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<PlusOneAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<BombardAMDCards.PlusOneRetaliateOne>(),
			ModelDB.AMDCard<BombardAMDCards.PlusOneRetaliateOne>()
		];
	}

	public class AddTwoPlusOnePullSelfTowardTarget : BombardPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<BombardAMDCards.PlusOnePullSelfTowardTarget>(),
			ModelDB.AMDCard<BombardAMDCards.PlusOnePullSelfTowardTarget>()
		];
	}

	public class AddOnePlusZeroStrengthenSelf : BombardPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<BombardAMDCards.PlusZeroStrengthenSelf>()
		];
	}

	public class AddOnePlusZeroStun : BombardPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<BombardAMDCards.PlusZeroStun>()
		];
	}

	public class AddOnePlusOneWound : BombardPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<BombardAMDCards.PlusOneWound>()
		];
	}

	public class AddTwoRollingPlusZeroShieldOne : BombardPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<BombardAMDCards.RollingPlusZeroShieldOne>(),
			ModelDB.AMDCard<BombardAMDCards.RollingPlusZeroShieldOne>()
		];
	}

	public class AddTwoRollingPlusZeroHealOneSelf : BombardPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<BombardAMDCards.RollingPlusZeroHealOneSelf>(),
			ModelDB.AMDCard<BombardAMDCards.RollingPlusZeroHealOneSelf>()
		];
	}

	public class IgnoreNegativeScenarioEffectsRemovePlusZero : BombardPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<PlusZeroAMDCard>()
		];

		public override bool IgnoreNegativeScenarioEffects => true;
	}

	public class IgnoreNegativeItemEffectsRemovePlusZero : BombardPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<PlusZeroAMDCard>()
		];

		public override bool IgnoreNegativeItemEffects => true;
	}
}
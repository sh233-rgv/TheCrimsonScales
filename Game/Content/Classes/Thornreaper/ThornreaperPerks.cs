using System.Collections.Generic;
using Fractural.Tasks;

public class ThornreaperPerks
{
	public abstract class ThornreaperPerk : PerkModel
	{
	}

	public class ReplaceOneMinusTwoWithOneMinusOneInvisibleSelf : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusTwoAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.MinusOneInvisibleSelf>()
		];
	}

	public class ReplaceOneMinusOneWithOnePlusZeroImmobilizeRolling : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusOneAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroImmobilizeRolling>()
		];
	}

	public class ReplaceOneMinusOneWithOnePlusZeroControlTargetMoveOneRolling : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusOneAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroControlTargetMoveOneRolling>()
		];
	}

	public class ReplaceTwoPlusZeroWithOnePlusOneLight : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<PlusZeroAMDCard>(),
			ModelDB.AMDCard<PlusZeroAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusOneLight>()
		];
	}

	public class AddTwoPlusOneHealOneRangeThree : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusOneHealOneRangeThree>(),
			ModelDB.AMDCard<ThornreaperAMDCards.PlusOneHealOneRangeThree>()
		];
	}

	public class AddOnePlusOneIfYouAreUndamagedPlusThreeInstead : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusOneIfYouAreUndamagedPlusThreeInstead>()
		];
	}

	public class AddTwoPlusZeroHealOneRangeOneRolling : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroHealOneRangeOneRolling>(),
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroHealOneRangeOneRolling>()
		];
	}

	public class AddOnePlusZeroLootOneRolling : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroLootOneRolling>()
		];
	}

	public class IgnoreScenarioEffectsAddOnePlusZeroDark : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroDark>()
		];

		public override bool IgnoreScenarioEffects => true;
	}

	public class ReflectiveJourney : ThornreaperPerk
	{
		public override int PerkBoxCount => 2;
		protected override string Title => "Reflective Journey";

		public override string GetNonAMDDescription(RichTextParameters richTextParameters) =>
			$"At the end of each of your long rests, {Icons.Inline(Icons.GetElement(Element.Light), richTextParameters)} or {Icons.Inline(Icons.GetElement(Element.Dark), richTextParameters)}. If you are undamaged, grant one ally: {Icons.Inline(Icons.Move, richTextParameters)}2.";

		public override async GDTask OnScenarioSetupPhaseCompleted(Character character)
		{
			await base.OnScenarioSetupPhaseCompleted(character);

			ScenarioEvents.LongRestEndedEvent.Subscribe(this,
				parameters => parameters.Character == character,
				async parameters =>
				{
					await AbilityCmd.InfuseElement(null, [Element.Light, Element.Dark], character);
					if(!character.IsDamaged())
					{
						await new ActionState(character,
						[
							GrantAbility.Builder()
								.WithAbilities(
								[
									MoveAbility.Builder().WithDistance(2).Build()
								])
								.WithRange(RangeHelper.InfiniteRange)
								.Build()
						]).Perform();
					}
				});
		}
	}
}
using System.Collections.Generic;
using Fractural.Tasks;

public class RimehearthPerks
{
	public abstract class RimehearthPerk : PerkModel
	{
	}

	public class ReplaceOneMinusTwoWithOneMinusOneInvisibleSelf : RimehearthPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusTwoAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<StarslingerAMDCards.MinusOneInvisibleSelf>()
		];
	}

	public class ReplaceOneMinusOneWithOnePlusZeroImmobilizeRolling : RimehearthPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusOneAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<StarslingerAMDCards.PlusZeroImmobilizeRolling>()
		];
	}

	public class ReplaceOneMinusOneWithOnePlusZeroControlTargetMoveOneRolling : RimehearthPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusOneAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<StarslingerAMDCards.PlusZeroControlTargetMoveOneRolling>()
		];
	}

	public class ReplaceTwoPlusZeroWithOnePlusOneLight : RimehearthPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<PlusZeroAMDCard>(),
			ModelDB.AMDCard<PlusZeroAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<StarslingerAMDCards.PlusOneLight>()
		];
	}

	public class AddTwoPlusOneHealOneRangeThree : RimehearthPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<StarslingerAMDCards.PlusOneHealOneRangeThree>(),
			ModelDB.AMDCard<StarslingerAMDCards.PlusOneHealOneRangeThree>()
		];
	}

	public class AddOnePlusOneIfYouAreUndamagedPlusThreeInstead : RimehearthPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<StarslingerAMDCards.PlusOneIfYouAreUndamagedPlusThreeInstead>()
		];
	}

	public class AddTwoPlusZeroHealOneRangeOneRolling : RimehearthPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<StarslingerAMDCards.PlusZeroHealOneRangeOneRolling>(),
			ModelDB.AMDCard<StarslingerAMDCards.PlusZeroHealOneRangeOneRolling>()
		];
	}

	public class AddOnePlusZeroLootOneRolling : RimehearthPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<StarslingerAMDCards.PlusZeroLootOneRolling>()
		];
	}

	public class IgnoreScenarioEffectsAddOnePlusZeroDark : RimehearthPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<StarslingerAMDCards.PlusZeroDark>()
		];

		public override bool IgnoreScenarioEffects => true;
	}

	public class ReflectiveJourney : RimehearthPerk, IEventSubscriber
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
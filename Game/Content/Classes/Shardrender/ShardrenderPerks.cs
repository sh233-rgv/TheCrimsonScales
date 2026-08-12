using System.Collections.Generic;
using Fractural.Tasks;

public class ShardrenderPerks
{
	public abstract class ShardrenderPerk : PerkModel
	{
	}

	public class ReplaceOneMinusTwoWithOneMinusOneInvisibleSelf : ShardrenderPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusTwoAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ShardrenderAMDCards.MinusOneInvisibleSelf>()
		];
	}

	public class ReplaceOneMinusOneWithOnePlusZeroImmobilizeRolling : ShardrenderPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusOneAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ShardrenderAMDCards.PlusZeroImmobilizeRolling>()
		];
	}

	public class ReplaceOneMinusOneWithOnePlusZeroControlTargetMoveOneRolling : ShardrenderPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusOneAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ShardrenderAMDCards.PlusZeroControlTargetMoveOneRolling>()
		];
	}

	public class ReplaceTwoPlusZeroWithOnePlusOneLight : ShardrenderPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<PlusZeroAMDCard>(),
			ModelDB.AMDCard<PlusZeroAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ShardrenderAMDCards.PlusOneLight>()
		];
	}

	public class AddTwoPlusOneHealOneRangeThree : ShardrenderPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ShardrenderAMDCards.PlusOneHealOneRangeThree>(),
			ModelDB.AMDCard<ShardrenderAMDCards.PlusOneHealOneRangeThree>()
		];
	}

	public class AddOnePlusOneIfYouAreUndamagedPlusThreeInstead : ShardrenderPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ShardrenderAMDCards.PlusOneIfYouAreUndamagedPlusThreeInstead>()
		];
	}

	public class AddTwoPlusZeroHealOneRangeOneRolling : ShardrenderPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ShardrenderAMDCards.PlusZeroHealOneRangeOneRolling>(),
			ModelDB.AMDCard<ShardrenderAMDCards.PlusZeroHealOneRangeOneRolling>()
		];
	}

	public class AddOnePlusZeroLootOneRolling : ShardrenderPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ShardrenderAMDCards.PlusZeroLootOneRolling>()
		];
	}

	public class IgnoreScenarioEffectsAddOnePlusZeroDark : ShardrenderPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ShardrenderAMDCards.PlusZeroDark>()
		];

		public override bool IgnoreScenarioEffects => true;
	}

	public class ReflectiveJourney : ShardrenderPerk, IEventSubscriber
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
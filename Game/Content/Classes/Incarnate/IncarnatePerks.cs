using System.Collections.Generic;
using Fractural.Tasks;

public class IncarnatePerks
{
	public abstract class IncarnatePerk : PerkModel
	{
	}

	public class RemoveOneMinusTwo : IncarnatePerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusTwoAMDCard>()
		];
	}

	public class ReplaceOneMinusOneWithOnePlusZeroRupture : IncarnatePerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusOneAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<IncarnateAMDCards.PlusZeroRupture>()
		];
	}

	public class ReplaceOneMinusOneWithOnePlusZeroWound : IncarnatePerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusOneAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<IncarnateAMDCards.PlusZeroWound>()
		];
	}

	public class ReplaceOnePlusZeroWithOnePlusOnePlusThreeInsteadIfTargetHasRuptureOrWound : IncarnatePerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<PlusZeroAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<IncarnateAMDCards.PlusOnePlusThreeInsteadIfTargetHasRuptureOrWound>()
		];
	}

	public class ReplaceOnePlusZeroWithOnePlusZeroHealOneEmpowerSelfRolling : IncarnatePerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<PlusZeroAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<IncarnateAMDCards.PlusZeroHealOneEmpowerSelfRolling>()
		];
	}

	public class AddOnePlusTwoIfThisAttackKillsTargetGainMoneyTokenDirectly : IncarnatePerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<IncarnateAMDCards.PlusTwoIfThisAttackKillsTargetGainMoneyTokenDirectly>()
		];
	}

	public class IgnoreScenarioEffectsRemoveOneMinusOne : IncarnatePerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusOneAMDCard>()
		];

		public override bool IgnoreScenarioEffects => true;
	}

	public class FollowTheScent : IncarnatePerk, IEventSubscriber
	{
		protected override string Title => "Follow the Scent";

		public override string GetNonAMDDescription(RichTextParameters richTextParameters) =>
			$"Whenever one of your abilities causes an enemy to gain {Icons.Inline(Icons.GetCondition(Conditions.Rupture), richTextParameters)}, immediately after the ability, perform {Icons.Inline(Icons.Move, richTextParameters)}2.";

		public override async GDTask OnScenarioSetupPhaseCompleted(Character character)
		{
			await base.OnScenarioSetupPhaseCompleted(character);

			List<AbilityState> currentAbilityStates = [];
			ScenarioEvents.InflictConditionEvent.Subscribe(this,
				parameters => parameters.PotentialConditionGiver == character && parameters.Target.EnemiesWith(character) &&
				              parameters.ConditionModel is Rupture && !currentAbilityStates.Contains(parameters.PotentialAbilityState),
				async parameters =>
				{
					currentAbilityStates.Add(parameters.PotentialAbilityState);
					ScenarioEvents.AbilityEndedEvent.Subscribe(this,
						abilityEndedParameters => abilityEndedParameters.AbilityState == parameters.PotentialAbilityState,
						async _ =>
						{
							await new ActionState(character, [MoveAbility.Builder().WithDistance(2).Build()]).Perform();
							currentAbilityStates.Remove(parameters.PotentialAbilityState);
							ScenarioEvents.AbilityEndedEvent.Unsubscribe(this);
						});
					await GDTask.CompletedTask;
				});
		}
	}

	public class ALullInHunger : IncarnatePerk, IEventSubscriber
	{
		protected override string Title => "A Lull in Hunger";

		public override string GetNonAMDDescription(RichTextParameters richTextParameters) =>
			$"Once each scenario, after you loot a money token, {Icons.Inline(Incarnate.SatedUpIconPath, richTextParameters)}.";

		public override async GDTask OnScenarioSetupPhaseCompleted(Character character)
		{
			await base.OnScenarioSetupPhaseCompleted(character);

			ScenarioEvents.CoinLootedEvent.Subscribe(this,
				parameters => parameters.LootObtainer == character,
				async parameters =>
				{
					await IncarnateCardSide.SateIncarnate(character);
				}, EffectType.Selectable,
				effectButtonParameters: new IconEffectButton.Parameters(Incarnate.SatedUpIconPath),
				effectInfoViewParameters: new TextEffectInfoView.Parameters(Icons.Inline(Incarnate.SatedUpIconPath)));
		}
	}

	public class AdrenalineRush : IncarnatePerk
	{
		public override int PerkBoxCount => 2;
		protected override string Title => "Adrenaline Rush";

		public override string GetNonAMDDescription(RichTextParameters richTextParameters) =>
			$"Whenever you become {Icons.Inline(Incarnate.SatedIconPath, richTextParameters)}, gain {Icons.Inline(Icons.GetCondition(Conditions.Ward))}.";

		public override async GDTask OnScenarioSetupPhaseCompleted(Character character)
		{
			await base.OnScenarioSetupPhaseCompleted(character);

			if(character is Incarnate IncarnateCharacter)
			{
				IncarnateCharacter.SateEvent += async Incarnate => await AbilityCmd.AddCondition(null, Incarnate, Conditions.Ward);
			}
		}
	}
}
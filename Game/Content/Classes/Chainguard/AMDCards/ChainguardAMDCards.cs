using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class ChainguardAMDCards
{
	public class MinusTwoRecoverRandomCardFromDiscard : BrightsparkAMDCardModel
	{
		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, -2,
				extraText: $"{Icons.Inline(Icons.RecoverCard)} one random card from your discard pile");

		protected override int AtlasIndex => 0;
		public override int? GetValue(AttackAbility.State attackAbilityState) => -2;

		public override Func<AttackAbility.State, GDTask> GetExtraEffects(AttackAbility.State attackAbilityState) =>
			async state =>
			{
				List<AbilityCard> discardedCards = ((Character)state.Performer).Cards.Where(card => card.CardState is CardState.Discarded).ToList();
				if(!discardedCards.Any())
				{
					return;
				}

				AbilityCard card = discardedCards.PickRandom(GameController.Instance.StateRNG);
				if(card == null)
				{
					return;
				}

				await AbilityCmd.ReturnToHand(card);
			};
	}

	public class PlusZeroConsumeElementForPlusTwo : BrightsparkAMDCardModel
	{
		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +0,
				extraText: $"Consume {Icons.Inline(Icons.WildElement)} to add +2{Icons.Inline(Icons.Attack)}");

		protected override int AtlasIndex => 1;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;

		public override Func<AttackAbility.State, GDTask> GetExtraEffects(AttackAbility.State attackAbilityState) =>
			async state =>
			{
				await AbilityCmd.GenericChoice(state.Authority,
				[
					ScenarioEvent<ScenarioEvents.GenericChoice.Parameters>.Subscription.ConsumeWildElement(applyFunction: async _ =>
						{
							state.SingleTargetAdjustAttackValue(2);
							await GDTask.CompletedTask;
						}, effectInfoViewParameters: new TextEffectInfoView.Parameters($"+2{Icons.Inline(Icons.Attack)}"),
						potentialConsumer: state.Performer)
				]);
			};
	}

	public class PlusOneHealOneAllyRangeTwo : BrightsparkAMDCardModel
	{
		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +1,
				extraText: $"{Icons.Inline(Icons.Heal)}1, {Icons.Inline(Icons.Targets)}1 ally, {Icons.Inline(Icons.Range)}2");

		protected override int AtlasIndex => 4;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +1;

		public override List<Ability> GetAbilities(AttackAbility.State attackAbilityState) =>
		[
			HealAbility.Builder().WithHealValue(1).WithTarget(Target.Allies).WithRange(2).Build()
		];
	}

	public class PlusOneGrantOneAllyWithinRangeTwoShieldOne : BrightsparkAMDCardModel
	{
		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +1,
				extraText: $"Grant one ally within {Icons.Inline(Icons.Range)}2 {Icons.Inline(Icons.Shield)}1");

		protected override int AtlasIndex => 4;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +1;

		public override List<Ability> GetAbilities(AttackAbility.State attackAbilityState) =>
		[
			GrantAbility.Builder()
				.WithAbilities(
				[
					ShieldAbility.Builder().WithShieldValue(1).Build()
				])
				.WithRange(2)
				.Build()
		];
	}

	public class PlusZeroConsumeElementToInfuseElementRolling : BrightsparkAMDCardModel
	{
		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +0,
				extraText: $"Consume {Icons.Inline(Icons.WildElement)} to {Icons.Inline(Icons.WildElement)}", rolling: true);

		protected override int AtlasIndex => 7;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;
		public override List<CardElementInfusion> ElementInfusions => [CardElementInfusion.ConsumeWildInfuseWild()];

		public override Func<AttackAbility.State, GDTask> GetExtraEffects(AttackAbility.State attackAbilityState) =>
			async state =>
			{
				await AbilityCmd.GenericChoice(state.Authority,
				[
					ScenarioEvent<ScenarioEvents.GenericChoice.Parameters>.Subscription.ConsumeWildElement(applyFunction: async _ =>
						{
							await AbilityCmd.InfuseWildElement(state);
						}, effectInfoViewParameters: new TextEffectInfoView.Parameters($"Infuse {Icons.Inline(Icons.WildElement)}"),
						potentialConsumer: state.Performer)
				]);
			};
	}

	public class PlusTwoWildElement : BrightsparkAMDCardModel
	{
		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +2,
				extraText: $"{Icons.Inline(Icons.WildElement)}");

		protected override int AtlasIndex => 10;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +2;
		public override List<CardElementInfusion> ElementInfusions => [CardElementInfusion.InfuseWild()];
	}

	public class PlusOneStrengthenAllyRangeTwo : BrightsparkAMDCardModel
	{
		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +1,
				extraText:
				$"{Icons.Inline(Icons.GetCondition(Conditions.Strengthen))}, {Icons.Inline(Icons.Targets)}1 ally, {Icons.Inline(Icons.Range)}2");

		protected override int AtlasIndex => 12;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +1;

		public override List<Ability> GetAbilities(AttackAbility.State attackAbilityState) =>
		[
			ConditionAbility.Builder().WithConditions(Conditions.Strengthen).WithTarget(Target.Allies).WithRange(2).Build()
		];
	}

	public class PlusZeroImmobilizeIceRolling : BrightsparkAMDCardModel
	{
		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +0, [Conditions.Immobilize],
				extraText: Icons.Inline(Icons.GetElement(Element.Ice)), rolling: true);

		protected override int AtlasIndex => 14;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;
		public override List<ConditionModel> GetConditionModels(AttackAbility.State attackAbilityState) => [Conditions.Immobilize];
		public override List<CardElementInfusion> ElementInfusions => [CardElementInfusion.Infuse(Element.Ice)];
	}

	public class PlusZeroPushOneOrPullOneAirRolling : BrightsparkAMDCardModel
	{
		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +0,
				extraText: $"{Icons.Inline(Icons.Push)}1 or {Icons.Inline(Icons.Pull)}1, {Icons.Inline(Icons.GetElement(Element.Air))}",
				rolling: true);

		protected override int AtlasIndex => 15;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;
		public override List<CardElementInfusion> ElementInfusions => [CardElementInfusion.Infuse(Element.Air)];

		public override Func<AttackAbility.State, GDTask> GetExtraEffects(AttackAbility.State attackAbilityState) =>
			async state =>
			{
				await AbilityCmd.GenericChoice(state.Authority,
				[
					ScenarioEvent<ScenarioEvents.GenericChoice.Parameters>.Subscription.New(applyFunction: async _ =>
						{
							state.SingleTargetAdjustPush(1);
							await GDTask.CompletedTask;
						}, effectType: EffectType.SelectableMandatory,
						effectButtonParameters: new IconEffectButton.Parameters(Icons.Push),
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.Push)}1")),
					ScenarioEvent<ScenarioEvents.GenericChoice.Parameters>.Subscription.New(applyFunction: async _ =>
						{
							state.SingleTargetAdjustPull(1);
							await GDTask.CompletedTask;
						}, effectType: EffectType.SelectableMandatory,
						effectButtonParameters: new IconEffectButton.Parameters(Icons.Pull),
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.Pull)}1")),
				]);
			};
	}

	public class PlusZeroPierceTwoFireRolling : BrightsparkAMDCardModel
	{
		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +0,
				extraText: $"{Icons.Inline(Icons.Pierce)}2, {Icons.Inline(Icons.GetElement(Element.Fire))}", rolling: true);

		protected override int AtlasIndex => 16;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;
		public override int? Pierce => 2;
		public override List<CardElementInfusion> ElementInfusions => [CardElementInfusion.Infuse(Element.Fire)];
	}

	public class PlusZeroHealOneRangeThreeLightRolling : BrightsparkAMDCardModel
	{
		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +0,
				extraText: $"{Icons.Inline(Icons.Heal)}1, {Icons.Inline(Icons.Range)}3, {Icons.Inline(Icons.GetElement(Element.Light))}",
				rolling: true);

		protected override int AtlasIndex => 17;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;
		public override List<CardElementInfusion> ElementInfusions => [CardElementInfusion.Infuse(Element.Light)];

		public override List<Ability> GetAbilities(AttackAbility.State attackAbilityState) =>
		[
			HealAbility.Builder().WithHealValue(1).WithRange(3).Build()
		];
	}
}
using System;
using System.Collections.Generic;
using Fractural.Tasks;

public class IncarnateAMDCards
{
	public class PlusZeroRupture : IncarnateAMDCardModel
	{
		protected override int AtlasIndex => 0;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;
		public override List<ConditionModel> GetConditionModels(AttackAbility.State attackAbilityState) => [Conditions.Rupture];
	}

	public class PlusZeroWound : IncarnateAMDCardModel
	{
		protected override int AtlasIndex => 1;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;
		public override List<ConditionModel> GetConditionModels(AttackAbility.State attackAbilityState) => [Conditions.Wound1];
	}

	public class PlusOnePlusThreeInsteadIfTargetHasRuptureOrWound : IncarnateAMDCardModel
	{
		public override string GetSimpleString(RichTextParameters richTextParameters) =>
			GetSimpleString(richTextParameters, +1,
				$"{Icons.InlineCondition(Conditions.Wound1, richTextParameters)}:{Icons.Inline(Icons.GetAMDValue("+3"), richTextParameters)}");

		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +1,
				extraText:
				$"If the target has {Icons.Inline(Icons.GetCondition(Conditions.Rupture), richTextParameters)} or {Icons.Inline(Icons.GetCondition(Conditions.Wound1), richTextParameters)}, {Icons.Inline(Icons.GetAMDValue("+3"), richTextParameters)} instead");

		protected override int AtlasIndex => 2;

		public override int? GetValue(AttackAbility.State attackAbilityState) =>
			attackAbilityState?.Target.HasWound() == true || attackAbilityState?.Target.HasCondition(Conditions.Rupture) == true ? +3 : +1;
	}

	public class PlusZeroHealOneEmpowerSelfRolling : IncarnateAMDCardModel
	{
		public override string GetSimpleString(RichTextParameters richTextParameters) =>
			GetSimpleString(richTextParameters, +0,
				$"{Icons.Inline(Icons.Heal, richTextParameters)}1 {Icons.Inline(Icons.Rolling, richTextParameters)}");

		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +0,
				extraText: $"{Icons.Inline(Icons.Heal, richTextParameters)}1, {Icons.Inline(Icons.GetCondition(Incarnate.Empower))}, self",
				rolling: true);

		protected override int AtlasIndex => 3;
		public override bool GetRolling(AttackAbility.State attackAbilityState) => true;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;

		public override List<Ability> GetAbilities(AttackAbility.State attackAbilityState) =>
		[
			HealAbility.Builder().WithHealValue(1).WithTarget(Target.Self).WithConditions(Incarnate.Empower).Build()
		];
	}

	public class PlusTwoIfThisAttackKillsTargetGainMoneyTokenDirectly : IncarnateAMDCardModel
	{
		public override string GetSimpleString(RichTextParameters richTextParameters) =>
			GetSimpleString(richTextParameters, +2, $"{Icons.Inline(Icons.Coins, richTextParameters)}");

		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +2,
				extraText: "If this attack kills the target, gain the money token directly");

		protected override int AtlasIndex => 4;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +2;

		public override Func<AttackAbility.State, Figure, GDTask> GetExtraEffects() =>
			async (state, _) =>
			{
				Figure target = state.Target;
				int coinsToLoot = 0;
				ScenarioCheckEvents.SpawnCoinCheckEvent.Subscribe(state, this,
					canApplyParameters => target == canApplyParameters.Dropper && canApplyParameters.CoinsToSpawn > 0,
					applyParameters =>
					{
						coinsToLoot = applyParameters.CoinsToSpawn;
						applyParameters.SetCoinsToSpawn(0);
					}, order: 100
				);
				ScenarioEvents.AfterAttackPerformedEvent.Subscribe(state, this,
					parameters => target.IsDead && coinsToLoot > 0,
					async parameters =>
					{
						List<Coin> coins = [];

						for(int i = 0; i < coinsToLoot; i++)
						{
							coins.AddRange(await AbilityCmd.SpawnCoin(target.Hex));
						}

						foreach(Coin coin in coins)
						{
							await coin.Loot(state.Performer);
						}

						coinsToLoot = 0;
					});

				await GDTask.CompletedTask;
			};
	}
}
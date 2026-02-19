using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class ArtificerAMDCards
{
	public class MinusOneGainScrap : ArtificerAMDCardModel
	{
		protected override int AtlasIndex => 0;
		public override int? GetValue(AttackAbility.State attackAbilityState) => -1;

		public override Func<AttackAbility.State, GDTask> GetExtraEffects(AttackAbility.State attackAbilityState) =>
			async state =>
			{
				await ArtificerCardSide.GainScrapToken(state);
			};
	}

	public class PlusOneGainScrap : ArtificerAMDCardModel
	{
		protected override int AtlasIndex => 1;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +1;

		public override Func<AttackAbility.State, GDTask> GetExtraEffects(AttackAbility.State attackAbilityState) =>
			async state =>
			{
				await ArtificerCardSide.GainScrapToken(state);
			};
	}

	public class PlusThreeDisarmGainScrap : ArtificerAMDCardModel
	{
		protected override int AtlasIndex => 2;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +3;
		public override List<ConditionModel> GetConditionModels(AttackAbility.State attackAbilityState) => [Conditions.Disarm];

		public override Func<AttackAbility.State, GDTask> GetExtraEffects(AttackAbility.State attackAbilityState) =>
			async state =>
			{
				await ArtificerCardSide.GainScrapToken(state);
			};
	}

	public class PlusOne : ArtificerAMDCardModel
	{
		protected override int AtlasIndex => 3;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +1;
	}

	public class PlusZeroPierceTwoRolling : ArtificerAMDCardModel
	{
		protected override int AtlasIndex => 5;
		public override bool GetRolling(AttackAbility.State attackAbilityState) => true;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;
		public override int? Pierce => 3;
	}

	public class PlusOneWoundIfDrawnBySummon : ArtificerAMDCardModel
	{
		protected override int AtlasIndex => 9;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +1;

		public override List<ConditionModel> GetConditionModels(AttackAbility.State attackAbilityState) =>
			attackAbilityState.Performer is Summon ? [Conditions.Wound1] : [];
	}

	public class PlusZeroCreateDamageTwoTrapRolling : ArtificerAMDCardModel
	{
		protected override int AtlasIndex => 12;
		public override bool GetRolling(AttackAbility.State attackAbilityState) => true;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;

		public override Func<AttackAbility.State, GDTask> GetExtraEffects(AttackAbility.State attackAbilityState) =>
			async state =>
			{
				Hex hex = await AbilityCmd.SelectHex(state,
					hexes => hexes.AddRange(RangeHelper.GetHexesInRange(attackAbilityState.Target.Hex, 1).Where(hex => hex.IsEmpty())),
					hintText: $"Select a hex to create a {Icons.HintText(Icons.Damage)}2 trap");
				if(hex != null)
				{
					await AbilityCmd.CreateTrap(hex, "res://Content/OverlayTiles/Traps/BearTrap1H.tscn", 2);
				}
			};
	}

	public class PlusFour : ArtificerAMDCardModel
	{
		protected override int AtlasIndex => 15;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +4;
	}

	public class PlusOneRolling : ArtificerAMDCardModel
	{
		protected override int AtlasIndex => 16;
		public override bool GetRolling(AttackAbility.State attackAbilityState) => true;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +1;
	}
}
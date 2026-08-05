using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class ThornreaperAMDCards
{
	public class PlusZero : ThornreaperAMDCardModel
	{
		protected override int AtlasIndex => 0;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;
	}

	public class PlusZeroPlusOneIfLightStrongOrWaningRolling : ThornreaperAMDCardModel
	{
		public override string GetSimpleString(RichTextParameters richTextParameters) =>
			GetSimpleString(richTextParameters, +0,
				$"{Icons.InlineElement(Element.Light, richTextParameters)}:{Icons.Inline(Icons.GetAMDValue("+1"), richTextParameters)}");

		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +0,
				extraText:
				$"If {Icons.InlineElement(Element.Light, richTextParameters)} is strong or waning, {Icons.Inline(Icons.GetAMDValue("+1"), richTextParameters)} instead",
				rolling: true);

		protected override int AtlasIndex => 1;
		public override bool GetRolling(AttackAbility.State attackAbilityState) => true;
		public override int? GetValue(AttackAbility.State attackAbilityState) => LightStrongOrWaning ? +1 : +0;
	}

	public class PlusZeroLightRolling : ThornreaperAMDCardModel
	{
		protected override int AtlasIndex => 9;
		public override bool GetRolling(AttackAbility.State attackAbilityState) => true;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;
		public override List<CardElementInfusion> ElementInfusions => [CardElementInfusion.Infuse(Element.Light)];
	}

	public class PlusZeroEarthIfLightStrongOrWaningRolling : ThornreaperAMDCardModel
	{
		public override string GetSimpleString(RichTextParameters richTextParameters) =>
			GetSimpleString(richTextParameters, +0,
				$"{Icons.InlineElement(Element.Light, richTextParameters)}:{Icons.InlineElement(Element.Light, richTextParameters)}");

		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +0,
				extraText:
				$"If {Icons.InlineElement(Element.Light, richTextParameters)} is strong or waning, {Icons.InlineElement(Element.Light, richTextParameters)}",
				rolling: true);

		protected override int AtlasIndex => 11;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;
		public override List<CardElementInfusion> ElementInfusions => LightStrongOrWaning ? [CardElementInfusion.Infuse(Element.Light)] : [];
	}

	public class PlusZeroCreateHazardousTerrainWithinRangeOne : ThornreaperAMDCardModel
	{
		public override string GetSimpleString(RichTextParameters richTextParameters) =>
			GetSimpleString(richTextParameters, +0,
				$"{Icons.Inline("res://Game/Content/Classes/Thornreaper/toa-thorns.png", richTextParameters)}");

		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +0,
				extraText: $"Create one hazardous terrain tile in a featureless hex within {Icons.Inline(Icons.Range, richTextParameters)}1");

		protected override int AtlasIndex => 13;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;

		public override Func<AttackAbility.State, Figure, GDTask> GetExtraEffects() =>
			async (state, _) =>
			{
				Hex hex = await AbilityCmd.SelectHex(state,
					hexes => hexes.AddRange(RangeHelper.GetHexesInRange(state.Performer.Hex, 1).Where(hex => hex.IsFeatureless())),
					hintText: "Select a hex to create hazardous terrain");
				if(hex != null)
				{
					await AbilityCmd.CreateOverlayTile<ThornsThornreaper>(hex,
						SceneLoader.LoadPackedScene("res://Content/Classes/Thornreaper/ThornsThornreaper1H.tscn"));
				}
			};
	}

	public class PlusZeroOnNextAttackWhileOccupyingHazardousTerrainRetaliateThreeRolling : ThornreaperAMDCardModel
	{
		public override string GetSimpleString(RichTextParameters richTextParameters) =>
			GetSimpleString(richTextParameters, +0,
				$"{Icons.Inline(Icons.Retaliate, richTextParameters)}3{Icons.Inline(Icons.Rolling, richTextParameters)}");

		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +0,
				extraText:
				$"Place this card in your active area. On the next attack targeting you while you are occupying hazardous terrain, discard this card to gain {Icons.Inline(Icons.Retaliate, richTextParameters)}3");

		protected override int AtlasIndex => 14;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;

		//TODO: Implement Retaliate
	}

	public class PlusZeroOnNextAttackWhileOccupyingHazardousTerrainShieldThreeRolling : ThornreaperAMDCardModel
	{
		public override string GetSimpleString(RichTextParameters richTextParameters) =>
			GetSimpleString(richTextParameters, +0,
				$"{Icons.Inline(Icons.Shield, richTextParameters)}3{Icons.Inline(Icons.Rolling, richTextParameters)}");

		public override string ToString(RichTextParameters richTextParameters) =>
			GetBasicString(richTextParameters, +0,
				extraText:
				$"Place this card in your active area. On the next attack targeting you while you are occupying hazardous terrain, discard this card to gain {Icons.Inline(Icons.Shield, richTextParameters)}3");

		protected override int AtlasIndex => 16;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;

		//TODO: Implement Shield
	}
}
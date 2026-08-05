using System.Collections.Generic;
using Fractural.Tasks;

public class ThornreaperPerks
{
	public abstract class ThornreaperPerk : PerkModel
	{
	}

	public class ReplaceOneMinusTwoWithOnePlusZero : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusTwoAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZero>()
		];
	}

	public class ReplaceOneMinusOneWithOnePlusZeroPlusOneIfLightStrongOrWaningRolling : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToRemove { get; } =
		[
			ModelDB.AMDCard<MinusOneAMDCard>()
		];

		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroPlusOneIfLightStrongOrWaningRolling>()
		];
	}

	public class AddThreePlusZeroPlusOneIfLightStrongOrWaningRolling : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroPlusOneIfLightStrongOrWaningRolling>(),
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroPlusOneIfLightStrongOrWaningRolling>(),
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroPlusOneIfLightStrongOrWaningRolling>()
		];
	}

	public class AddTwoPlusZeroLightRolling : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroLightRolling>(),
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroLightRolling>()
		];
	}

	public class AddThreePlusZeroEarthIfLightStrongOrWaningRolling : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroEarthIfLightStrongOrWaningRolling>(),
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroEarthIfLightStrongOrWaningRolling>(),
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroEarthIfLightStrongOrWaningRolling>()
		];
	}

	public class AddOnePlusZeroCreateHazardousTerrainWithinRangeOne : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroCreateHazardousTerrainWithinRangeOne>()
		];
	}

	public class AddOnePlusZeroOnNextAttackWhileOccupyingHazardousTerrainRetaliateThreeRolling : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroOnNextAttackWhileOccupyingHazardousTerrainRetaliateThreeRolling>()
		];
	}

	public class AddOnePlusZeroOnNextAttackWhileOccupyingHazardousTerrainShieldThreeRolling : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroOnNextAttackWhileOccupyingHazardousTerrainShieldThreeRolling>()
		];
	}

	public class IgnoreItemMinusOneEffectsAddOnePlusZeroPlusOneIfLightStrongOrWaningRolling : ThornreaperPerk
	{
		public override List<AMDCardModel> CardsToAdd { get; } =
		[
			ModelDB.AMDCard<ThornreaperAMDCards.PlusZeroPlusOneIfLightStrongOrWaningRolling>()
		];

		public override bool IgnoreItemMinusOneEffects => true;
	}

	public class NaturesArmor : ThornreaperPerk
	{
		public override int PerkBoxCount => 2;
		protected override string Title => "Nature's Armor";

		public override string GetNonAMDDescription(RichTextParameters richTextParameters) =>
			$"Gain {Icons.Inline(Icons.Shield)}1 while you are occupying hazardous terrain.";

		public override async GDTask OnScenarioSetupPhaseCompleted(Character character)
		{
			await base.OnScenarioSetupPhaseCompleted(character);

			ScenarioEvents.SufferDamageEvent.Subscribe(this,
				parameters => parameters.Figure == character && parameters.FromAttack &&
				              parameters.Figure.Hex.HasHexObjectOfType<HazardousTerrain>(),
				async parameters =>
				{
					parameters.AdjustShield(1);
					await GDTask.CompletedTask;
				});

			ScenarioCheckEvents.ShieldCheckEvent.Subscribe(this,
				parameters => parameters.Figure == character &&
				              parameters.Figure.Hex.HasHexObjectOfType<HazardousTerrain>(),
				parameters =>
				{
					parameters.AdjustShield(1);
				});

			ScenarioEvents.FigureEnteredHexEvent.Subscribe(this,
				parameters => parameters.Figure == character,
				async _ =>
				{
					ScenarioCheckEvents.ShieldCheckEvent.FireChangedEvent();

					await GDTask.CompletedTask;
				}, EffectType.Visuals);

			ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Subscribe(this,
				parameters => parameters.Figure == character,
				parameters =>
				{
					parameters.Add(new InfoTextExtraEffect.Parameters(_ =>
						$"Gain {Icons.Inline(Icons.Shield)}1 while you are occupying hazardous terrain."));
				});
		}
	}
}
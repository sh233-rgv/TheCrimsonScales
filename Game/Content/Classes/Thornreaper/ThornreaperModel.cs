using System.Collections.Generic;
using Godot;

public class ThornreaperModel : ClassModel
{
	public override string Name => "Thornreaper";
	public override MaxHealthValues MaxHealthValues => MaxHealthValues.MediumHigh;
	public override int HandSize => 11;
	public override Ancestry Ancestry => Ancestry.Orchid;

	public override List<EventModel> UnlockEvents { get; } =
	[
	];

	public override List<EventModel> RetirementEvents { get; } =
	[
	];

	public override string AssetPath => "res://Content/Classes/Thornreaper";
	public override Color PrimaryColor => Color.FromHtml("dae182");
	public override Color SecondaryColor => Color.FromHtml("62622c");

	public override PackedScene Scene => SceneLoader.LoadPackedScene($"{AssetPath}/Thornreaper.tscn");

	public override List<AbilityCardModel> AbilityCards { get; } =
	[
		ModelDB.AbilityCard<CoverOfGreen>(),
		ModelDB.AbilityCard<DawnsGift>(),
		ModelDB.AbilityCard<EncasedInThorns>(),
		ModelDB.AbilityCard<ExtendedBranch>(),
		ModelDB.AbilityCard<JaggedClutch>(),
		ModelDB.AbilityCard<LashingThorns>(),
		ModelDB.AbilityCard<Midsummer>(),
		ModelDB.AbilityCard<Photosynthesis>(),
		ModelDB.AbilityCard<Overgrowth>(),
		ModelDB.AbilityCard<Superradiance>(),
		ModelDB.AbilityCard<ViolentSprout>(),

		ModelDB.AbilityCard<BlackRose>(),
		ModelDB.AbilityCard<SpikedEmbrace>(),
		ModelDB.AbilityCard<Thornstride>(),

		ModelDB.AbilityCard<OutwardSpurs>(),
		ModelDB.AbilityCard<FloralBurst>(),
		ModelDB.AbilityCard<BrightSkies>(),
		ModelDB.AbilityCard<WelcomeToTheJungle>(),
		ModelDB.AbilityCard<PricklySituation>(),
		ModelDB.AbilityCard<TwistedThistle>(),
		ModelDB.AbilityCard<BarbedOnslaught>(),
		ModelDB.AbilityCard<BranchedSlam>(),
		ModelDB.AbilityCard<DevouredByThorns>(),
		ModelDB.AbilityCard<SolarFlare>(),
		ModelDB.AbilityCard<NaturesFury>(),
		ModelDB.AbilityCard<RabidUndergrowth>(),
		ModelDB.AbilityCard<FissiveEruption>(),
		ModelDB.AbilityCard<ImpalingCommand>(),
		ModelDB.AbilityCard<RavagedEarth>(),
		ModelDB.AbilityCard<BedOfRoses>()
	];

	public override List<PerkModel> Perks { get; } =
	[
		ModelDB.Perk<ThornreaperPerks.ReplaceOneMinusTwoWithOnePlusZero>(),

		ModelDB.Perk<ThornreaperPerks.ReplaceOneMinusOneWithOnePlusZeroPlusOneIfLightStrongOrWaningRolling>(),
		ModelDB.Perk<ThornreaperPerks.ReplaceOneMinusOneWithOnePlusZeroPlusOneIfLightStrongOrWaningRolling>(),

		ModelDB.Perk<ThornreaperPerks.AddThreePlusZeroPlusOneIfLightStrongOrWaningRolling>(),
		ModelDB.Perk<ThornreaperPerks.AddThreePlusZeroPlusOneIfLightStrongOrWaningRolling>(),

		ModelDB.Perk<ThornreaperPerks.AddTwoPlusZeroLightRolling>(),

		ModelDB.Perk<ThornreaperPerks.AddThreePlusZeroEarthIfLightStrongOrWaningRolling>(),

		ModelDB.Perk<ThornreaperPerks.AddOnePlusZeroCreateHazardousTerrainWithinRangeOne>(),

		ModelDB.Perk<ThornreaperPerks.AddOnePlusZeroOnNextAttackWhileOccupyingHazardousTerrainRetaliateThreeRolling>(),
		ModelDB.Perk<ThornreaperPerks.AddOnePlusZeroOnNextAttackWhileOccupyingHazardousTerrainRetaliateThreeRolling>(),

		ModelDB.Perk<ThornreaperPerks.AddOnePlusZeroOnNextAttackWhileOccupyingHazardousTerrainShieldThreeRolling>(),
		ModelDB.Perk<ThornreaperPerks.AddOnePlusZeroOnNextAttackWhileOccupyingHazardousTerrainShieldThreeRolling>(),

		ModelDB.Perk<ThornreaperPerks.IgnoreItemMinusOneEffectsAddOnePlusZeroPlusOneIfLightStrongOrWaningRolling>(),

		ModelDB.Perk<ThornreaperPerks.NaturesArmor>(),
	];
}
using System.Collections.Generic;
using Godot;

public class LuminaryModel : ClassModel
{
	public override string Name => "Luminary";
	public override MaxHealthValues MaxHealthValues => MaxHealthValues.High;
	public override int HandSize => 15;
	public override string AssetPath => "res://Content/Classes/Luminary";
	public override Color PrimaryColor => Color.FromHtml("b289be");
	public override Color SecondaryColor => Color.FromHtml("383f74");

	public override PackedScene Scene => ResourceLoader.Load<PackedScene>($"{AssetPath}/Luminary.tscn");

	public override IList<AbilityCardModel> AbilityCards { get; } =
	[
		ModelDB.Card<BurningSparks>(),
		ModelDB.Card<ChillingWave>(),
		ModelDB.Card<FlickeringLights>(),
		ModelDB.Card<FrostyGlimmer>(),
		ModelDB.Card<HeatWaves>(),
		ModelDB.Card<Moonbeam>(),
		ModelDB.Card<RadiantGlare>(),
		ModelDB.Card<ShimmeringScuttle>(),
		ModelDB.Card<SoftGlow>(),
		ModelDB.Card<TorridRadiation>(),
		ModelDB.Card<ViolentFlash>(),
		ModelDB.Card<SolidLight>(),
		ModelDB.Card<SparklingGlow>(),
		ModelDB.Card<TricklingSting>(),

		ModelDB.Card<DarkenedOvercast>(),
		ModelDB.Card<Luminescence>(),
		ModelDB.Card<BlackenedRage>(),
		ModelDB.Card<ShiningDiversion>(),
		ModelDB.Card<EmpoweringRays>(),
		ModelDB.Card<Floodlight>(),
		ModelDB.Card<ColorfulWavelengths>(),
		ModelDB.Card<ShadowClaws>(),
		ModelDB.Card<EncompassingAura>(),
		ModelDB.Card<ImposingBrilliance>(),
		ModelDB.Card<GammaEnergy>(),
		ModelDB.Card<PhotonicDefense>(),
		ModelDB.Card<DominatingIllusion>(),
		ModelDB.Card<OpticalRefraction>(),
	];
}
using System.Collections.Generic;
using Godot;

public class AmberAegisModel : ClassModel
{
	public override string Name => "Amber Aegis";
	public override MaxHealthValues MaxHealthValues => MaxHealthValues.VeryHigh;
	public override int HandSize => 11;
	public override Ancestry Ancestry => Ancestry.Harrower;

	public override string AssetPath => "res://Content/Classes/AmberAegis";
	public override Color PrimaryColor => Color.FromHtml("ef6b26");
	public override Color SecondaryColor => Color.FromHtml("4b732e");

	public override PackedScene Scene => ResourceLoader.Load<PackedScene>($"{AssetPath}/AmberAegis.tscn");

	public override IList<AbilityCardModel> AbilityCards { get; } =
	[
		ModelDB.AbilityCard<BloodThinner>(),
		ModelDB.AbilityCard<Bogstep>(),
		ModelDB.AbilityCard<DeathSentence>(),
		ModelDB.AbilityCard<GroundSolvent>(),
		ModelDB.AbilityCard<LashingVines>(),
		ModelDB.AbilityCard<Mudslide>(),
		ModelDB.AbilityCard<Neurotoxin>(),
		ModelDB.AbilityCard<ParalyticAgent>(),
		ModelDB.AbilityCard<SerpentsKiss>(),
		ModelDB.AbilityCard<StillRiverAlgae>(),

		ModelDB.AbilityCard<Sinkhole>(),
		ModelDB.AbilityCard<VolatileTonic>(),
		ModelDB.AbilityCard<CopperneckBerries>(),

		ModelDB.AbilityCard<ThrowingDaggers>(),
		ModelDB.AbilityCard<AirborneSpores>(),
		ModelDB.AbilityCard<PotentMixture>(),
		ModelDB.AbilityCard<HideAndSeek>(),
		ModelDB.AbilityCard<RadiantForestFungi>(),
		ModelDB.AbilityCard<FirerootSap>(),
	];
}
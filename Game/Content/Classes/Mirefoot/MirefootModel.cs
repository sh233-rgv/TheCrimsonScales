using System.Collections.Generic;
using Godot;

public class MirefootModel : ClassModel
{
	public override string Name => "Mirefoot";
	public override MaxHealthValues MaxHealthValues => MaxHealthValues.Low;
	public override int HandSize => 10;
	public override Ancestry Ancestry => Ancestry.Quatryl;

	public override string AssetPath => "res://Content/Classes/Mirefoot";
	public override Color PrimaryColor => Color.FromHtml("ef6b26");
	public override Color SecondaryColor => Color.FromHtml("4b732e");

	public override PackedScene Scene => ResourceLoader.Load<PackedScene>($"{AssetPath}/Mirefoot.tscn");

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

		ModelDB.Card<ThrowingDaggers>(),
		ModelDB.Card<AirborneSpores>(),
		ModelDB.Card<PotentMixture>(),
		ModelDB.Card<HideAndSeek>(),
		ModelDB.Card<RadiantForestFungi>(),
		ModelDB.Card<FirerootSap>(),
		ModelDB.Card<PersonalPoison>(),
		ModelDB.Card<CompoundToxin>(),
		ModelDB.Card<TaintedWaters>(),
		ModelDB.Card<Anticoagulant>(),
		ModelDB.Card<WildStings>(),
		ModelDB.Card<SludgeBomb>(),
		ModelDB.Card<WhitefireBalm>(),
		ModelDB.Card<TwistTheBlade>(),
		ModelDB.Card<LingeringSwampMoss>(),
		ModelDB.Card<ComplexToxicology>(),
	];
}
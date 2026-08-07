using System.Collections.Generic;
using Godot;

public class IncarnateModel : ClassModel
{
	public override string Name => "Incarnate";
	public override MaxHealthValues MaxHealthValues => MaxHealthValues.High;
	public override int HandSize => 14;
	public override Ancestry Ancestry => Ancestry.Inox;

	public override List<EventModel> UnlockEvents { get; } =
	[
	];

	public override List<EventModel> RetirementEvents { get; } =
	[
	];

	public override string AssetPath => "res://Content/Classes/Incarnate";
	public override Color PrimaryColor => Color.FromHtml("c9c9c9");
	public override Color SecondaryColor => Color.FromHtml("1e625c");

	public override PackedScene Scene => SceneLoader.LoadPackedScene($"{AssetPath}/Incarnate.tscn");

	public override List<AbilityCardModel> AbilityCards { get; } =
	[
		ModelDB.AbilityCard<AncientBanner>(),
		ModelDB.AbilityCard<FarseersPilum>(),
		ModelDB.AbilityCard<VitalTether>(),
		ModelDB.AbilityCard<Firebrand>(),
		ModelDB.AbilityCard<TomegsShieldArm>(),
		ModelDB.AbilityCard<VaskasCunning>(),
		ModelDB.AbilityCard<MakusDeadlyAim>(),
		ModelDB.AbilityCard<PakhsLunge>(),
		ModelDB.AbilityCard<WarbornsShout>(),
		ModelDB.AbilityCard<GolsTonic>(),
		ModelDB.AbilityCard<HandsOfThreeTribes>(),

		ModelDB.AbilityCard<AncestralBlade>(),
		ModelDB.AbilityCard<WarDrums>(),
		ModelDB.AbilityCard<WieldedMemory>(),
	];

	public override List<PerkModel> Perks { get; } =
	[

	];
}
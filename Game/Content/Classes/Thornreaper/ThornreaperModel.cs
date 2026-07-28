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
	];

	public override List<PerkModel> Perks { get; } =
	[
	];
}
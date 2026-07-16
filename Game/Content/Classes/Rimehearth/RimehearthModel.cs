using System.Collections.Generic;
using Godot;

public class RimehearthModel : ClassModel
{
	public override string Name => "Rimehearth";
	public override MaxHealthValues MaxHealthValues => MaxHealthValues.Medium;
	public override int HandSize => 10;
	public override Ancestry Ancestry => Ancestry.Savvas;

	public override List<EventModel> UnlockEvents { get; } =
	[
	];

	public override List<EventModel> RetirementEvents { get; } =
	[
	];

	public override string AssetPath => "res://Content/Classes/Rimehearth";
	public override Color PrimaryColor => Color.FromHtml("f46e4d");
	public override Color SecondaryColor => Color.FromHtml("153752");

	public override PackedScene Scene => SceneLoader.LoadPackedScene($"{AssetPath}/Rimehearth.tscn");

	public override List<AbilityCardModel> AbilityCards { get; } =
	[
	];

	public override List<PerkModel> Perks { get; } =
	[
	];
}
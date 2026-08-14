using System.Collections.Generic;
using Godot;

public class ShardrenderModel : ClassModel
{
	public override string Name => "Shardrender";
	public override MaxHealthValues MaxHealthValues => MaxHealthValues.LowMedium;
	public override int HandSize => 11;
	public override Ancestry Ancestry => Ancestry.Orchid;

	public override List<EventModel> UnlockEvents { get; } =
	[
	];

	public override List<EventModel> RetirementEvents { get; } =
	[
	];

	public override string AssetPath => "res://Content/Classes/Shardrender";
	public override Color PrimaryColor => Color.FromHtml("f7ce65");
	public override Color SecondaryColor => Color.FromHtml("87702a");

	public override PackedScene Scene => SceneLoader.LoadPackedScene($"{AssetPath}/Shardrender.tscn");

	public override List<AbilityCardModel> AbilityCards { get; } =
	[
	];

	public override List<PerkModel> Perks { get; } =
	[
	];
}
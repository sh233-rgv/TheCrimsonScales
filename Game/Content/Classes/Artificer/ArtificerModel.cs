using System.Collections.Generic;
using Godot;

public class ArtificerModel : ClassModel
{
	public override string Name => "Artificer";
	public override MaxHealthValues MaxHealthValues => MaxHealthValues.Medium;
	public override int HandSize => 10;
	public override Ancestry Ancestry => Ancestry.Quatryl;
	public override string AssetPath => "res://Content/Classes/Artificer";
	public override Color PrimaryColor => Color.FromHtml("94dbe8");
	public override Color SecondaryColor => Color.FromHtml("286976");

	public override PackedScene Scene => ResourceLoader.Load<PackedScene>($"{AssetPath}/Artificer.tscn");

	public override IList<AbilityCardModel> AbilityCards { get; } =
	[
	];
}
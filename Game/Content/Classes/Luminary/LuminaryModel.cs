using System.Collections.Generic;
using Godot;

public class LuminaryModel : ClassModel
{
	public override string Name => "Luminary";
	public override MaxHealthValues MaxHealthValues => MaxHealthValues.High;
	public override int HandSize => 10;
	public override Ancestry Ancestry => Ancestry.Lurker;

	public override string AssetPath => "res://Content/Classes/Luminary";
	public override Color PrimaryColor => Color.FromHtml("b28abf");
	public override Color SecondaryColor => Color.FromHtml("7a3d99");

	public override PackedScene Scene => ResourceLoader.Load<PackedScene>($"{AssetPath}/Luminary.tscn");

	public override IList<AbilityCardModel> AbilityCards { get; } =
	[
	];
}
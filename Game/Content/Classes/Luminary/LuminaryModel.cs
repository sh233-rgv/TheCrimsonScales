using System.Collections.Generic;
using Godot;

public class LuminaryModel : ClassModel
{
	public override string Name => "Luminary";
	public override MaxHealthValues MaxHealthValues => MaxHealthValues.High;
	public override int HandSize => 2;
	public override string AssetPath => "res://Content/Classes/Luminary";
	public override Color PrimaryColor => Color.FromHtml("b289be");
	public override Color SecondaryColor => Color.FromHtml("383f74");

	public override PackedScene Scene => ResourceLoader.Load<PackedScene>($"{AssetPath}/Luminary.tscn");

	public override IList<AbilityCardModel> AbilityCards { get; } =
	[
		ModelDB.Card<BurningSparks>(),
		ModelDB.Card<ChillingWave>(),

	];
}
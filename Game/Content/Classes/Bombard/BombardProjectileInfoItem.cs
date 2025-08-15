using Godot;

public partial class BombardProjectileInfoItem : InfoItem<BombardProjectileInfoItem.Parameters>
{
	public class Parameters(BombardProjectileToken hexObject) : InfoItemParameters<BombardProjectileToken>(hexObject)
	{
		public override string ScenePath => "res://Content/Classes/Bombard/BombardProjectileInfoItem.tscn";
	}

	[Export]
	private CardSideView _cardSideView;

	public override void Init(Parameters parameters)
	{
		base.Init(parameters);

		_cardSideView.SetCard(parameters.HexObject.AbilityCardSide);
	}
}
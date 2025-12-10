using Godot;

public partial class TemporaryAMDCardsPopupCard : Control
{
	[Export]
	private AMDCardView _amdCardView;

	public void Init(AMDCardModel amdCardModel)
	{
		_amdCardView.SetCard(amdCardModel);
	}
}
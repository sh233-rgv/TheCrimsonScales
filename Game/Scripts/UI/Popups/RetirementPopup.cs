using Godot;

public partial class RetirementPopup : Popup<RetirementPopup.Request>
{
	public class Request : PopupRequest
	{
		public SavedCharacter Character { get; init; }
	}

	[Export]
	private TextureRect _portraitTexture;
	[Export]
	private PersonalQuestProgressView _personalQuestProgressView;

	protected override void OnOpen()
	{
		base.OnOpen();

		_portraitTexture.SetTexture(PopupRequest.Character.ClassModel.PortraitTexture);
		_personalQuestProgressView.Init(PopupRequest.Character);
	}
}
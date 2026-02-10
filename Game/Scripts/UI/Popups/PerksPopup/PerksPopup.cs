using System.Collections.Generic;
using Godot;

public partial class PerksPopup : Popup<PerksPopup.Request>
{
	public class Request : PopupRequest
	{
		public SavedCharacter SavedCharacter { get; init; }
	}

	[Export]
	private PackedScene _perkScene;

	[Export]
	private Control _perkContainer;

	private readonly List<PerksPopupPerk> _perks = new List<PerksPopupPerk>();

	protected override void OnOpen()
	{
		base.OnOpen();

		foreach(SavedPerk savedPerk in PopupRequest.SavedCharacter.SavedPerks)
		{
			PerksPopupPerk perk = _perkScene.Instantiate<PerksPopupPerk>();
			_perkContainer.AddChild(perk);
			perk.Init(savedPerk);
			_perks.Add(perk);
		}
	}

	protected override void OnClosed()
	{
		base.OnClosed();

		foreach(PerksPopupPerk perk in _perks)
		{
			perk.QueueFree();
		}

		_perks.Clear();
	}
}
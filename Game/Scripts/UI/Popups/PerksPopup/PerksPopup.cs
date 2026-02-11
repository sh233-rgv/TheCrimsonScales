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

		for(int i = 0; i < PopupRequest.SavedCharacter.ClassModel.Perks.Count; i++)
		{
			PerkModel perkModel = PopupRequest.SavedCharacter.ClassModel.Perks[i];
			PerksPopupPerk perk = _perkScene.Instantiate<PerksPopupPerk>();
			_perkContainer.AddChild(perk);
			perk.Init(perkModel, i, PopupRequest.SavedCharacter.AcquiredPerkIndices.Contains(i));
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
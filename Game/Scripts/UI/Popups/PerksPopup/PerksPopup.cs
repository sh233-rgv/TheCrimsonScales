using System.Collections.Generic;
using System.Linq;
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

		int perkIndex = 0;
		IEnumerable<IGrouping<PerkModel, PerkModel>> perkGroups = PopupRequest.SavedCharacter.ClassModel.Perks.GroupBy(perkModel => perkModel);
		foreach(IGrouping<PerkModel, PerkModel> perkGroup in perkGroups)
		{
			PerkModel perkModel = PopupRequest.SavedCharacter.ClassModel.Perks[perkIndex];
			PerksPopupPerk perk = _perkScene.Instantiate<PerksPopupPerk>();
			_perkContainer.AddChild(perk);
			perk.Init(perkModel, perkIndex, perkGroup.Count(), PopupRequest.SavedCharacter);
			_perks.Add(perk);

			perkIndex++;
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
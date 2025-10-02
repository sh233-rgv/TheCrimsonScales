using Godot;

public partial class PartyInfoCharacterItem : Control
{
	[Export]
	private ItemView _itemView;
	[Export]
	private TextureRect _itemTypeIcon;
	[Export]
	private Panel _border;

	public void Init(ItemType itemType, ItemModel itemModel)
	{
		_itemView.SetItem(itemModel);
		_itemView.SetVisible(itemModel != null);

		if(itemModel != null)
		{
			UIHelper.SetItemMaterial(_itemView.TextureRect, itemModel.ItemState);
		}

		_itemTypeIcon.SetTexture(ResourceLoader.Load<Texture2D>(Icons.GetItem(itemType)));
		_itemTypeIcon.SetVisible(itemModel == null);
	}
}
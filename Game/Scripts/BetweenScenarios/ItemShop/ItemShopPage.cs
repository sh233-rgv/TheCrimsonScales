using System.Collections.Generic;
using Godot;

public partial class ItemShopPage : Control
{
	[Export]
	private Control _itemParent;
	[Export]
	private PackedScene _itemScene;

	public List<ItemShopItem> Items { get; } = new List<ItemShopItem>();

	public void Init(List<ItemModel> itemModels)
	{
		foreach(ItemModel itemModel in itemModels)
		{
			ItemShopItem item = _itemScene.Instantiate<ItemShopItem>();
			_itemParent.AddChild(item);
			item.Init(itemModel);

			Items.Add(item);
		}
	}
}
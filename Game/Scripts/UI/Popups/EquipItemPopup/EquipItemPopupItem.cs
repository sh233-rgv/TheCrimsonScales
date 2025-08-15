using System;
using Godot;

public partial class EquipItemPopupItem : Control
{
	[Export]
	private ItemView _itemView;
	[Export]
	private BetterButton _betterButton;

	public ItemModel ItemModel { get; private set; }

	public event Action<EquipItemPopupItem> PressedEvent;

	public void Init(ItemModel itemModel)
	{
		ItemModel = itemModel;

		_itemView.SetItem(ItemModel);

		_betterButton.Pressed += OnPressed;
	}

	private void OnPressed()
	{
		PressedEvent?.Invoke(this);
	}
}
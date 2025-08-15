using System;
using Godot;

public partial class EquipmentSlot : Control
{
	[Export]
	private ItemView _itemView;
	[Export]
	private TextureRect _itemTypeIcon;
	[Export]
	private BetterButton _betterButton;

	public ItemType ItemType { get; private set; }
	public int SlotIndex { get; private set; }

	public event Action<EquipmentSlot> PressedEvent;

	public void Init(ItemType itemType, int slotIndex)
	{
		ItemType = itemType;
		SlotIndex = slotIndex;

		_itemTypeIcon.Texture = ResourceLoader.Load<Texture2D>(Icons.GetItem(itemType));

		_betterButton.Pressed += OnPressed;
	}

	public void SetItem(ItemModel itemModel)
	{
		_itemView.SetItem(itemModel);
	}

	private void OnPressed()
	{
		PressedEvent?.Invoke(this);
	}
}
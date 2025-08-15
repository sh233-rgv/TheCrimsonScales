using System;
using System.Collections.Generic;
using Godot;

public partial class EquipItemPopup : Popup<EquipItemPopup.Request>
{
	public class Request : PopupRequest
	{
		public SavedCharacter SavedCharacter { get; init; }
		public ItemType ItemType { get; init; }
		public int SlotIndex { get; init; }
		public Action<int, ItemModel> ItemSelectedEvent { get; init; }
	}

	[Export]
	private ScrollContainer _scrollContainer;
	[Export]
	private PackedScene _itemScene;
	[Export]
	private Control _itemParent;

	[Export]
	private Label _nothingAvailableLabel;

	[Export]
	private BetterButton _cancelButton;
	[Export]
	private BetterButton _clearButton;

	private readonly List<EquipItemPopupItem> _items = new List<EquipItemPopupItem>();

	public override void _Ready()
	{
		base._Ready();

		_cancelButton.Pressed += OnCancelPressed;
		_clearButton.Pressed += OnClearPressed;
	}

	protected override void OnOpen()
	{
		base.OnOpen();

		foreach(string itemId in PopupRequest.SavedCharacter.ItemIds)
		{
			ItemModel itemModel = ModelDB.GetById<ItemModel>(itemId);

			if(PopupRequest.ItemType == ItemType.TwoHands)
			{
				if(itemModel.ItemType != ItemType.TwoHands && itemModel.ItemType != ItemType.OneHand)
				{
					continue;
				}
			}
			else
			{
				if(itemModel.ItemType != PopupRequest.ItemType)
				{
					continue;
				}
			}

			if(PopupRequest.SavedCharacter.HasEquippedItem(itemId))
			{
				continue;
			}

			EquipItemPopupItem item = _itemScene.Instantiate<EquipItemPopupItem>();
			_itemParent.AddChild(item);
			item.Init(itemModel);
			item.PressedEvent += OnItemPressed;
			_items.Add(item);
		}

		_nothingAvailableLabel.SetVisible(_items.Count == 0);

		this.DelayedCall(() =>
		{
			float targetSize = Mathf.Clamp(_itemParent.Size.X, 270f, 1350f);
			bool shouldScroll = targetSize < _itemParent.Size.X;
			_scrollContainer.CustomMinimumSize = new Vector2(targetSize, shouldScroll ? 440f : 420f);
			_scrollContainer.HorizontalScrollMode = shouldScroll ? ScrollContainer.ScrollMode.Auto : ScrollContainer.ScrollMode.Disabled;

			this.DelayedCall(() =>
			{
				_panelContainer.PivotOffset = _panelContainer.Size * 0.5f;
			});
		});
	}

	protected override void OnClosed()
	{
		base.OnClosed();

		foreach(EquipItemPopupItem item in _items)
		{
			item.QueueFree();
		}

		_items.Clear();
	}

	private void OnItemPressed(EquipItemPopupItem item)
	{
		PopupRequest.ItemSelectedEvent?.Invoke(PopupRequest.SlotIndex, item.ItemModel);

		Close();
	}

	private void OnCancelPressed()
	{
		Close();
	}

	private void OnClearPressed()
	{
		PopupRequest.ItemSelectedEvent?.Invoke(PopupRequest.SlotIndex, null);

		Close();
	}
}
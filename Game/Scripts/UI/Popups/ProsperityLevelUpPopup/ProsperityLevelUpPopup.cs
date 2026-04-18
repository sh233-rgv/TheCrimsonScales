using System.Collections.Generic;
using Godot;

public partial class ProsperityLevelUpPopup : Popup<ProsperityLevelUpPopup.Request>
{
	public class Request : PopupRequest
	{
		public ItemModel[] ItemModels { get; init; }
	}

	[Export]
	private PackedScene _itemScene;
	[Export]
	private Control _itemParent;
	[Export]
	private ScrollContainer _scrollContainer;

	private readonly List<ProsperityLevelUpPopupItem> _items = new List<ProsperityLevelUpPopupItem>();

	protected override void OnOpen()
	{
		base.OnOpen();

		foreach(ItemModel itemModel in PopupRequest.ItemModels)
		{
			ProsperityLevelUpPopupItem item = _itemScene.Instantiate<ProsperityLevelUpPopupItem>();
			_itemParent.AddChild(item);
			item.Init(itemModel);
			_items.Add(item);
		}

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

		foreach(ProsperityLevelUpPopupItem item in _items)
		{
			item.QueueFree();
		}

		_items.Clear();
	}
}
using System.Collections.Generic;
using Godot;

public partial class ItemView : Control
{
	[Export]
	public TextureRect TextureRect;

	[Export]
	private PackedScene _characterTokenScene;
	[Export]
	private Control _characterTokenParent;
	[Export]
	private Control _maxUseCountTokenParent;

	private readonly List<ItemViewCharacterToken> _tokens = new List<ItemViewCharacterToken>();

	public ItemModel ItemModel { get; private set; }

	public void SetItem(ItemModel itemModel, bool showCharacterToken = false)
	{
		ItemModel = itemModel;

		if(ItemModel == null)
		{
			TextureRect.SetVisible(false);

			foreach(ItemViewCharacterToken token in _tokens)
			{
				token.QueueFree();
			}

			_tokens.Clear();

			return;
		}

		TextureRect.SetVisible(true);
		TextureRect.SetTexture(ItemModel.GetTexture());

		if(showCharacterToken)
		{
			// Add use slots
			if(ItemModel.Owner != null && ItemModel.HasUseSlots && ItemModel.UseSlotIndex < ItemModel.UseSlots.Count)
			{
				ItemUseSlot useSlot = ItemModel.UseSlots[ItemModel.UseSlotIndex];
				Texture2D tokenTexture = ItemModel.Owner.ClassModel.CharacterTokenTexture;

				ItemViewCharacterToken characterToken = _characterTokenScene.Instantiate<ItemViewCharacterToken>();
				_characterTokenParent.AddChild(characterToken);
				characterToken.Init(tokenTexture, useSlot.NormalizedPosition);
				_tokens.Add(characterToken);
			}

			// Add tokens for max use count items (like orbs)
			if(ItemModel.Owner != null && itemModel.HasMaxUseCount)
			{
				Texture2D tokenTexture = ItemModel.Owner.ClassModel.CharacterTokenTexture;

				for(int i = 0; i < itemModel.CurrentUseCountWithMaxUseCount; i++)
				{
					ItemViewCharacterToken characterToken = _characterTokenScene.Instantiate<ItemViewCharacterToken>();
					_maxUseCountTokenParent.AddChild(characterToken);
					characterToken.Init(tokenTexture, Vector2.Zero);
					_tokens.Add(characterToken);
				}
			}
		}
	}
}
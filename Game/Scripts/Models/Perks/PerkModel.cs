using System.Collections.Generic;
using Godot;

public abstract class PerkModel : AbstractModel
{
	protected abstract string TexturePath { get; }
	protected abstract int AtlasIndex { get; }
	protected abstract int ColumnCount { get; }
	protected abstract int RowCount { get; }

	public virtual List<AMDCardModel> CardsToRemove => [];
	public virtual List<AMDCardModel> CardsToAdd => [];

	public Texture2D GetTexture()
	{
		return AtlasTextureHelper.CreateAtlasTexture(AtlasIndex, ColumnCount, RowCount, ResourceLoader.Load<Texture2D>(TexturePath));
	}
}
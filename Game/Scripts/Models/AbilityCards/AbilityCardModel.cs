using Godot;

public abstract class AbilityCardModel<TTop, TBottom> : AbilityCardModel
	where TTop : AbilityCardSideModel
	where TBottom : AbilityCardSideModel
{
	public override AbilityCardSideModel Top => ModelDB.AbilityCardSide<TTop>();
	public override AbilityCardSideModel Bottom => ModelDB.AbilityCardSide<TBottom>();
}

public abstract class AbilityCardModel : AbstractModel
{
	public abstract string Name { get; }
	public abstract int Level { get; }
	public abstract int Initiative { get; }

	protected abstract string TexturePath { get; }
	protected abstract int ColumnCount { get; }
	protected abstract int RowCount { get; }
	protected abstract int AtlasIndex { get; }

	public abstract AbilityCardSideModel Top { get; }
	public abstract AbilityCardSideModel Bottom { get; }
	public virtual AbilityCardSideModel BasicTop => ModelDB.AbilityCardSide<BasicAbilityCardTop>();
	public virtual AbilityCardSideModel BasicBottom => ModelDB.AbilityCardSide<BasicAbilityCardBottom>();

	public Texture2D GetTexture()
	{
		return AtlasTextureHelper.CreateAtlasTexture(
			AtlasIndex, ColumnCount, RowCount,
			ResourceLoader.Load<Texture2D>(TexturePath));
	}
}
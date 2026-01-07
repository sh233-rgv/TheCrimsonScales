using Godot;

public abstract class EnhancementModel : AbstractModel
{
	protected abstract string TexturePath { get; }
	public abstract int BaseCost { get; }

	public Texture2D GetTexture()
	{
		return ResourceLoader.Load<Texture2D>(TexturePath);
	}
}
using Godot;

public interface IDeckCard
{
	public bool Reshuffles { get; }
	public bool RemoveAfterDraw { get; }

	public Texture2D GetTexture();
	public void Drawn();
}
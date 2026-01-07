using Godot;

public class EnhancementMark
{
	public EnhancementPipModel PipModel { get; private set; }
	public Vector2 NormalizedPosition { get; }

	public EnhancementMark(EnhancementPipModel pipModel, Vector2 normalizedPosition)
	{
		PipModel = pipModel;
		NormalizedPosition = normalizedPosition;
	}
}
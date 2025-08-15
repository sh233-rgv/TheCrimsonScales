using Godot;

public partial class FigureInfoIcon : Control
{
	[Export]
	private TextureRect _iconTexture;

	public void Init(ConditionModel conditionModel)
	{
		_iconTexture.SetTexture(ResourceLoader.Load<Texture2D>(Icons.GetCondition(conditionModel)));
	}
}
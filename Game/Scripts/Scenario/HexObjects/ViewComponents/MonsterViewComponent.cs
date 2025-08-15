using Godot;

public partial class MonsterViewComponent : HexObjectViewComponent
{
	[Export]
	public Sprite2D StandeeNumberCircle { get; private set; }

	[Export]
	public Label StandeeNumberLabel { get; private set; }
}
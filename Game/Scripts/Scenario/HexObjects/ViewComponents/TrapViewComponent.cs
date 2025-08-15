using Godot;

public partial class TrapViewComponent : HexObjectViewComponent
{
	[Export]
	public Node2D DamageContainer { get; private set; }

	[Export]
	public Label DamageLabel { get; private set; }

	[Export]
	public Node2D ConditionContainer { get; private set; }
}
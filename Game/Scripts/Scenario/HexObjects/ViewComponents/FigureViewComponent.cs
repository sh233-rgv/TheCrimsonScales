using Godot;

public partial class FigureViewComponent : HexObjectViewComponent
{
	[Export]
	public Node2D Health { get; private set; }

	[Export]
	public TextureProgressBar HealthProgressBar { get; private set; }

	[Export]
	public Label HealthLabel { get; private set; }

	[Export]
	public Node2D Shield { get; private set; }

	[Export]
	public Label ShieldLabel { get; private set; }

	[Export]
	public Node2D Retaliate { get; private set; }

	[Export]
	public Label RetaliateLabel { get; private set; }

	[Export]
	public Node2D Flying { get; private set; }

	[Export]
	public Node2D ConditionParent { get; private set; }

	[Export]
	public Sprite2D Outline { get; private set; }

	[Export]
	public GpuParticles2D TurnStartPS { get; private set; }
	
	[Export]
	public Node2D ActivePS { get; set; }
}
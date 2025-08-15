using Godot;

public partial class Scenario : Node
{
	[Export]
	public Map Map { get; private set; }

	public void Init()
	{
		Map.Init();
	}
}
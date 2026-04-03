using Godot;

public partial class BetweenScenariosPartyGoals : BetweenScenariosAction
{
	[Export]
	private PackedScene _partyGoalScene;

	protected override bool SelectCharacter => false;

	public override void _Ready()
	{
		base._Ready();
	}
}
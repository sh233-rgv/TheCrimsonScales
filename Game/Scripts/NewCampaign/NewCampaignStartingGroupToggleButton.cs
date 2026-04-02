using Godot;

public partial class NewCampaignStartingGroupToggleButton : ToggleButton<NewCampaignStartingGroupToggleButton>
{
	[Export]
	public StartingGroup StartingGroup { get; private set; }

	public new void Init()
	{
		base.Init();
	}
}
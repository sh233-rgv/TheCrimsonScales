using Godot;

public partial class MerchantsGuildHallReward : Control
{
	[Export]
	private RichTextLabel _description;

	public void Init(MerchantsGuildHallRewardModel model)
	{
		RichTextParameters parameters = _description.GetRichTextParameters();
		_description.SetText(model.GetDescription(parameters));
	}
}
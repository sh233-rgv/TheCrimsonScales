using Godot;

public partial class ItemEffectButton : EffectButton<ItemEffectButton.Parameters>
{
	public class Parameters : EffectButtonParameters
	{
		public override string ScenePath => "res://Scenes/Scenario/UI/EffectButtons/ItemEffectButton.tscn";

		public ItemModel ItemModel { get; }

		public Parameters(ItemModel itemModel)
		{
			ItemModel = itemModel;
		}
	}

	[Export]
	private ItemView _itemView;

	protected override void Init(Parameters parameters)
	{
		base.Init(parameters);

		_itemView.SetItem(parameters.ItemModel);
	}
}
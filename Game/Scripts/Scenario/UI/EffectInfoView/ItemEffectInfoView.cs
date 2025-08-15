using Godot;

public partial class ItemEffectInfoView : EffectInfoView<ItemEffectInfoView.Parameters>
{
	public class Parameters : EffectInfoViewParameters
	{
		public override string ScenePath => "res://Scenes/Scenario/UI/EffectInfoViews/ItemEffectInfoView.tscn";

		public ItemModel ItemModel { get; }

		public Parameters(ItemModel itemModel)
		{
			ItemModel = itemModel;
		}
	}

	[Export]
	private ItemView _itemView;

	public override void Init(Parameters parameters)
	{
		base.Init(parameters);

		_itemView.SetItem(parameters.ItemModel, true);
	}
}
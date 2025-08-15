using Godot;

public partial class ItemCurrentActionView : CurrentActionView<ItemCurrentActionView.Parameters>
{
	public class Parameters : CurrentActionViewParameters
	{
		public override string ScenePath => "res://Scenes/Scenario/UI/CurrentActionViews/ItemCurrentActionView.tscn";
		public override object Source => ItemModel;

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
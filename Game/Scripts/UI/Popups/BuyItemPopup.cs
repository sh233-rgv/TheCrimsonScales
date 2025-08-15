using Godot;

public partial class BuyItemPopup : Popup<BuyItemPopup.Request>
{
	public class Request : PopupRequest
	{
		public ItemModel ItemModel { get; init; }
		public SavedItem SavedItem { get; init; }
		public SavedCharacter Buyer { get; init; }
	}

	[Export]
	private ItemView _itemView;
	[Export]
	private RichTextLabel _itemAndBuyerLabel;
	[Export]
	private RichTextLabel _costLabel;

	[Export]
	private BetterButton _cancelButton;
	[Export]
	private BetterButton _confirmButton;

	public override void _Ready()
	{
		base._Ready();

		_cancelButton.Pressed += OnCancelPressed;
		_confirmButton.Pressed += OnConfirmPressed;
	}

	protected override void OnOpen()
	{
		base.OnOpen();

		_confirmButton.SetEnabled(true, false);

		_itemView.SetItem(PopupRequest.ItemModel);

		_itemAndBuyerLabel.Text = $"Buy {PopupRequest.ItemModel.Name} for {PopupRequest.Buyer.GetNameAndIcon()}?";
		_costLabel.Text = $"Cost: [img={{30}}]res://Art/Icons/Other/Coins.svg[/img]{PopupRequest.ItemModel.Cost}";
	}

	private void OnCancelPressed()
	{
		Close();
	}

	private void OnConfirmPressed()
	{
		_confirmButton.SetEnabled(false, false);

		PopupRequest.Buyer.RemoveGold(PopupRequest.ItemModel.Cost);
		PopupRequest.Buyer.AddItem(PopupRequest.ItemModel);
		PopupRequest.SavedItem.RemoveStock(1);

		AppController.Instance.AudioController.Play("res://Audio/SFX/ItemShop/COINS_Rattle_01_mono.wav", delay: 0.0f);

		AppController.Instance.SaveFile.Save();

		Close();
	}
}
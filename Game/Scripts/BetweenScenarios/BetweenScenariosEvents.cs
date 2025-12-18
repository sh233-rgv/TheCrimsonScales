public class BetweenScenariosEvents
{
	public class CalculateBuyPrice : BetweenScenariosEvent<CalculateBuyPrice.Parameters>
	{
		public class Parameters(SavedCharacter buyer, ItemModel itemModel, int initialPrice) : ParametersBase
		{
			public SavedCharacter Buyer { get; private set; } = buyer;
			public ItemModel ItemModel { get; private set; } = itemModel;
			public int Price { get; private set; } = initialPrice;

			public void AdjustPrice(int delta)
			{
				Price += delta;
			}
		}
	}

	private readonly CalculateBuyPrice _calculateBuyPrice = new CalculateBuyPrice();
	public static CalculateBuyPrice CalculateBuyPriceEvent => BetweenScenariosController.Instance.Events._calculateBuyPrice;

	public class ItemBought : BetweenScenariosEvent<ItemBought.Parameters>
	{
		public class Parameters(SavedCharacter buyer, ItemModel itemModel, int price) : ParametersBase
		{
			public SavedCharacter Buyer { get; private set; } = buyer;
			public ItemModel ItemModel { get; private set; } = itemModel;
			public int Price { get; private set; } = price;
		}
	}

	private readonly ItemBought _itemBought = new ItemBought();
	public static ItemBought ItemBoughtEvent => BetweenScenariosController.Instance.Events._itemBought;
}
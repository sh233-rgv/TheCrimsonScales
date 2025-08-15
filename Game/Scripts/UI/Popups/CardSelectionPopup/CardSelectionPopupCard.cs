using System;
using Godot;

[GlobalClass]
public partial class CardSelectionPopupCard : Control
{
	[Export]
	private Control _container;
	[Export]
	private TextureRect _textureRect;
	[Export]
	private Label _initiativeLabel;
	[Export]
	private BetterButton _cardButton;

	public AbilityCardModel AbilityCardModel { get; private set; }

	//public bool Selected { get; private set; }
	public bool InitiativeSelected { get; private set; }
	public bool Grayscale { get; private set; }

	public event Action<CardSelectionPopupCard> CardPressedEvent;
	public event Action<CardSelectionPopupCard> InitiativePressedEvent;
	public event Action<CardSelectionPopupCard> MouseEnteredEvent;
	public event Action<CardSelectionPopupCard> MouseExitedEvent;

	public void Init(AbilityCardModel cardModel, bool canSelect, bool grayscale)
	{
		AbilityCardModel = cardModel;
		Grayscale = grayscale;

		_textureRect.Texture = AbilityCardModel.GetTexture();
		_initiativeLabel.Text = AbilityCardModel.Initiative.ToString();

		_cardButton.SetEnabled(canSelect, false);
		UIHelper.SetCardMaterial(_textureRect, Grayscale ? CardState.Discarded : CardState.Hand);

		_cardButton.Pressed += OnCardPressed;
		_cardButton.MouseEntered += OnMouseEntered;
		_cardButton.MouseExited += OnMouseExited;
	}

	// public void SetSelected(bool selected, bool forceChange)
	// {
	// 	if(Selected == selected && !forceChange)
	// 	{
	// 		return;
	// 	}
	//
	// 	UIHelper.SetCardMaterial(_textureRect, CardState.Hand);
	//
	// 	Selected = selected;
	// }

	private void OnCardPressed()
	{
		CardPressedEvent?.Invoke(this);
	}

	private void OnInitiativePressed()
	{
		InitiativePressedEvent?.Invoke(this);
	}

	private void OnMouseEntered()
	{
		MouseEnteredEvent?.Invoke(this);
	}

	private void OnMouseExited()
	{
		MouseExitedEvent?.Invoke(this);
	}
}
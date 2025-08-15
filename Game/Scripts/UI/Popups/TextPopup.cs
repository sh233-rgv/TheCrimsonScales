using System.Collections.Generic;
using Godot;

public partial class TextPopup : Popup<TextPopup.Request>
{
	public class Request : PopupRequest
	{
		public string HeaderText { get; }
		public string BodyText { get; }

		public TextButton.Parameters[] ButtonParameters { get; }

		public Request(string headerText, string bodyText)
			: this(headerText, bodyText, new TextButton.Parameters("Confirm", null))
		{
		}

		public Request(string headerText, string bodyText, params TextButton.Parameters[] buttonParameters)
		{
			HeaderText = headerText;
			BodyText = bodyText;
			ButtonParameters = buttonParameters;
		}
	}

	[Export]
	private Label _headerLabel;
	[Export]
	private RichTextLabel _bodyLabel;

	[Export]
	private PackedScene _textButtonScene;
	[Export]
	private Control _textButtonParent;

	private readonly List<TextButton> _buttons = new List<TextButton>();

	protected override void OnOpen()
	{
		base.OnOpen();

		_headerLabel.SetText(PopupRequest.HeaderText);
		_bodyLabel.SetText(PopupRequest.BodyText);

		foreach(TextButton.Parameters buttonParameters in PopupRequest.ButtonParameters)
		{
			TextButton textButton = _textButtonScene.Instantiate<TextButton>();
			_textButtonParent.AddChild(textButton);
			textButton.Init(buttonParameters);
			textButton.PressedEvent += OnButtonPressed;
			_buttons.Add(textButton);
		}
	}

	protected override void OnClosed()
	{
		base.OnClosed();

		foreach(TextButton button in _buttons)
		{
			button.QueueFree();
		}

		_buttons.Clear();
	}

	private void OnButtonPressed()
	{
		Close();
	}
}
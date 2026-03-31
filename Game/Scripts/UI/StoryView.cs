using System;
using System.Collections.Generic;
using Godot;
using GTweensGodot.Extensions;

public partial class StoryView : Control
{
	[Export]
	private Label _title;
	[Export]
	private Label _subtitle;

	[Export]
	private RichTextLabel _text;

	[Export]
	private ChoiceButton _backButton;
	[Export]
	private ChoiceButton _continueButton;

	private readonly List<string> _texts = new List<string>();
	private int _textIndex;

	public bool Opened { get; private set; }

	public override void _Ready()
	{
		base._Ready();

		_backButton.BetterButton.Pressed += OnBackPressed;
		_continueButton.BetterButton.Pressed += OnContinuePressed;

		SetVisible(false);
	}

	public void Open(string title, string subtitle, string text)
	{
		_title.SetText(title);
		_subtitle.SetText(subtitle);

		_texts.Clear();
		//_text.SetText(text);

		string paragraphMarker = "\n";
		string[] paragraphs = text.Split([paragraphMarker], StringSplitOptions.RemoveEmptyEntries);
		_texts.AddRange(paragraphs);

		SetIndex(0);

		_backButton.SetActive(true);
		_continueButton.SetActive(true);
		SetVisible(true);

		Opened = true;
	}

	private void Close()
	{
		_backButton.SetActive(false);
		_continueButton.SetActive(false);

		this.TweenModulateAlpha(0f, 0.5f).OnComplete(Hide).Play();

		Opened = false;
	}

	private void SetIndex(int index)
	{
		_textIndex = index;

		_text.SetText(_texts[index]);

		UpdateButtons();
	}

	private void UpdateButtons()
	{
		_backButton.SetActive(_textIndex > 0);
		//_continueButton.SetActive(_textIndex < _texts.Count - 1);
	}

	private void OnBackPressed()
	{
		SetIndex(_textIndex - 1);
	}

	private void OnContinuePressed()
	{
		if(_textIndex >= _texts.Count - 1)
		{
			Close();

			return;
		}

		SetIndex(_textIndex + 1);
	}
}
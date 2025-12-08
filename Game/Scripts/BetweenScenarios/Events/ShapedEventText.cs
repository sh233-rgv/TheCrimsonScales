using System.Collections.Generic;
using System.Linq;
using Godot;

[Tool]
public partial class ShapedEventText : Control
{
	[Export]
	public string Text
	{
		get => _text;
		private set
		{
			_text = value;
			UpdateText();
		}
	}

	[Export]
	public Font Font
	{
		get => _font;
		private set
		{
			_font = value;
			UpdateText();
		}
	}

	[Export]
	public int FontSize
	{
		get => _fontSize;
		private set
		{
			_fontSize = value;
			UpdateText();
		}
	}

	[Export]
	public float[] Widths
	{
		get => _widths;
		private set
		{
			_widths = value;
			UpdateText();
		}
	}

	[Export]
	private RichTextLabel _richTextLabel;

	private string _text;
	private Font _font;
	private int _fontSize;
	private float _radius;
	private float[] _widths;

	private void UpdateText()
	{
		if(_richTextLabel == null || _widths == null || _font == null)
		{
			return;
		}

		_richTextLabel.SetText(WrapTextToWidths(_text, _widths, _font, _fontSize));
	}

	private static string WrapTextToWidths(string text, float[] widths, Font font, int fontSize)
	{
		List<string> words = text.Split(' ').ToList();
		int lineIndex = 0;

		string currentLine = "";
		string result = "";

		foreach(string word in words)
		{
			if(lineIndex >= widths.Length)
			{
				break;
			}

			string testLine = (currentLine + " " + word).Trim();
			float testWidth = font.GetStringSize(testLine, fontSize: fontSize).X;

			if(testWidth <= widths[lineIndex])
			{
				currentLine = testLine;
			}
			else
			{
				// Line finished, append it
				result += currentLine + "\n";
				lineIndex++;

				if(lineIndex >= widths.Length)
				{
					break;
				}

				currentLine = word; // Start new line with this word
			}
		}

		// Add final line
		if(lineIndex < widths.Length)
		{
			result += currentLine;
		}

		return result;
	}
}
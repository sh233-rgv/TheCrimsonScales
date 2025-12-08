using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Environment = System.Environment;

[Tool]
public partial class ShapedEventText : Control
{
	[Export]
	public Curve TextShapeCurve
	{
		get => _textShapeCurve;
		private set
		{
			_textShapeCurve = value;
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
	public float LineHeight
	{
		get => _lineHeight;
		private set
		{
			_lineHeight = value;
			UpdateText();
		}
	}

	[Export]
	private Control _richTextLabelParent;

	[Export]
	private RichTextLabel[] _richTextLabels;

	private Curve _textShapeCurve;
	private Font _font;
	private int _fontSize;
	private float _lineHeight;

	public override void _Ready()
	{
		base._Ready();

		this.DelayedCall(UpdateText);
	}

	private void UpdateText()
	{
		if(_textShapeCurve == null || _font == null || _richTextLabelParent == null)
		{
			return;
		}

		foreach(RichTextLabel richTextLabel in _richTextLabels)
		{
			richTextLabel.QueueFree();
		}

		List<RichTextLabel> labels = new List<RichTextLabel>();
		string testText =
			"""
			"Come one, come all, and welcome to the county fair!" a Quatryl with red-and-white facepaint and a clownish blue wig smiles as he waves you in through the entrance. You've decided to take the day off and visit the county fair, which you've enjoyed frequenting as a youth.

			"Step right up and try your luck!" an Inox strongman wielding a giant hammer beckons you forward. "Do you have what it takes to hit the bell?"

			On the other side, an Aesther throws a dart and pops a balloon. "Try your aim! Can you hit the balloon? Find out here!"
			""";
		var paragraphMarker = Environment.NewLine + Environment.NewLine;
		var paragraphs = testText.Split([paragraphMarker],
			StringSplitOptions.RemoveEmptyEntries);
		//List<string> paragraphs = testText.Split('\n').ToList();
		foreach(string paragraph in paragraphs)
		{
			List<string> words = paragraph.Split(' ').ToList();

			string currentLine = "";
			float lineWidth = 0f;

			foreach(string word in words)
			{
				int lineIndex = labels.Count;
				float yOffset = lineIndex * _lineHeight;
				float curveT = yOffset / _richTextLabelParent.Size.Y;
				lineWidth = _textShapeCurve.Sample(curveT) * _richTextLabelParent.Size.X;

				string testLine = (currentLine + " " + word).Trim();
				float testWidth = _font.GetStringSize(testLine, fontSize: _fontSize).X;

				if(testWidth <= lineWidth)
				{
					currentLine = testLine;
				}
				else
				{
					// Line finished, append it
					labels.Add(CreateLabel(currentLine, false, lineWidth, labels.Count));

					currentLine = word; // Start new line with this word
				}
			}

			// Add final line
			labels.Add(CreateLabel(currentLine, true, lineWidth, labels.Count));

			// Empty line at the end of a paragraph
			labels.Add(CreateLabel(string.Empty, false, lineWidth, labels.Count));
		}

		_richTextLabels = labels.ToArray();
	}

	private RichTextLabel CreateLabel(string line, bool finalLine, float lineWidth, int lineIndex)
	{
		RichTextLabel label = new RichTextLabel();
		_richTextLabelParent.AddChild(label);
		if(Engine.IsEditorHint())
		{
			label.SetOwner(GetTree().EditedSceneRoot);
		}
		else
		{
			label.SetOwner(GetTree().Root);
		}

		label.SetPosition(new Vector2((_richTextLabelParent.Size.X - lineWidth) / 2f, lineIndex * _lineHeight));
		label.SetSize(new Vector2(lineWidth, _lineHeight));
		label.SetHorizontalAlignment(finalLine ? HorizontalAlignment.Left : HorizontalAlignment.Fill);
		label.PushFont(_font, _fontSize);
		label.AppendText(line);
		label.SetFitContent(true);
		label.PopAll();
		return label;
		//labels.Add(label);
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
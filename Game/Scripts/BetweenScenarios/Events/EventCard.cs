using System.Threading;
using Fractural.Tasks;
using Godot;

public partial class EventCard : Control
{
	[Export]
	public Control FrontContainer { get; private set; }

	[Export]
	public ShapedEventText FrontEventText { get; private set; }

	[Export]
	public Control BackContainer { get; private set; }

	[Export]
	public ShapedEventText BackEventText { get; private set; }

	[Export]
	public Label NumberLabel;

	private bool _skipText;

	public void SetupFront(EventModel eventModel, bool showText, bool showFront = true)
	{
		if(showFront)
		{
			ShowFront();
		}

		FrontEventText.SetText(eventModel.Text, showText);
		NumberLabel.SetText(eventModel.Number.ToString());
	}

	public void SetupBack(SavedEventState savedEventState, bool showText = true, bool showBack = true)
	{
		if(showBack)
		{
			ShowBack();
		}

		BackEventText.SetText(savedEventState.Choice.GetStoryText(savedEventState), showText);
	}

	public void ShowFront()
	{
		FrontContainer.SetVisible(true);
		BackContainer.SetVisible(true);
		FrontContainer.SetModulate(Colors.White);
		BackContainer.SetModulate(Colors.Transparent);
	}

	public void ShowBack()
	{
		FrontContainer.SetVisible(true);
		BackContainer.SetVisible(true);
		FrontContainer.SetModulate(Colors.Transparent);
		BackContainer.SetModulate(Colors.White);
	}

	public async GDTask AnimateText(ShapedEventText shapedEventText, CancellationToken cancellationToken)
	{
		_skipText = false;

		const float charactersPerSecond = 50f;
		float charactersToDisplay = 0f;
		bool waitedFrame = false;

		foreach(RichTextLabel label in shapedEventText.RichTextLabels)
		{
			int labelLength = label.Text.Length;
			while(true)
			{
				if(_skipText)
				{
					charactersToDisplay += Mathf.Inf;
				}

				if(waitedFrame)
				{
					charactersToDisplay += charactersPerSecond * (float)GetProcessDeltaTime();
					waitedFrame = false;
				}

				label.SetVisibleCharacters(Mathf.Min(Mathf.FloorToInt(charactersToDisplay), labelLength));

				if(charactersToDisplay > labelLength)
				{
					charactersToDisplay -= labelLength;
					break;
				}

				await GDTask.Yield(cancellationToken);
				waitedFrame = true;
			}
		}
	}

	public void SkipText()
	{
		_skipText = true;
	}
}
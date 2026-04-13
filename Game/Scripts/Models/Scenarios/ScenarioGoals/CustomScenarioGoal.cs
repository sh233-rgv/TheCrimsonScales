using System;
using Fractural.Tasks;

public class CustomScenarioGoal : ScenarioGoal
{
	private readonly TextHelper.LabelTextDelegate _getLabelText;
	private readonly Func<CustomScenarioGoal, GDTask> _onStart;

	public CustomScenarioGoal(TextHelper.LabelTextDelegate getLabelText, Func<CustomScenarioGoal, GDTask> onStart = null,
		bool hasProgress = false, int? maxProgress = null,
		int order = 1)
		: base(order)
	{
		_getLabelText = getLabelText;
		_onStart = onStart;

		HasProgress = hasProgress;
		MaxProgress = maxProgress;
	}

	public override string GetLabelText(RichTextParameters textParameters) => _getLabelText(textParameters);

	public override async GDTask Start()
	{
		await base.Start();

		if(_onStart != null)
		{
			await _onStart.Invoke(this);
		}
	}

	public new async GDTask Complete()
	{
		await base.Complete();
	}

	public new async GDTask AdjustProgress(int progress)
	{
		await base.AdjustProgress(progress);
	}

	public new async GDTask SetProgress(int progress)
	{
		await base.SetProgress(progress);
	}

	public new async GDTask SetMaxProgress(int? maxProgress)
	{
		await base.SetMaxProgress(maxProgress);
	}
}
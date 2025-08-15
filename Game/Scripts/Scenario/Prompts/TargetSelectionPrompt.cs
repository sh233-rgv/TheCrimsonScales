using System;
using System.Collections.Generic;

public class TargetSelectionPrompt(Action<List<Figure>> getValidTargets, bool autoSelectIfOne, bool mandatory, EffectCollection effectCollection, Func<string> getHintText)
	: Prompt<TargetSelectionPrompt.Answer>(effectCollection, getHintText)
{
	public class Answer : PromptAnswer
	{
		public int FigureReferenceId { get; init; }
	}

	private readonly List<Figure> _validTargets = new List<Figure>();

	private Figure _selectedFigure;

	protected override bool CanConfirm => _selectedFigure != null;
	protected override bool CanSkip => _validTargets.Count == 0 || !mandatory;

	protected override void Enable()
	{
		base.Enable();

		_validTargets.Clear();
		getValidTargets(_validTargets);

		if(autoSelectIfOne && _validTargets.Count == 1)
		{
			_selectedFigure = _validTargets[0];

			if(mandatory)
			{
				Complete(true);
			}
		}
	}

	protected override void UpdateState()
	{
		base.UpdateState();

		GameController.Instance.HexIndicatorManager.StartSettingIndicators();

		foreach(Figure figure in _validTargets)
		{
			GameController.Instance.HexIndicatorManager.SetIndicator(figure.Hex, figure == _selectedFigure ? HexIndicatorType.Selected : HexIndicatorType.Normal, OnIndicatorPressed);
		}

		GameController.Instance.HexIndicatorManager.EndSettingIndicators();
	}

	protected override void Disable()
	{
		base.Disable();

		GameController.Instance.HexIndicatorManager.ClearIndicators();
	}

	protected override Answer CreateAnswer()
	{
		return new Answer()
		{
			FigureReferenceId = _selectedFigure.ReferenceId
		};
	}

	private void OnIndicatorPressed(HexIndicator hexIndicator)
	{
		if(_selectedFigure != null && hexIndicator.Hex == _selectedFigure.Hex)
		{
			_selectedFigure = null;
		}
		else
		{
			//TODO: Decide between overlapping figures
			_selectedFigure = hexIndicator.Hex.GetHexObjectOfType<Figure>();
		}

		FullUpdateState();
	}
}
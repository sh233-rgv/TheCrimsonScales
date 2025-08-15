using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft.Json;

public class HexSelectionPrompt(Action<List<Hex>> getValidHexes, bool autoSelectIfMaxCountIsValidCount, EffectCollection effectCollection, Func<string> getHintText, int minSelectionCount = 1, int maxSelectionCount = 1)
	: Prompt<HexSelectionPrompt.Answer>(effectCollection, getHintText)
{
	public class Answer : PromptAnswer
	{
		public List<Vector2I> CoordSets { get; init; }

		[JsonIgnore]
		public Vector2I Coords => CoordSets.Count == 0 ? Vector2I.Zero : CoordSets[0];
	}

	private readonly List<Hex> _validHexes = new List<Hex>();

	private readonly List<Hex> _selectedHexes = new List<Hex>();

	protected override bool CanConfirm => _selectedHexes.Count > 0 && (_selectedHexes.Count >= minSelectionCount || _selectedHexes.Count == _validHexes.Count) && _selectedHexes.Count <= maxSelectionCount;
	protected override bool CanSkip => minSelectionCount == 0 || _validHexes.Count == 0;

	protected override void Enable()
	{
		base.Enable();

		_validHexes.Clear();
		getValidHexes(_validHexes);

		_selectedHexes.Clear();

		if(autoSelectIfMaxCountIsValidCount && _validHexes.Count == maxSelectionCount)
		{
			_selectedHexes.AddRange(_validHexes);
		}

		if(_authority is not Character)
		{
			if(_validHexes.Count == 0)
			{
				Skip();
			}

			if(_validHexes.Count == 1)
			{
				_selectedHexes.Add(_validHexes[0]);
				Complete(true);
			}
		}
	}

	protected override void UpdateState()
	{
		base.UpdateState();

		GameController.Instance.HexIndicatorManager.StartSettingIndicators();

		foreach(Hex hex in _validHexes)
		{
			GameController.Instance.HexIndicatorManager.SetIndicator(hex, _selectedHexes.Contains(hex) ? HexIndicatorType.Selected : HexIndicatorType.Normal, OnIndicatorPressed);
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
			CoordSets = _selectedHexes.Select(hex => hex.Coords).ToList()
		};
	}

	private void OnIndicatorPressed(HexIndicator hexIndicator)
	{
		if(_selectedHexes.Contains(hexIndicator.Hex))
		{
			_selectedHexes.Remove(hexIndicator.Hex);
		}
		else
		{
			if(_selectedHexes.Count < maxSelectionCount)
			{
				_selectedHexes.Add(hexIndicator.Hex);
			}
		}

		FullUpdateState();
	}
}
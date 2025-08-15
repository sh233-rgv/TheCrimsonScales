using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class LootPrompt(Action<List<Hex>> getValidObjects, EffectCollection effectCollection, Func<string> getHintText)
	: Prompt<LootPrompt.Answer>(effectCollection, getHintText)
{
	public class Answer : PromptAnswer
	{
		public List<Vector2I> HexCoords { get; init; }
		//public List<int> ObjectsReferenceIds { get; init; }
	}

	private readonly List<Hex> _validHexes = new List<Hex>();

	protected override bool CanConfirm => _validHexes.Count > 0;

	protected override void Enable()
	{
		base.Enable();

		_validHexes.Clear();
		getValidObjects(_validHexes);

		if(_authority is not Character)
		{
			if(_validHexes.Count == 0)
			{
				Skip();
			}
			else
			{
				Complete(true);
			}
		}
	}

	protected override void UpdateState()
	{
		base.UpdateState();

		GameController.Instance.HexIndicatorManager.StartSettingIndicators();

		foreach(Hex validHex in _validHexes)
		{
			GameController.Instance.HexIndicatorManager.SetIndicator(validHex, HexIndicatorType.Selected, null);
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
			HexCoords = _validHexes.Select(hex => hex.Coords).ToList()
		};
	}
}
using System;
using System.Collections.Generic;
using Godot;

public class AOEPrompt(
	Figure performer, AOEPattern pattern, Hex forcedOriginHex, EffectCollection effectCollection, Func<string> getHintText, int range)
	: Prompt<AOEPrompt.Answer>(effectCollection, getHintText)
{
	public class Answer : PromptAnswer
	{
		public List<Vector2I> HexCoords { get; init; }
		public List<AOEHexType> HexTypes { get; init; }
		public List<string> HexCustomMarks { get; init; }
	}

	protected override void Enable()
	{
		base.Enable();

		GameController.Instance.AOEView.AOEChangedEvent += OnAOEChanged;
		GameController.Instance.AOEView.Open(pattern, forcedOriginHex, performer, range);
	}

	protected override void Disable()
	{
		base.Disable();

		GameController.Instance.AOEView.AOEChangedEvent -= OnAOEChanged;
		GameController.Instance.AOEView.Close();
	}

	protected override Answer CreateAnswer()
	{
		List<Vector2I> hexCoords = new List<Vector2I>();
		List<AOEHexType> hexTypes = new List<AOEHexType>();
		List<string> hexCustomMarks = new List<string>();

		foreach(AOEHexView hexView in GameController.Instance.AOEView.Hexes)
		{
			hexCoords.Add(hexView.GlobalCoords);
			hexTypes.Add(hexView.Type);
			hexCustomMarks.Add(hexView.CustomMark);
		}

		return new Answer()
		{
			HexCoords = hexCoords,
			HexTypes = hexTypes,
			HexCustomMarks = hexCustomMarks
		};
	}

	private void OnAOEChanged()
	{
		FullUpdateState();
	}
}
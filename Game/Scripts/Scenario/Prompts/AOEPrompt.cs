using System;
using System.Collections.Generic;
using Godot;

public class AOEPrompt(TargetedAbilityState targetedAbilityState, AOEPattern pattern, Hex forcedOriginHex, EffectCollection effectCollection, Func<string> getHintText)
	: Prompt<AOEPrompt.Answer>(effectCollection, getHintText)
{
	public class Answer : PromptAnswer
	{
		public List<Vector2I> HexCoords { get; init; }
		public List<AOEHexType> HexTypes { get; init; }
	}

	protected override void Enable()
	{
		base.Enable();

		GameController.Instance.AOEView.AOEChangedEvent += OnAOEChanged;
		GameController.Instance.AOEView.Open(pattern, forcedOriginHex, targetedAbilityState.Performer, targetedAbilityState.AbilityRange);
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

		foreach(AOEHexView hexView in GameController.Instance.AOEView.Hexes)
		{
			hexCoords.Add(hexView.GlobalCoords);
			hexTypes.Add(hexView.Type);
		}

		return new Answer()
		{
			HexCoords = hexCoords,
			HexTypes = hexTypes
		};
	}

	private void OnAOEChanged()
	{
		FullUpdateState();
	}
}
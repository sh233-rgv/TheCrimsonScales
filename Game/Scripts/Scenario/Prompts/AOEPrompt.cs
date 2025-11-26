using System;
using System.Collections.Generic;
using Godot;

public class AOEPrompt(AbilityState abilityState, AOEPattern pattern, Hex forcedOriginHex, EffectCollection effectCollection, Func<string> getHintText, int range = 1)
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
		if (abilityState is TargetedAbilityState targetedAbilityState)
        {
            range = targetedAbilityState.AbilityRange;
        }
		GameController.Instance.AOEView.Open(pattern, forcedOriginHex, abilityState.Performer, range);
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
using System;
using System.Collections.Generic;
using Godot;

public class MonsterAOEPrompt(
	AbilityState abilityState, AOEPattern pattern, int range, RangeType rangeType, Figure focus,
	EffectCollection effectCollection, Func<string> getHintText)
	: Prompt<MonsterAOEPrompt.Answer>(effectCollection, getHintText)
{
	public class Answer : PromptAnswer
	{
		public List<Vector2I> HexCoords { get; init; }
		public List<AOEHexType> HexTypes { get; init; }
	}

	private readonly List<AIAttackNode> _bestAIAttackNodes = new List<AIAttackNode>();

	private AIAttackNode _selectedNode;

	protected override bool CanSkip => false;

	protected override void Enable()
	{
		base.Enable();

		if(focus == null)
		{
			Skip();
			return;
		}

		bool hasGrayHex = false;

		foreach(AOEHex pivotAOEHex in pattern.Hexes)
		{
			if(pivotAOEHex.Type == AOEHexType.Gray)
			{
				hasGrayHex = true;
			}
		}

		Map map = GameController.Instance.Map;

		_bestAIAttackNodes.Clear();

		List<Hex> rangeCache = new List<Hex>();
		RangeHelper.FindHexesInRange(abilityState.Performer.Hex, range, false, rangeCache);

		void CompareAttackNode(AIAttackNode newAIAttackNode)
		{
			if(newAIAttackNode.AttackCount == 0)
			{
				return;
			}

			if(_bestAIAttackNodes.Count == 0)
			{
				_bestAIAttackNodes.Add(newAIAttackNode);
			}
			else
			{
				AIAttackNode previousBestAttackNode = _bestAIAttackNodes[0];
				CompareResult compareResult = newAIAttackNode.CompareTo(previousBestAttackNode);
				switch(compareResult)
				{
					case CompareResult.Better:
						_bestAIAttackNodes.Clear();
						_bestAIAttackNodes.Add(newAIAttackNode);
						break;
					case CompareResult.Equal:
						_bestAIAttackNodes.Add(newAIAttackNode);
						break;
					case CompareResult.Worse:
						break;
				}
			}
		}

		//TODO: This can be optimized quite a bit probably

		foreach(Hex hexInRange in rangeCache)
		{
			if(hasGrayHex && hexInRange != abilityState.Performer.Hex)
			{
				continue;
			}

			for(int i = 0; i < 6; i++)
			{
				foreach(AOEHex pivotAOEHex in pattern.Hexes)
				{
					if(hasGrayHex && pivotAOEHex.Type != AOEHexType.Gray)
					{
						continue;
					}

					Figure attackableFocus = null;
					int attackableFigureCount = 0;
					int disadvantageCount = 0;

					Vector2I pivotOffset = -pivotAOEHex.LocalCoords;
					foreach(AOEHex aoeHex in pattern.Hexes)
					{
						if(aoeHex.Type != AOEHexType.Red)
						{
							continue;
						}

						Vector2I globalCoords = hexInRange.Coords + Map.RotateCoordsClockwise(pivotOffset + aoeHex.LocalCoords, i);
						Hex potentialTargetHex = map.GetHex(globalCoords);

						if(potentialTargetHex == null || !GameController.Instance.Map.HasLineOfSight(abilityState.Performer.Hex, potentialTargetHex))
						{
							continue;
						}

						foreach(Figure potentialTarget in potentialTargetHex.GetHexObjectsOfType<Figure>())
						{
							if(!abilityState.Authority.EnemiesWith(potentialTarget))
							{
								continue;
							}

							if(potentialTarget == focus)
							{
								attackableFocus = potentialTarget;
							}

							attackableFigureCount++;

							ScenarioCheckEvents.DisadvantageCheck.Parameters disadvantageCheck = ScenarioCheckEvents.DisadvantageCheckEvent.Fire(
								new ScenarioCheckEvents.DisadvantageCheck.Parameters(potentialTarget, abilityState.Performer, abilityState.Performer.Hex,
									rangeType == RangeType.Range && RangeHelper.Distance(abilityState.Performer.Hex, potentialTargetHex) == 1));

							if(disadvantageCheck.HasDisadvantage)
							{
								disadvantageCount++;
							}
						}
					}

					// We are ignoring focusParameters.Targets here because it's an AOE. If we have a weird AOE ability like Boldening Blow, that would not work properly.
					int finalTargetCount = attackableFigureCount;
					//int finalTargetCount = Mathf.Min(attackableFigureCount, focusParameters.Targets);
					AIAttackNode newAIAttackNode = new AIAttackNode(hexInRange, pivotOffset, i, attackableFocus, finalTargetCount, disadvantageCount);

					CompareAttackNode(newAIAttackNode);
				}
			}
		}

		if(_bestAIAttackNodes.Count == 0)
		{
			// No attacks can be made
			Skip();
			return;
		}

		//TODO: Currently, the player is not allowed to choose the AOE pattern to perform
		_selectedNode = _bestAIAttackNodes[0];
		Complete(true);

		// GameController.Instance.AOEView.AOEChangedEvent += OnAOEChanged;
		// GameController.Instance.AOEView.Open(pattern, forcedOriginHex, abilityState.Performer, range);
	}

	protected override void Disable()
	{
		base.Disable();

		// GameController.Instance.AOEView.AOEChangedEvent -= OnAOEChanged;
		// GameController.Instance.AOEView.Close();
	}

	protected override Answer CreateAnswer()
	{
		List<Vector2I> hexCoords = new List<Vector2I>();
		List<AOEHexType> hexTypes = new List<AOEHexType>();

		foreach(AOEHex aoeHex in pattern.Hexes)
		{
			Vector2I globalCoords = _selectedNode.HexInRange.Coords + Map.RotateCoordsClockwise(_selectedNode.PivotOffset + aoeHex.LocalCoords, _selectedNode.RotationIndex);
			Hex potentialTargetHex = GameController.Instance.Map.GetHex(globalCoords);

			if(potentialTargetHex == null)
			{
				continue;
			}

			hexCoords.Add(potentialTargetHex.Coords);
			hexTypes.Add(aoeHex.Type);
		}

		return new Answer()
		{
			HexCoords = hexCoords,
			HexTypes = hexTypes
		};
	}

	private class AIAttackNode
	{
		public Hex HexInRange { get; }
		public Vector2I PivotOffset { get; }
		public int RotationIndex { get; }
		public Figure AttackableFocus { get; }
		public int AttackCount { get; }
		public int DisadvantageCount { get; }

		public AIAttackNode(Hex hexInRange, Vector2I pivotOffset, int rotationIndex, Figure attackableFocus, int attackCount, int disadvantageCount)
		{
			HexInRange = hexInRange;
			PivotOffset = pivotOffset;
			RotationIndex = rotationIndex;
			AttackableFocus = attackableFocus;
			AttackCount = attackCount;
			DisadvantageCount = disadvantageCount;
		}

		public CompareResult CompareTo(AIAttackNode other)
		{
			if(AttackableFocus != null && other.AttackableFocus == null)
			{
				return CompareResult.Better;
			}

			if(other.AttackableFocus != null && AttackableFocus == null)
			{
				return CompareResult.Worse;
			}

			if(AttackCount > other.AttackCount)
			{
				return CompareResult.Better;
			}

			if(other.AttackCount > AttackCount)
			{
				return CompareResult.Worse;
			}

			if(DisadvantageCount > other.DisadvantageCount)
			{
				return CompareResult.Worse;
			}

			if(other.DisadvantageCount > DisadvantageCount)
			{
				return CompareResult.Better;
			}

			return CompareResult.Equal;
		}
	}
}
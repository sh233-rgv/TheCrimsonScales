using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class PushPullPrompt(
	AbilityState abilityState, Hex origin, Figure target, int distance, bool push,
	EffectCollection effectCollection, Func<string> getHintText)
	: Prompt<PushPullPrompt.Answer>(effectCollection, getHintText)
{
	public class Answer : PromptAnswer
	{
		public List<Vector2I> Path { get; init; }
	}

	private readonly Dictionary<Hex, PushPullNode> _closedList = new Dictionary<Hex, PushPullNode>();
	private PushPullNode _currentNode;
	private readonly List<PushPullNode> _waypoints = new List<PushPullNode>();
	private readonly List<Hex> _path = new List<Hex>();

	private readonly List<PushPullNode> _nodes = new List<PushPullNode>();

	protected override bool CanConfirm => PathExists && MoveHelper.CanStopAt(target, _currentNode.Hex);
	protected override bool CanSkip => true;

	private bool PathExists => _currentNode != null && _currentNode.Parents.Count > 0;

	protected override void Enable()
	{
		base.Enable();

		_currentNode = new PushPullNode(target.Hex, 0, distance);
		_waypoints.Clear();
		_waypoints.Add(_currentNode);

		// Find all hexes we can push/pull into to
		MoveHelper.FindReachablePushPullHexes(abilityState, _currentNode, target, origin, push, _closedList);
		_closedList.Add(_currentNode.Hex, _currentNode);

		_nodes.Clear();

		foreach((Hex hex, PushPullNode node) in _closedList)
		{
			if(!MoveHelper.CanStopAt(target, hex))
			{
				continue;
			}

			_nodes.Add(node);

			// if(_nodes.Count == 0)
			// {
			// 	_nodes.Add(node);
			// }
			// else
			// {
			// 	PushPullNode previousBestNode = _nodes[0];
			// 	if(node.MoveSpent > previousBestNode.MoveSpent)
			// 	{
			// 		_nodes.Clear();
			// 		_nodes.Add(node);
			// 	}
			//
			// 	if(node.MoveSpent == previousBestNode.MoveSpent)
			// 	{
			// 		_nodes.Add(node);
			// 	}
			// }
		}

		// if(_bestNodes.Count == 0 || _bestNodes[0].Hex == _currentNode.Hex)
		// {
		// 	// Cannot push/pull further
		// 	Skip();
		// }
	}

	protected override void UpdateState()
	{
		base.UpdateState();

		MoveHelper.FindReachablePushPullHexes(abilityState, _currentNode, target, origin, push, _closedList);

		GameController.Instance.HexIndicatorManager.StartSettingIndicators();

		HashSet<Hex> reachableHexes = new HashSet<Hex>();

		foreach(PushPullNode bestMoveNode in _nodes)
		{
			HandleNode(bestMoveNode);
		}

		bool HandleNode(PushPullNode moveNode)
		{
			bool cameAcrossWaypoint = _waypoints.LastOrDefault()?.Hex == moveNode.Hex;
			foreach(PushPullNode moveNodeParent in moveNode.Parents)
			{
				if(HandleNode(moveNodeParent))
				{
					cameAcrossWaypoint = true;
				}
			}

			if(cameAcrossWaypoint)
			{
				if(moveNode.Hex == _waypoints.LastOrDefault()?.Hex)
				{
					//waypointHexes.AddIfNew(moveNode.Hex);
				}
				else
				{
					reachableHexes.Add(moveNode.Hex);
				}
			}

			return cameAcrossWaypoint;
		}

		foreach(Hex hex in reachableHexes)
		{
			GameController.Instance.HexIndicatorManager.SetIndicator(hex, HexIndicatorType.Normal, OnIndicatorPressed);
		}

		if(_currentNode != null && _currentNode.Parents.Count > 0)
		{
			GameController.Instance.HexIndicatorManager.SetIndicator(_currentNode.Hex, HexIndicatorType.Selected, OnIndicatorPressed);
		}

		GameController.Instance.HexIndicatorManager.EndSettingIndicators();

		// Create path out of the waypoints
		_path.Clear();
		PushPullNode pathNode = _currentNode;
		while(pathNode != null)
		{
			_path.Add(pathNode.Hex);
			pathNode = pathNode.Parents.FirstOrDefault();
		}

		_path.Reverse();

		List<Hex> waypointHexes = _waypoints.Select(waypoint => waypoint.Hex).ToList();
		GameController.Instance.MovePath.Open(_path, waypointHexes);
	}

	protected override void Disable()
	{
		base.Disable();

		GameController.Instance.HexIndicatorManager.ClearIndicators();
		GameController.Instance.MovePath.Close();
	}

	private void OnIndicatorPressed(HexIndicator hexIndicator)
	{
		if(_currentNode != null && _currentNode.Parents.Count > 0 && _currentNode.Hex == hexIndicator.Hex)
		{
			_waypoints.RemoveAt(_waypoints.Count - 1);
			_currentNode = _waypoints[^1];

			FullUpdateState();
			return;
		}

		if(_closedList.TryGetValue(hexIndicator.Hex, out PushPullNode node))
		{
			_currentNode = node;
			_waypoints.Add(_currentNode);

			FullUpdateState();
		}
	}

	protected override Answer CreateAnswer()
	{
		return new Answer()
		{
			Path = _path.TakeLast(_path.Count - 1).Select(hex => hex.Coords).ToList()
		};
	}
}
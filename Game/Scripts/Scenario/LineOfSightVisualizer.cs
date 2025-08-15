using System.Diagnostics;
using Godot;

public partial class LineOfSightVisualizer : Node2D
{
	[Export]
	private Line2D _line;

	private Hex _from;
	private Vector2 _fromPosition;

	public override void _Ready()
	{
		base._Ready();

		_line.Hide();
		SetProcess(false);
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if(@event is InputEventKey inputEventKey)
		{
			if(inputEventKey.Keycode == Key.L)
			{
				if(inputEventKey.Pressed && !inputEventKey.Echo)
				{
					Vector2 mousePosition = GetGlobalMousePosition();
					_from = GameController.Instance.Map.GetHex(Map.GlobalPositionToCoords(mousePosition));
					_fromPosition = Map.CoordsToGlobalPosition(Map.GlobalPositionToCoords(mousePosition));

					_line.Show();
					SetProcess(true);
				}
				else if(inputEventKey.IsReleased())
				{
					_line.Hide();
					SetProcess(false);
				}
			}
			// else if(inputEventKey.Keycode == Key.T && inputEventKey.Pressed)
			// {
			// 	Stopwatch stopwatch = new Stopwatch();
			// 	stopwatch.Start();
			// 	foreach((Vector2I coords, Hex hex) in ScenarioController.Instance.Map.Hexes)
			// 	{
			// 		if(!hex.Revealed)
			// 		{
			// 			continue;
			// 		}
			//
			// 		foreach((Vector2I otherCoords, Hex otherHex) in ScenarioController.Instance.Map.Hexes)
			// 		{
			// 			if(!otherHex.Revealed)
			// 			{
			// 				continue;
			// 			}
			//
			// 			ScenarioController.Instance.Map.HasLineOfSight(hex, otherHex);
			// 		}
			// 	}
			//
			// 	stopwatch.Stop();
			// 	GD.Print(stopwatch.ElapsedMilliseconds);
			// }
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		Vector2 mousePosition = GetGlobalMousePosition();
		Hex hex = GameController.Instance.Map.GetHex(Map.GlobalPositionToCoords(mousePosition));

		if(hex == null || _from == null)
		{
			_line.SetPointPosition(0, _fromPosition);
			_line.SetPointPosition(1, Map.CoordsToGlobalPosition(Map.GlobalPositionToCoords(mousePosition)));
			_line.SelfModulate = Colors.Red;
			return;
		}

		if(GameController.Instance.Map.CheckLineOfSight(_from, hex, out Vector2 pointA, out Vector2 pointB))
		{
			_line.SetPointPosition(0, pointA);
			_line.SetPointPosition(1, pointB);
			_line.SelfModulate = Colors.White;
		}
		else
		{
			_line.SetPointPosition(0, _from.GlobalPosition);
			_line.SetPointPosition(1, hex.GlobalPosition);
			_line.SelfModulate = Colors.Red;
		}
	}
}
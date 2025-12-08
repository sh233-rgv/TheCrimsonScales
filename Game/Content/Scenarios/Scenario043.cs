using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using GTweens.Easings;
using GTweensGodot.Extensions;

public class Scenario043 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario043.tscn";
	public override int ScenarioNumber => 43;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<EventScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() => new KillAlLEnemiesScenarioGoals();

	private Door _door1;
	private Door _door2;
	private Door _door3;
	private Door _door4;
	private PressurePlate _pressurePlateA;
	private PressurePlate _pressurePlateB;
	private List<PressurePlate> _pressurePlatesC;
	private List<PressurePlate> _pressurePlatesD;


	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		GameController.Instance.Map.Treasures[0].SetItemLoot(ModelDB.Item<AshsteelGauntlets>());

		Marker marker1 = GameController.Instance.Map.GetMarker(Marker.Type._1);
		_door1 = marker1.GetHexObject<Door>();

		Marker marker2 = GameController.Instance.Map.GetMarker(Marker.Type._2);
		_door2 = marker2.GetHexObject<Door>();

		Marker marker3 = GameController.Instance.Map.GetMarker(Marker.Type._3);
		_door3 = marker3.GetHexObject<Door>();

		Marker marker4 = GameController.Instance.Map.GetMarker(Marker.Type._4);
		_door4 = marker4.GetHexObject<Door>();
		
		_pressurePlateA = GameController.Instance.Map.GetMarker(Marker.Type.a).GetHexObject<PressurePlate>();
		_pressurePlateB = GameController.Instance.Map.GetMarker(Marker.Type.b).GetHexObject<PressurePlate>();
		_pressurePlatesC = GameController.Instance.Map.GetMarkers(Marker.Type.c).Select(marker => marker.GetHexObject<PressurePlate>()).ToList();
		_pressurePlatesD = GameController.Instance.Map.GetMarkers(Marker.Type.d).Select(marker => marker.GetHexObject<PressurePlate>()).ToList();

		ScenarioEvents.DuringMovementEvent.Subscribe(this,
			canApplyParameters => canApplyParameters.Performer is Character && canApplyParameters.AbilityState.MoveValue > 0 &&
				RangeHelper.GetHexesInRange(canApplyParameters.Performer.Hex, 1).Any(hex =>
				{
					Obstacle obstacle = hex.GetHexObjectOfType<Obstacle>();
					return obstacle != null && obstacle.Name.ToString().Contains("Boulder1H");
				}),
			async applyParameters =>
			{
				applyParameters.AbilityState.AdjustMoveValue(-1);

				Obstacle obstacle = (await AbilityCmd.SelectHex(applyParameters.Performer, list =>
					{
						list.AddRange(RangeHelper.GetHexesInRange(applyParameters.Performer.Hex, 1).Where(hex =>
						{
							Obstacle obstacle = hex.GetHexObjectOfType<Obstacle>();
							return obstacle != null && obstacle.Name.ToString().Contains("Boulder1H");
						}));
					}, mandatory: true, hintText: "Select an obstacle to move")).GetHexObjectOfType<Obstacle>();

				Hex movedToHex = await AbilityCmd.SelectHex(applyParameters.Performer, list =>
				{
					list.AddRange(RangeHelper.GetHexesInRange(obstacle.Hex, 1).Where(hex => hex.IsEmpty() || (hex.IsUnoccupied() && hex.GetHexObjectOfType<Trap>() != null)));
				}, mandatory: true, hintText: "Select a hex to move the obstacle to");

				if (movedToHex == null)
                {
                    return;
                }
				movedToHex.GetHexObjectOfType<Trap>()?.Destroy();

				await obstacle.TweenGlobalPosition(movedToHex.GlobalPosition, 0.3f).SetEasing(Easing.OutSine)
					.PlayFastForwardableAsync();
				await GDTask.DelayFastForwardable(0.03f);
				obstacle.SetOriginHexAndRotation(movedToHex);
			},
			EffectType.Selectable,
			canApplyMultipleTimesInEffectCollection: true,
			effectButtonParameters: new IconEffectButton.Parameters("res://Art/OverlayTiles/Boulder 1h.png"),
			effectInfoViewParameters: new TextEffectInfoView.Parameters("Move one adjacent boulder"));
	}

	protected override async GDTask OnRoomRevealed(ScenarioEvents.RoomRevealed.Parameters parameters)
	{
		await base.OnRoomRevealed(parameters);

		
	}
}
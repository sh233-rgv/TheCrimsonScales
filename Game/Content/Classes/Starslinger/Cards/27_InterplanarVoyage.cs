using System.Collections.Generic;
using Godot;
using System.Linq;

public class InterplanarVoyage : StarslingerCardModel<InterplanarVoyage.CardTop, InterplanarVoyage.CardBottom>
{
	public override string Name => "Interplanar Voyage";
	public override int Level => 9;
	public override int Initiative => 24;
	protected override int AtlasIndex => 27;

	public class CardTop : StarslingerCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(4)
				.WithPierce(2)
				.WithAOEPattern(new AOEPattern([
							new AOEHex(Vector2I.Zero, AOEHexType.Gray),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Yellow),
							new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Yellow),
							new AOEHex(Vector2I.Zero.Add(Direction.West), AOEHexType.Yellow),
							new AOEHex(Vector2I.Zero.Add(Direction.SouthWest), AOEHexType.Yellow),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthWest), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthWest).Add(Direction.NorthWest), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.SouthEast), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.SouthEast).Add(Direction.SouthEast), AOEHexType.Red)
						]))
				.Build()),
			new AbilityCardAbility(HealAbility.Builder()
				.WithHealValue(3)
				.WithTarget(Target.Allies | Target.TargetAll)
				.WithCustomGetTargets((abilityState, list) =>
				{
					AttackAbility.State attackAbilityState = abilityState.ActionState.GetAbilityState<AttackAbility.State>(0);

					if(attackAbilityState.Performed)
					{
						foreach(Hex yellowHex in attackAbilityState.GetYellowAOEHexes())
						{
							foreach(Figure figure in yellowHex.GetHexObjectsOfType<Figure>())
							{
								list.Add(figure);
							}
						}
					}
				})
				.Build()),
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					AttackAbility.State attackAbilityState = state.ActionState.GetAbilityState<AttackAbility.State>(0);
					Figure swapped = await AbilityCmd.SelectFigure(state, list => {
						GD.Print(attackAbilityState.UniqueTargetedFigures.Count);
						list.AddRange(attackAbilityState.UniqueTargetedFigures.Where(figure => !figure.IsDead &&
							figure.CanSwapWith(state.Performer) && state.Performer.CanSwapWith(figure)));
						}, mandatory: false, hintText: "Choose an enemy to swap hexes with");
					if (swapped == null)
					{
						return;
					}

					Hex performerHex = state.Performer.Hex;
					Hex swappedHex = swapped.Hex;
					state.Performer.RemoveFromMap();
					await AbilityCmd.EnterHex(state, swapped, state.Authority, performerHex, true);
					await AbilityCmd.EnterHex(state, state.Performer, state.Authority, swappedHex, true);
				})
				.Build())
		];
	}

	public class CardBottom : StarslingerCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					Room performerRoom = null;
					foreach(Room room in GameController.Instance.Map.Rooms)
					{
						if (room.MapTiles.Contains(state.Performer.Hex.MapTile))
						{
							performerRoom = room;
							break;
						}
					}
					if (performerRoom == null)
					{
						return;
					}
					Figure swapped = await AbilityCmd.SelectFigure(state, list => {
						list.AddRange(performerRoom.Hexes.SelectMany(hex => hex.GetChildrenOfType<Figure>()).Where(figure => figure != state.Performer &&
							figure.CanSwapWith(state.Performer) && state.Performer.CanSwapWith(figure)));
						}, mandatory: false, hintText: "Choose a figure to swap hexes with");
					if (swapped == null)
					{
						return;
					}

					Hex performerHex = state.Performer.Hex;
					Hex swappedHex = swapped.Hex;
					state.Performer.RemoveFromMap();
					await AbilityCmd.EnterHex(state, swapped, state.Authority, performerHex, true);
					await AbilityCmd.EnterHex(state, state.Performer, state.Authority, swappedHex, true);
				})
				.Build())
		];
	}
}
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public abstract class LuminaryCardModel<TTop, TBottom> : AtlasAbilityCardModel<TTop, TBottom>
	where TTop : LuminaryCardSide, new()
	where TBottom : LuminaryCardSide, new()
{
	protected override string TexturePath => "res://Content/Classes/Luminary/Cards.jpg";
	protected override int ColumnCount => 8;
	protected override int RowCount => 4;
}

public abstract class LuminaryCardSide : AbilityCardSide
{
	protected AbilityCardAbility Scuttle(int distance, IReadOnlyCollection<Element> possibleElements)
	{
		return new AbilityCardAbility(MoveAbility.Builder()
			.WithDistance(distance)
			.WithMoveType(MoveType.Jump)
			.WithConditionalAbilityCheck(async state =>
				{
					await GDTask.CompletedTask;

					return state.ActionState.GetAbilityState<AttackAbility.State>(0).Performed;
				})
			.WithOnAbilityEndedPerformed(async state =>
			{
				if(possibleElements.Count == 1)
				{
					await AbilityCmd.InfuseElement(possibleElements.ToList()[0]);
				}
				else
				{
					await AbilityCmd.InfuseElement(state.Performer, possibleElements);
				}
			})
			.WithOnAbilityStarted(async state =>
			{
			ScenarioCheckEvents.MoveCanStopAtCheckEvent.Subscribe(state.Performer, this,
				parameters =>
					parameters.AbilityState == state && !state.ActionState.GetAbilityState<AttackAbility.State>(0).GetEmptyAOEHexes().Contains(parameters.Hex),
					parameters =>
					{
						parameters.SetCannotStopAt();
					}
				);

				await GDTask.CompletedTask;
			})
			.WithOnAbilityEnded(async state =>
				{
					ScenarioCheckEvents.MoveCanStopAtCheckEvent.Unsubscribe(state.Performer, this);

					await GDTask.CompletedTask;
				}
			)
			.Build());
	}
	
}
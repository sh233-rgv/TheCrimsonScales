using System.Collections.Generic;
using Fractural.Tasks;

public class Scenario026 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario026.tscn";
	public override int ScenarioNumber => 26;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<ChillyScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() => new KillAlLEnemiesScenarioGoals();

	public override string BGSPath => "res://Audio/BGS/Cave.ogg";

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		GameController.Instance.Map.Treasures[0].SetItemLoot(ModelDB.Item<DizzyingTincture>()); //TODO: IronSnare

		ScenarioEvents.AbilityCardSideStartedEvent.Subscribe(this,
			parameters => !parameters.ForgoneAction,
			async parameters =>
			{
				parameters.ForgoAction();

				ActionState actionState = new ActionState(parameters.Performer, [OtherAbility.Builder()
					.WithPerformAbility(async state =>
					{
						Figure figure = await AbilityCmd.SelectFigure(state, list =>
						{
							foreach(Figure figure in RangeHelper.GetFiguresInRange(state.Performer.Hex, 2))
							{
								if(state.Authority.AlliedWith(figure) || state.Authority == figure)
								{
									list.Add(figure);
								}
							}
						});

						if(figure == null)
						{
							return;
						}

						await figure.RemoveAllChill();
					})
					.Build()]);
				await actionState.Perform();
			},
			EffectType.Selectable,
			effectButtonParameters: new IconEffectButton.Parameters(Icons.GetCondition(Conditions.Chill)),
			effectInfoViewParameters: new TextEffectInfoView.Parameters($"Remove all {Icons.Inline(Icons.GetCondition(Conditions.Chill))} from self or one of your summons.")
		);
	}
}
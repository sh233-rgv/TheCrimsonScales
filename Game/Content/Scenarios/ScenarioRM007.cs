using Fractural.Tasks;

public class ScenarioRM007 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/ScenarioRM007.tscn";
	public override string ScenarioPrefix => "RM";
	public override int ScenarioNumber => 7;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<RMScenarioChain>();

	private int _remainingEmpowerCount = 12;

	protected override ScenarioGoals CreateScenarioGoals() =>
		new KillSpecificEnemiesTypeGoals(ModelDB.Monster<RuinmawBossRoom5>(), "Kill the Ruinmaw to win this scenario.");

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		//TODO
	}

	protected override async GDTask OnRoomRevealed(ScenarioEvents.RoomRevealed.Parameters roomRevealedParameters)
	{
		await base.OnRoomRevealed(roomRevealedParameters);

		if(roomRevealedParameters.Room == GameController.Instance.Map.Rooms[1])
		{
			//TODO
		}
		else if(roomRevealedParameters.Room == GameController.Instance.Map.Rooms[3])
		{
			//TODO
		}
	}

	public async GDTask Empower(AbilityState potentialAbilityState, Figure figure)
	{
		Figure potentialConditionGiver = potentialAbilityState?.Authority;

		ScenarioEvents.InflictConditions.Parameters inflictConditionsParameters =
			await ScenarioEvents.InflictConditionsEvent.CreatePrompt(
				new ScenarioEvents.InflictConditions.Parameters(potentialAbilityState, figure, [Ruinmaw.Empower]), figure);

		foreach(ConditionModel conditionModel in inflictConditionsParameters.ConditionModels)
		{
			ScenarioEvents.InflictCondition.Parameters inflictConditionParameters =
				await ScenarioEvents.InflictConditionEvent.CreatePrompt(
					new ScenarioEvents.InflictCondition.Parameters(potentialAbilityState, figure, potentialConditionGiver, conditionModel), figure);

			if(!inflictConditionParameters.Prevented)
			{
				if(conditionModel == Ruinmaw.Empower)
				{
					if(_remainingEmpowerCount == 0)
					{
						continue;
					}

					_remainingEmpowerCount--;
					AMDCard card = new AMDCard(ModelDB.AMDCard<RuinmawEmpowerAMDCard>(), figure.AMDCardDeck.Owner);
					ScenarioEvents.EmpowerAdded.Parameters empowerAddedParameters =
						await ScenarioEvents.EmpowerAddedEvent.CreatePrompt(
							new ScenarioEvents.EmpowerAdded.Parameters(figure));

					card.DrawnEvent += OnEmpowerDrawn;

					figure.AMDCardDeck.AddCard(card, empowerAddedParameters.ShuffleDrawPile);

					await ScenarioEvents.ConditionAddedEvent.CreatePrompt(
						new ScenarioEvents.ConditionAdded.Parameters(potentialAbilityState, figure, potentialConditionGiver, conditionModel), figure);
				}
				else
				{
					ScenarioEvents.InflictConditionDuplicatesCheck.Parameters inflictConditionDuplicatesCheckParameters =
						await ScenarioEvents.InflictConditionDuplicatesCheckEvent.CreatePrompt(
							new ScenarioEvents.InflictConditionDuplicatesCheck.Parameters(potentialAbilityState, figure, conditionModel), figure);

					if(!inflictConditionDuplicatesCheckParameters.Prevented)
					{
						if(inflictConditionDuplicatesCheckParameters.AddStack)
						{
							await figure.AddConditionStack(conditionModel);
						}
						else
						{
							await figure.AddCondition(conditionModel, potentialAbilityState?.Performer);
						}

						await ScenarioEvents.ConditionAddedEvent.CreatePrompt(
							new ScenarioEvents.ConditionAdded.Parameters(potentialAbilityState, figure, potentialConditionGiver, conditionModel),
							figure);
					}
				}
			}
		}
	}

	private void OnEmpowerDrawn(AMDCard card)
	{
		_remainingEmpowerCount++;
	}
}
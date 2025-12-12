using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class Scenario045 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario045.tscn";
	public override int ScenarioNumber => 45;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<EventScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() => new KillSpecificEnemiesTypeGoals(
		ModelDB.Monster<LandLeviathan>(), "Kill the Land Leviathan to win this scenario.");

	IEnumerable<Hex> _markerAHexes;

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		_markerAHexes = GameController.Instance.Map.GetMarkers(Marker.Type.a).Select(marker => marker.Hex);
	}

	protected override async GDTask OnRoomRevealed(ScenarioEvents.RoomRevealed.Parameters parameters)
	{
		await base.OnRoomRevealed(parameters);

		await AbilityCmd.GenericChoice(GameController.Instance.Map.Figures.First(figure => figure is Character),
		[
			ScenarioEvents.GenericChoice.Subscription.New(canApplyFunction: canApplyParameters => true,
				applyFunction: async applyParameters =>
				{
					await SpawnMonster(null, ModelDB.Monster<CaveBear>(), MonsterType.Normal);
					await SpawnMonster(null, ModelDB.Monster<CaveBear>(), MonsterType.Normal);
					foreach(Figure figure in GameController.Instance.Map.Figures.Where(figure => figure is Monster monster && monster.MonsterModel is CaveBear))
                    {
                        figure.SetAlignment(Alignment.Characters);
                        figure.SetEnemies(Alignment.Enemies);
						ConditionImmunityTrait curseImmunity = new ConditionImmunityTrait(Conditions.Curse);
						await curseImmunity.Activate(figure);
						ScenarioEvents.FigureKilledEvent.Subscribe(this, figure,
							parameters => parameters.Figure == figure,
							async parameters =>
                            {
                                await curseImmunity.Deactivate(figure);
								ScenarioEvents.FigureKilledEvent.Unsubscribe(this, figure);
                            });
                    }
				},
				effectButtonParameters: new IconEffectButton.Parameters("res://Content/Monsters/CaveBear/Portrait.jpg"),
				effectInfoViewParameters: new TextEffectInfoView.Parameters($"Spawn two normal Cave Bears"),
				effectType: EffectType.SelectableMandatory
			),
			ScenarioEvents.GenericChoice.Subscription.New(canApplyFunction: canApplyParameters => true,
				applyFunction: async applyParameters =>
				{
					await SpawnMonster(null, ModelDB.Monster<RendingDrake>(), MonsterType.Normal);
					await SpawnMonster(null, ModelDB.Monster<SpittingDrake>(), MonsterType.Normal);
					foreach(Figure figure in GameController.Instance.Map.Figures.Where(figure => figure is Monster monster && (monster.MonsterModel is RendingDrake || monster.MonsterModel is SpittingDrake)))
                    {
                        figure.SetAlignment(Alignment.Characters);
                        figure.SetEnemies(Alignment.Enemies);
						ConditionImmunityTrait curseImmunity = new ConditionImmunityTrait(Conditions.Curse);
						await curseImmunity.Activate(figure);
						ScenarioEvents.FigureKilledEvent.Subscribe(this, figure,
							parameters => parameters.Figure == figure,
							async parameters =>
                            {
                                await curseImmunity.Deactivate(figure);
								ScenarioEvents.FigureKilledEvent.Unsubscribe(this, figure);
                            });
                    }
				},
				effectButtonParameters: new IconEffectButton.Parameters("res://Content/Monsters/RendingDrake/Portrait.jpg"),
				effectInfoViewParameters: new TextEffectInfoView.Parameters($"Spawn one normal Rending Drake and one normal Spitting Drake"),
				effectType: EffectType.SelectableMandatory
			),
			ScenarioEvents.GenericChoice.Subscription.New(canApplyFunction: canApplyParameters => true,
				applyFunction: async applyParameters =>
				{
					await SpawnMonster(null, ModelDB.Monster<Lurker>(), MonsterType.Normal);
					await SpawnMonster(null, ModelDB.Monster<Lurker>(), MonsterType.Normal);
					await SpawnMonster(null, ModelDB.Monster<GiantViper>(), MonsterType.Elite);
					foreach(Figure figure in GameController.Instance.Map.Figures.Where(figure => figure is Monster monster && (monster.MonsterModel is GiantViper || monster.MonsterModel is Lurker)))
                    {
                        figure.SetAlignment(Alignment.Characters);
                        figure.SetEnemies(Alignment.Enemies);
						ConditionImmunityTrait curseImmunity = new ConditionImmunityTrait(Conditions.Curse);
						await curseImmunity.Activate(figure);
						ScenarioEvents.FigureKilledEvent.Subscribe(this, figure,
							parameters => parameters.Figure == figure,
							async parameters =>
                            {
                                await curseImmunity.Deactivate(figure);
								ScenarioEvents.FigureKilledEvent.Unsubscribe(this, figure);
                            });
                    }
				},
				effectButtonParameters: new IconEffectButton.Parameters("res://Content/Monsters/Lurker/Portrait.jpg"),
				effectInfoViewParameters: new TextEffectInfoView.Parameters($"Spawn two normal Lurkers and one elite Giant Viper"),
				effectType: EffectType.SelectableMandatory
			),
		], hintText: "Choose a group of monsters to spawn");
	}

	private async GDTask SpawnMonster(Figure authority, MonsterModel monsterModel, MonsterType monsterType)
    {
		await SpawnMonster(authority, monsterModel, monsterType, _markerAHexes, GameController.Instance.SavedScenario.ScenarioLevel - 1);
    }
}
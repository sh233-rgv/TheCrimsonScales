using System.Collections.Generic;
using Fractural.Tasks;

public abstract class DummyScenario : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario002.tscn";

	public override int ScenarioNumber => 5;
	public override string Name => "TODO";

	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<MainCampaignScenarioChain>();
	public override IEnumerable<ScenarioConnection> Connections => [];

	public override string IntroductionText =>
		"""
		TODO
		""";

	public override string ConclusionText =>
		"""
		TODO
		""";

	public override List<MonsterModel> MonsterModels { get; } =
	[
		//TODO
	];

	public override List<SavedReward> Rewards { get; } =
	[
		//TODO
	];

	public override async GDTask InitializeAfterFirstRoomRevealed()
	{
		await base.InitializeAfterFirstRoomRevealed();

		await AddGoal(new KillAllEnemiesScenarioGoal());
	}
}

public class Scenario025 : ScenarioModel
{
	public override int ScenarioNumber => 25;

	public override string ScenePath => "res://Content/Scenarios/Scenario002.tscn";
	public override string Name => "TODO";

	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<MainCampaignScenarioChain>();
	public override IEnumerable<ScenarioConnection> Connections => [];

	public override string IntroductionText =>
		"""
		TODO
		""";

	public override string ConclusionText =>
		"""
		TODO
		""";

	public override List<MonsterModel> MonsterModels { get; } =
	[
		//TODO
	];

	public override List<SavedReward> Rewards { get; } =
	[
		//TODO
	];
}

public class Scenario031 : ScenarioModel
{
	public override int ScenarioNumber => 31;

	public override string ScenePath => "res://Content/Scenarios/Scenario002.tscn";
	public override string Name => "TODO";

	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<MainCampaignScenarioChain>();
	public override IEnumerable<ScenarioConnection> Connections => [];

	public override string IntroductionText =>
		"""
		TODO
		""";

	public override string ConclusionText =>
		"""
		TODO
		""";

	public override List<MonsterModel> MonsterModels { get; } =
	[
		//TODO
	];

	public override List<SavedReward> Rewards { get; } =
	[
		//TODO
	];
}

public class Scenario032 : ScenarioModel
{
	public override int ScenarioNumber => 32;

	public override string ScenePath => "res://Content/Scenarios/Scenario002.tscn";
	public override string Name => "TODO";

	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<MainCampaignScenarioChain>();
	public override IEnumerable<ScenarioConnection> Connections => [];

	public override string IntroductionText =>
		"""
		TODO
		""";

	public override string ConclusionText =>
		"""
		TODO
		""";

	public override List<MonsterModel> MonsterModels { get; } =
	[
		//TODO
	];

	public override List<SavedReward> Rewards { get; } =
	[
		//TODO
	];
}

public class Scenario034 : ScenarioModel
{
	public override int ScenarioNumber => 34;

	public override string ScenePath => "res://Content/Scenarios/Scenario002.tscn";
	public override string Name => "TODO";

	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<MainCampaignScenarioChain>();
	public override IEnumerable<ScenarioConnection> Connections => [];

	public override string IntroductionText =>
		"""
		TODO
		""";

	public override string ConclusionText =>
		"""
		TODO
		""";

	public override List<MonsterModel> MonsterModels { get; } =
	[
		//TODO
	];

	public override List<SavedReward> Rewards { get; } =
	[
		//TODO
	];
}

public class Scenario035 : ScenarioModel
{
	public override int ScenarioNumber => 35;

	public override string ScenePath => "res://Content/Scenarios/Scenario002.tscn";
	public override string Name => "TODO";

	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<MainCampaignScenarioChain>();
	public override IEnumerable<ScenarioConnection> Connections => [];

	public override string IntroductionText =>
		"""
		TODO
		""";

	public override string ConclusionText =>
		"""
		TODO
		""";

	public override List<MonsterModel> MonsterModels { get; } =
	[
		//TODO
	];

	public override List<SavedReward> Rewards { get; } =
	[
		//TODO
	];
}

public class Scenario052 : ScenarioModel
{
	public override int ScenarioNumber => 52;

	public override string ScenePath => "res://Content/Scenarios/Scenario002.tscn";
	public override string Name => "TODO";

	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<MainCampaignScenarioChain>();
	public override IEnumerable<ScenarioConnection> Connections => [];

	public override string IntroductionText =>
		"""
		TODO
		""";

	public override string ConclusionText =>
		"""
		TODO
		""";

	public override List<MonsterModel> MonsterModels { get; } =
	[
		//TODO
	];

	public override List<SavedReward> Rewards { get; } =
	[
		//TODO
	];
}

public class Scenario054 : ScenarioModel
{
	public override int ScenarioNumber => 54;

	public override string ScenePath => "res://Content/Scenarios/Scenario002.tscn";
	public override string Name => "TODO";

	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<MainCampaignScenarioChain>();
	public override IEnumerable<ScenarioConnection> Connections => [];

	public override string IntroductionText =>
		"""
		TODO
		""";

	public override string ConclusionText =>
		"""
		TODO
		""";

	public override List<MonsterModel> MonsterModels { get; } =
	[
		//TODO
	];

	public override List<SavedReward> Rewards { get; } =
	[
		//TODO
	];
}

public class Scenario055 : ScenarioModel
{
	public override int ScenarioNumber => 55;

	public override string ScenePath => "res://Content/Scenarios/Scenario002.tscn";
	public override string Name => "TODO";

	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<MainCampaignScenarioChain>();
	public override IEnumerable<ScenarioConnection> Connections => [];

	public override string IntroductionText =>
		"""
		TODO
		""";

	public override string ConclusionText =>
		"""
		TODO
		""";

	public override List<MonsterModel> MonsterModels { get; } =
	[
		//TODO
	];

	public override List<SavedReward> Rewards { get; } =
	[
		//TODO
	];
}
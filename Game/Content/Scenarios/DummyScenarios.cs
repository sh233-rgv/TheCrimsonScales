using System.Collections.Generic;
using Fractural.Tasks;

public abstract class DummyScenario : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario002.tscn";

	public override int ScenarioNumber => 2;
	public override string Name => "TODO";

	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<MainCampaignScenarioChain>();
	public override IEnumerable<ScenarioConnection> Connections => [new ScenarioConnection<Scenario003>(true)];

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

	public override List<Reward> Rewards { get; } =
	[
		//TODO
	];

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		AddGoal(new KillAllEnemiesScenarioGoal());
	}
}

public class Scenario004 : DummyScenario
{
}

public class Scenario005 : DummyScenario
{
}

public class Scenario006 : DummyScenario
{
}

public class Scenario007 : DummyScenario
{
}

public class Scenario008 : DummyScenario
{
}

public class Scenario009 : DummyScenario
{
}

public class Scenario010 : DummyScenario
{
}

public class Scenario011 : DummyScenario
{
}

public class Scenario012 : DummyScenario
{
}

public class Scenario013 : DummyScenario
{
}

public class Scenario014 : DummyScenario
{
}

public class Scenario015 : DummyScenario
{
}

public class Scenario016 : DummyScenario
{
}

public class Scenario017 : DummyScenario
{
}

public class Scenario018 : DummyScenario
{
}

public class Scenario019 : DummyScenario
{
}

public class Scenario020 : DummyScenario
{
}

public class Scenario021 : DummyScenario
{
}

public class Scenario022 : DummyScenario
{
}

public class Scenario023 : DummyScenario
{
}

public class Scenario024 : DummyScenario
{
}

public class Scenario025 : DummyScenario
{
}

public class Scenario026 : DummyScenario
{
}

public class Scenario027 : DummyScenario
{
}

public class Scenario028 : DummyScenario
{
}

public class Scenario029 : DummyScenario
{
}

public class Scenario030 : DummyScenario
{
}

public class Scenario031 : DummyScenario
{
}

public class Scenario032 : DummyScenario
{
}

public class Scenario033 : DummyScenario
{
}

public class Scenario034 : DummyScenario
{
}

public class Scenario035 : DummyScenario
{
}

public class Scenario036 : DummyScenario
{
}

public class Scenario037 : DummyScenario
{
}

public class Scenario038 : DummyScenario
{
}

public class Scenario039 : DummyScenario
{
}

public class Scenario040 : DummyScenario
{
}

public class Scenario041 : DummyScenario
{
}

public class Scenario042 : DummyScenario
{
}

public class Scenario043 : DummyScenario
{
}

public class Scenario044 : DummyScenario
{
}

public class Scenario045 : DummyScenario
{
}

public class Scenario046 : DummyScenario
{
}

public class Scenario047 : DummyScenario
{
}

public class Scenario048 : DummyScenario
{
}

public class Scenario049 : DummyScenario
{
}

public class Scenario050 : DummyScenario
{
}

public class Scenario051 : DummyScenario
{
}

public class Scenario052 : DummyScenario
{
}

public class Scenario053 : DummyScenario
{
}

public class Scenario054 : DummyScenario
{
}

public class Scenario055 : DummyScenario
{
}
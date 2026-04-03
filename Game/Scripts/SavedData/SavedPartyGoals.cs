using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class SavedPartyGoals
{
	private static readonly PartyGoalModel[] AllPartyGoals =
	[
		ModelDB.PartyGoal<BattleGoalsPartyGoal>(),
	];

	[JsonProperty]
	public List<SavedPartyGoal> PartyGoals { get; private set; }

	public SavedPartyGoals()
	{
		PartyGoals = AllPartyGoals.Select(personalQuestModel => new SavedPartyGoal(personalQuestModel)).ToList();
	}
}
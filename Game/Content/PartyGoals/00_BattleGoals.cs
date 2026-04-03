using System;
using Fractural.Tasks;

public class BattleGoalsPartyGoal : PartyGoalModel<PartyGoalData>
{
	public override bool ScalesWithCharacterCount => true;

	public override string GetText(int characterCount) => $"Complete 5 battle goals with at least {characterCount} characters";
}
using System.Collections.Generic;
using Fractural.Tasks;

public class ChainguardMasteries
{
	public abstract class ChainguardMastery : MasteryModel
	{
	}

	public class Mastery1 : ChainguardMastery
	{
		public override string Description => $"Give {Icons.Inline(Icons.GetCondition(Chainguard.Shackle))} to 30 different enemies.";

		public override bool SingleScenario => false;

		public override async GDTask OnScenarioSetupPhaseCompleted(Mastery mastery, Character character)
		{
			await base.OnScenarioSetupPhaseCompleted(mastery, character);

			List<Figure> shackledMonsters = [];

			ScenarioEvents.ConditionAddedEvent.Subscribe(this,
				parameters => parameters.PotentialConditionGiver == character && parameters.ConditionModel is Shackle &&
				              character.EnemiesWith(parameters.Target) && !shackledMonsters.Contains(parameters.Target),
				async parameters =>
				{
					shackledMonsters.Add(parameters.Target);
					mastery.AdjustProgress();
					await GDTask.CompletedTask;
				});
		}
	}

	public class Mastery2 : ChainguardMastery
	{
		public override string Description =>
			$"Cause enemies with {Icons.Inline(Icons.GetCondition(Chainguard.Shackle))} to suffer at least {Icons.Inline(Icons.Damage)}20 in a single round.";

		public override int MaxProgress => 20;

		public override async GDTask OnScenarioSetupPhaseCompleted(Mastery mastery, Character character)
		{
			await base.OnScenarioSetupPhaseCompleted(mastery, character);

			ScenarioEvents.AfterSufferDamageEvent.Subscribe(this,
				parameters => parameters.SufferDamageParameters.PotentialDamageDealer == character &&
				              character.EnemiesWith(parameters.Figure) && parameters.Figure.HasCondition(Chainguard.Shackle),
				async parameters =>
				{
					mastery.AdjustProgress(parameters.DamageSuffered);
					await GDTask.CompletedTask;
				});

			ScenarioEvents.RoundEndedEvent.Subscribe(this,
				_ => !mastery.ProgressFull,
				async _ =>
				{
					mastery.ResetProgress();
					await GDTask.CompletedTask;
				});
		}
	}
}
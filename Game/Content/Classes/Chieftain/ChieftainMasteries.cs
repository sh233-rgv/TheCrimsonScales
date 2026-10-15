using System.Collections.Generic;
using Fractural.Tasks;

public class ChieftainMasteries
{
	public abstract class ChieftainMastery : MasteryModel
	{
	}

	public class Mastery1 : ChieftainMastery
	{
		public override string Description => "For an entire scenario, end each of your turns mounted on a summon you summoned on the first round.";

		public override async GDTask OnScenarioSetupPhaseCompleted(Mastery mastery, Character character)
		{
			await base.OnScenarioSetupPhaseCompleted(mastery, character);

			mastery.AdjustProgress();

			List<Summon> firstRoundSummons = [];

			ScenarioEvents.AbilityPerformedEvent.Subscribe(this,
				parameters => parameters.Performer == character && parameters.AbilityState is SummonAbility.State,
				async parameters =>
				{
					firstRoundSummons.Add(((SummonAbility.State)parameters.AbilityState).Summon);
					await GDTask.CompletedTask;
				});

			ScenarioEvents.RoundEndedEvent.Subscribe(this,
				_ => true,
				async _ =>
				{
					ScenarioEvents.AbilityPerformedEvent.Unsubscribe(this);
					ScenarioEvents.RoundEndedEvent.Unsubscribe(this);
					await GDTask.CompletedTask;
				});

			ScenarioEvents.FigureTurnEndedEvent.Subscribe(this,
				parameters => parameters.Figure == character && !firstRoundSummons.Contains((Summon)Chieftain.GetMount(character)),
				async _ =>
				{
					mastery.ResetProgress();
					mastery.SetTemporarilyFailed(true);
					await GDTask.CompletedTask;
				});
		}
	}

	public class Mastery2 : ChieftainMastery
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
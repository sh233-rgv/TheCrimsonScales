using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class BombardMasteries
{
	public abstract class BombardMastery : MasteryModel
	{
	}

	public class Mastery1 : BombardMastery
	{
		private Dictionary<AbilityCardSide, int> _projectileSides = [];

		public override string Description =>
			$"In a single scenario, successfully resolve 4 different {Icons.Inline(BombardCardSide.ProjectileIconPath)} abilities twice each.";

		public override int MaxProgress => 4;

		public override async GDTask OnScenarioSetupPhaseCompleted(Character character)
		{
			await base.OnScenarioSetupPhaseCompleted(character);

			ScenarioEvents.AbilityPerformedEvent.Subscribe(this,
				parameters => parameters.Performer == character &&
				              (parameters.AbilityState.ActionState.ParentActionState?.AbilityStates.Any(state => state is ProjectileAbility.State) ??
				               false),
				async parameters =>
				{
					AbilityCardSide cardSide = (AbilityCardSide)parameters.AbilityState.ActionState.ActionSource;
					if(_projectileSides.ContainsKey(cardSide))
					{
						if(_projectileSides[cardSide] == 1)
						{
							await AdjustProgress();
							_projectileSides[cardSide]++;
						}
					}
					else
					{
						_projectileSides.Add(cardSide, 1);
					}
				});
		}

		public override async GDTask OnScenarioCompleted(Character character)
		{
			await base.OnScenarioCompleted(character);

			_projectileSides.Clear();
		}
	}

	public class Mastery2 : BombardMastery
	{
		public override string Description =>
			$"For an entire scenario, deal more {Icons.Inline(Icons.Damage)} with {Icons.Inline(Icons.Retaliate)} than you suffer whenever you are attacked, and be targeted by at least 5 attacks.";

		public override int MaxProgress => 5;

		public override async GDTask OnScenarioSetupPhaseCompleted(Character character)
		{
			await base.OnScenarioSetupPhaseCompleted(character);

			ScenarioEvents.AfterAttackPerformedEvent.Subscribe(this,
				parameters => parameters.AbilityState.Target == character,
				async parameters =>
				{
					await AdjustProgress();
				});

			ScenarioEvents.AfterSufferDamageEvent.Subscribe(this,
				parameters => parameters.SufferDamageParameters.FromAttack && parameters.Figure == character,
				async parameters =>
				{
					ScenarioEvents.RetaliateEvent.Subscribe(parameters.PotentialAbilityState, this,
						retaliateParameters => retaliateParameters.RetaliatingFigure == character &&
						                       retaliateParameters.AbilityState == parameters.PotentialAbilityState &&
						                       retaliateParameters.Retaliate <= parameters.DamageSuffered,
						async retaliateParameters =>
						{
							await SetFailed(true);
						}, order: 1000);

					await GDTask.CompletedTask;
				});
		}
	}
}
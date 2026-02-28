using Fractural.Tasks;

public class ShieldRangedAttacksTrait(int shield) : FigureTrait
{
	public override async GDTask Activate(Figure figure)
	{
		await base.Activate(figure);

		ScenarioCheckEvents.ShieldCheckEvent.Subscribe(figure, this,
			canApplyParameters => canApplyParameters.Figure == figure,
			applyParameters =>
			{
				applyParameters.AdjustShield(shield);
			});

		ScenarioEvents.SufferDamageEvent.Subscribe(figure, this,
			canApplyParameters => canApplyParameters.Figure == figure && canApplyParameters.FromAttack &&
			                      ((AttackAbility.State)canApplyParameters.PotentialAbilityState).SingleTargetRangeType == RangeType.Range,
			async applyParameters =>
			{
				applyParameters.AdjustShield(shield);
				await GDTask.CompletedTask;
			});

		ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Subscribe(figure, this,
			parameters => parameters.Figure == figure,
			parameters => parameters.Add(
				new InfoTextExtraEffect.Parameters($"This figure gains {Icons.Inline(Icons.Shield)}{shield} that only applies for ranged attacks."))
		);

		//figure.UpdateShield();
	}

	public override async GDTask Deactivate(Figure figure)
	{
		await base.Deactivate(figure);

		ScenarioCheckEvents.ShieldCheckEvent.Unsubscribe(figure, this);
		ScenarioEvents.SufferDamageEvent.Unsubscribe(figure, this);
		ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Unsubscribe(figure, this);
	}
}
using System;
using System.Collections.Generic;
using Fractural.Tasks;

public class RuinmawBossRoom5 : RuinmawBoss, IBossMonsterModel
{
	private bool _sated;
	private bool _satedAppliedThisTurn;
	public SatedIndicator SatedIndicator;

	//TODO: Change from text to sated Icon (Requires AMDs)
	public override string GetSpecial1Description(Monster monster) =>
		$"""
		 Rip and Tear - 
		 {Icons.Inline(Icons.Move)}{monster.Stats.Move}, {Icons.Inline(Icons.Jump)}
		 {Icons.Inline(Icons.Attack)}{monster.Stats.Attack}, {Icons.Inline(Icons.Targets)}{CharacterCount - 1}, {Icons.Inline(Icons.GetCondition(Conditions.Wound1))}, {Icons.Inline(Icons.GetCondition(Conditions.Rupture))}
		 Sated: +1{Icons.Inline(Icons.Push)}2, {Icons.Inline(Icons.Attack)}, {Icons.Inline(Icons.Pierce)}2
		 """;

	public override string GetSpecial2Description(Monster monster) =>
		$"""
		 Heartripper -
		 Focus the enemy with the most damage.
		 {Icons.Inline(Icons.Move)}{monster.Stats.Move + 2}
		 Sated: {Icons.Inline(Icons.Move)}+2
		 {Icons.Inline(Icons.Attack)}{monster.Stats.Attack + 2}
		 Sated: +2{Icons.Inline(Icons.Attack)}, advantage
		 Sated: {Icons.Inline(Icons.GetCondition(Ruinmaw.Empower))}, {Icons.Inline(Icons.GetCondition(Ruinmaw.Empower))}, self
		 """;

	public override IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.MoveAbility(monster, +0).WithMoveType(MoveType.Jump)),
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, +0)
			.WithTargets(CharacterCount - 1)
			.WithConditions([Conditions.Wound1, Conditions.Rupture])
			.WithDuringAttackSubscription(
				ScenarioEvents.DuringAttack.Subscription.New(
					_ => _sated,
					async parameters =>
					{
						parameters.AbilityState.AbilityAdjustAttackValue(1);
						parameters.AbilityState.AbilityAdjustPierce(2);
						parameters.AbilityState.AbilityAdjustPush(2);
						await GDTask.CompletedTask;
					}
				)
			)
		)
	];

	public Action<ScenarioCheckEvents.FigureFocusCheck.Parameters> AdjustFocusSpecial2 =>
		parameters => parameters.SetFocusMostDamage();

	public override IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.MoveAbility(monster, +2)
			.WithDuringMovementSubscription(
				ScenarioEvents.DuringMovement.Subscription.New(
					_ => _sated,
					async parameters =>
					{
						parameters.AbilityState.AdjustMoveValue(2);
						await GDTask.CompletedTask;
					}))),
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, +2)
			.WithDuringAttackSubscription(
				ScenarioEvents.DuringAttack.Subscription.New(
					_ => _sated,
					async parameters =>
					{
						parameters.AbilityState.AbilityAdjustAttackValue(2);
						parameters.AbilityState.AbilitySetHasAdvantage();
						await GDTask.CompletedTask;
					}))),
		new MonsterAbilityCardAbility(OtherTargetedAbility.Builder()
			.WithOnAfterTargetConfirmed(async (state, figure) =>
			{
				if(GameController.Instance.ScenarioModel is ScenarioRM007 scenarioRM007)
				{
					await scenarioRM007.Empower(state, figure);
					await scenarioRM007.Empower(state, figure);
				}
			})
			.WithTarget(Target.Self)
			.WithConditionalAbilityCheck(async _ =>
			{
				await GDTask.CompletedTask;
				return _sated;
			})
		)
	];

	public void Sate(Monster monster)
	{
		if(monster.TakingTurn)
		{
			_satedAppliedThisTurn = true;
		}

		if(_sated)
		{
			return;
		}

		SatedIndicator.ShowAnimated();
		object subscriber = new object();
		ScenarioEvents.FigureTurnEndedEvent.Subscribe(monster, subscriber,
			canApplyParameters => canApplyParameters.Figure == monster,
			async _ =>
			{
				if(_satedAppliedThisTurn)
				{
					_satedAppliedThisTurn = false;
				}
				else
				{
					SatedIndicator.HideAnimated();

					ScenarioEvents.FigureTurnEndedEvent.Unsubscribe(monster, subscriber);
					ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Unsubscribe(monster, subscriber);
				}

				await GDTask.CompletedTask;
			});
		ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Subscribe(monster, subscriber,
			parameters => parameters.Figure == monster,
			parameters =>
			{
				parameters.Add(new InfoTextExtraEffect.Parameters($"{Icons.Inline("res://Content/Classes/Ruinmaw/RuinmawSated.png")}"));
			}
		);

		_sated = true;
	}
}
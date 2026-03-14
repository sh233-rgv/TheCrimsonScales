using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class RuinmawBossRoom2 : RuinmawBoss
{
	public override string GetSpecial1Description(Monster monster) =>
		$"""
		 Berserk Barrage - 
		 {Icons.Inline(Icons.Move)}{monster.Stats.Move + 1}
		 {Icons.Inline(Icons.Attack)}{monster.Stats.Attack}, {Icons.Inline(Icons.Pierce)}1, {Icons.Inline(Icons.GetCondition(Conditions.Rupture))}, area of effect
		 """;

	public override string GetSpecial2Description(Monster monster) =>
		$"""
		 Dig In -
		 {Icons.Inline(Icons.GetCondition(Conditions.Invisible))}, self
		 The first time an enemy ends their turn adjacent to this figure, perform:
		 {Icons.Inline(Icons.Attack)}{monster.Stats.Attack + 1}, {Icons.Inline(Icons.GetCondition(Conditions.Wound1))}
		 Remove {Icons.Inline(Icons.GetCondition(Conditions.Invisible))} from self.
		 At the end of the round, if this figure has {Icons.Inline(Icons.GetCondition(Conditions.Invisible))}, perform:
		 {Icons.Inline(Icons.Move)}{monster.Stats.Move + 4}
		 {Icons.Inline(Icons.Attack)}{monster.Stats.Attack}, advantage
		 Remove {Icons.Inline(Icons.GetCondition(Conditions.Invisible))} from self.
		 """;

	public override IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.MoveAbility(monster, +1)),
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, +0)
			.WithConditions(Conditions.Rupture)
			.WithPierce(1)
			.WithAOEPattern(new AOEPattern(
				[
					new AOEHex(Vector2I.Zero, AOEHexType.Gray),
					new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
					new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
					new AOEHex(Vector2I.Zero.Add(Direction.NorthWest), AOEHexType.Red),
				]
			)))
	];

	public override IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(ConditionAbility.Builder()
			.WithConditions(Conditions.Invisible)
			.WithTarget(Target.Self)),
		new MonsterAbilityCardAbility(OtherActiveAbility.Builder()
			.WithOnActivate(async state =>
			{
				ScenarioEvents.FigureTurnEndedEvent.Subscribe(state, this,
					canApplyParameters => canApplyParameters.Figure.EnemiesWith(state.Performer) &&
					                      RangeHelper.Distance(state.Performer.Hex, canApplyParameters.Figure.Hex) <= 1,
					async _ =>
					{
						ActionState actionState = new ActionState(state.Performer,
						[
							MonsterAbilityCardModel.AttackAbility(monster, +1).WithConditions(Conditions.Wound1)
						]);
						await actionState.Perform();
						await AbilityCmd.RemoveCondition(state.Performer, Conditions.Invisible);
						ScenarioEvents.FigureTurnEndedEvent.Unsubscribe(state, this);
					});

				ScenarioEvents.RoundEndedEvent.Subscribe(state, this,
					_ => state.Performer.HasCondition(Conditions.Invisible),
					async _ =>
					{
						ActionState actionState = new ActionState(state.Performer,
						[
							MonsterAbilityCardModel.MoveAbility(monster, +4),
							MonsterAbilityCardModel.AttackAbility(monster, +0).WithAdvantage()
						]);
						await actionState.Perform();
						await AbilityCmd.RemoveCondition(state.Performer, Conditions.Invisible);
						await GDTask.CompletedTask;
					});
				await GDTask.CompletedTask;
			})
			.WithOnDeactivate(async state =>
			{
				ScenarioEvents.FigureTurnEndedEvent.Unsubscribe(state, this);
				ScenarioEvents.RoundEndedEvent.Unsubscribe(state, this);

				await GDTask.CompletedTask;
			}))
	];
}
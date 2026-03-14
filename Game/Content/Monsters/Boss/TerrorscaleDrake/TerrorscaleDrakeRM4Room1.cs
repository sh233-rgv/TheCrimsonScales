using System.Collections.Generic;
using System.Linq;

public class TerrorscaleDrakeRM4Room1 : TerrorscaleDrake
{
	public override string GetSpecial1Description(Monster monster) =>
		$"""
		 Claws like Spears -
		 {Icons.Inline(Icons.Attack)}{monster.Stats.Attack + 1}, {Icons.Inline(Icons.Pierce)}3
		 All enemies adjacent to the target suffer {Icons.Inline(Icons.Damage)}1.
		 Destroy the occupied obstacle.
		 """;

	public override string GetSpecial2Description(Monster monster) =>
		$"""
		 Scales like Tenfold Shields - 
		 {Icons.Inline(Icons.Attack)}{monster.Stats.Attack - 2}
		 {Icons.Inline(Icons.Shield)}{monster.Stats.CustomValue}
		 {Icons.Inline(Icons.Retaliate)}{monster.Stats.CustomValue}
		 {Icons.Inline(Icons.Heal)}{monster.Stats.CustomValue}
		 Destroy the occupied obstacle.
		 """;

	public override IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, +1)
			.WithPierce(3)
			.WithAfterAttackPerformedSubscription(
				ScenarioEvents.AfterAttackPerformed.Subscription.New(
					applyFunction: async parameters =>
					{
						foreach(Figure figure in RangeHelper.GetFiguresInRange(parameters.AbilityState.Target.Hex, 1).Where(figure =>
							        figure.EnemiesWith(parameters.Performer) && figure != parameters.AbilityState.Target))
						{
							await AbilityCmd.SufferDamage(parameters.AbilityState, figure, 1);
						}
					}))),
		new MonsterAbilityCardAbility(OtherAbility.Builder()
			.WithPerformAbility(async state =>
			{
				if(state.Performer.Hex.TryGetHexObjectOfType(out Obstacle obstacle))
				{
					await AbilityCmd.TryDestroyObstacle(obstacle);
				}
			}))
	];

	public override IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, -2)),
		new MonsterAbilityCardAbility(ShieldAbility.Builder().WithShieldValue(monster.Stats.CustomValue)),
		new MonsterAbilityCardAbility(RetaliateAbility.Builder().WithRetaliateValue(monster.Stats.CustomValue)),
		new MonsterAbilityCardAbility(HealAbility.Builder().WithHealValue(monster.Stats.CustomValue)),
		new MonsterAbilityCardAbility(OtherAbility.Builder()
			.WithPerformAbility(async state =>
			{
				if(state.Performer.Hex.TryGetHexObjectOfType(out Obstacle obstacle))
				{
					await AbilityCmd.TryDestroyObstacle(obstacle);
				}
			}))
	];
}
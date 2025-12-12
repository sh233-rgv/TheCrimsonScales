using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class LandLeviathan : MonsterModel, IBossMonsterModel
{
	private bool _summonBlackImp = true;
	public override MonsterStats[] BossLevelStats =>
	[
		new MonsterStats()
		{
			Health = 5 * (CharacterCount * 2 - 1),
			Attack = 3,
			Traits = [ConditionImmunityTrait.PoisonImmunityTrait(), new ConditionImmunityTrait(Conditions.Stun),
				new ConditionImmunityTrait(Conditions.Disarm), new ForcedMovementImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 6 * (CharacterCount * 2 - 1),
			Attack = 3,
			Traits = [new RetaliateTrait(1), ConditionImmunityTrait.PoisonImmunityTrait(), new ConditionImmunityTrait(Conditions.Stun),
				new ConditionImmunityTrait(Conditions.Disarm), new ForcedMovementImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 7 * (CharacterCount * 2 - 1),
			Attack = 4,
			Traits = [new RetaliateTrait(1), ConditionImmunityTrait.PoisonImmunityTrait(), new ConditionImmunityTrait(Conditions.Stun),
				new ConditionImmunityTrait(Conditions.Disarm), new ForcedMovementImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 8 * (CharacterCount * 2 - 1),
			Attack = 4,
			Traits = [new RetaliateTrait(2), ConditionImmunityTrait.PoisonImmunityTrait(), new ConditionImmunityTrait(Conditions.Stun),
				new ConditionImmunityTrait(Conditions.Disarm), new ForcedMovementImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 9 * (CharacterCount * 2 - 1),
			Attack = 5,
			Traits = [new RetaliateTrait(2), ConditionImmunityTrait.PoisonImmunityTrait(), new ConditionImmunityTrait(Conditions.Stun),
				new ConditionImmunityTrait(Conditions.Disarm), new ForcedMovementImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 11 * (CharacterCount * 2 - 1),
			Attack = 5,
			Traits = [new RetaliateTrait(3), ConditionImmunityTrait.PoisonImmunityTrait(), new ConditionImmunityTrait(Conditions.Stun),
				new ConditionImmunityTrait(Conditions.Disarm), new ForcedMovementImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 13 * (CharacterCount * 2 - 1),
			Attack = 6,
			Traits = [new RetaliateTrait(3), ConditionImmunityTrait.PoisonImmunityTrait(), new ConditionImmunityTrait(Conditions.Stun),
				new ConditionImmunityTrait(Conditions.Disarm), new ForcedMovementImmunityTrait()]
		},
		new MonsterStats()
		{
			Health = 15 * (CharacterCount * 2 - 1),
			Attack = 6,
			Traits = [new RetaliateTrait(4), ConditionImmunityTrait.PoisonImmunityTrait(), new ConditionImmunityTrait(Conditions.Stun),
				new ConditionImmunityTrait(Conditions.Disarm), new ForcedMovementImmunityTrait()]
		},
	];

	public override string Name => "Land Leviathan";

	public override string AssetPath => "res://Content/Monsters/DeepTerror";

	public override int MaxStandeeCount => 1;

	public override IEnumerable<MonsterAbilityCardModel> Deck => BossAbilityCard.Deck;

	// IBossMonsterModel
	public IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, -1, targets: 2, range: 5)),

		new MonsterAbilityCardAbility(OtherAbility.Builder()
			.WithPerformAbility(async state =>
            {
                monster.SetMaxHealth(monster.MaxHealth + 2);
				await GDTask.CompletedTask;
            })
			.Build()),

		new MonsterAbilityCardAbility(HealAbility.Builder()
			.WithHealValue(2)
			.WithTarget(Target.Self)
			.Build())
	];

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterSummonAbility.Builder()
			.WithMonsterModel(_summonBlackImp ? ModelDB.Monster<BlackImp>() : ModelDB.Monster<ForestImp>())
			.WithMonsterType(_summonBlackImp ? ((CharacterCount >= 3) ? MonsterType.Elite : MonsterType.Normal) : ((CharacterCount >= 4) ? MonsterType.Elite : MonsterType.Normal))
			.WithGetValidHexes((state, hexes) =>
            {
                hexes = RangeHelper.GetHexesInRange(state.Performer.Hex, 1, true).Where(hex => hex.IsEmpty()).ToList();
				if (hexes.Count == 0)
                {
                    hexes = RangeHelper.GetHexesInRange(state.Performer.Hex, 2, true).Where(hex => hex.IsEmpty()).ToList();
                }
            })
			.WithOnAbilityEndedPerformed(async state =>
            {
                _summonBlackImp = !_summonBlackImp;
				await GDTask.CompletedTask;
            })
			.Build()),

		new MonsterAbilityCardAbility(GrantAbility.Builder()
            .WithGetAbilities(state =>
			[
				HealAbility.Builder().WithHealValue(1).WithTarget(Target.Self).Build()
			])
			.Build())
	];
}
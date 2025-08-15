// using Fractural.Tasks;
//
// public class AbilityCardActionState : ActionState
// {
// 	private readonly AbilityCardSideOld _abilityCardSide;
//
// 	public AbilityCardActionState(AbilityCardSideOld abilityCardSide)
// 		: base(abilityCardSide)
// 	{
// 		_abilityCardSide = abilityCardSide;
// 	}
//
// 	public override async GDTask ActiveAbility(ActiveAbilityState abilityState)
// 	{
// 		await base.ActiveAbility(abilityState);
//
// 		await AbilityCmd.PlayingActiveCard(_abilityCardSide.AbilityCard);
// 	}
// }


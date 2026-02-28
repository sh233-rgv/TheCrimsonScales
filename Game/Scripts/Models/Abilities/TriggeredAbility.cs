using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

/// <summary>
/// An <see cref="Ability{T}"/> that makes figures suffer damage.
/// </summary>
public class TriggeredAbility : ActiveAbility<TriggeredAbility.State>
{
	public class State : ActiveAbilityState
	{
	}

	public List<Ability> Abilities;
	public int Initiative;

	/// <summary>
	/// A builder extending <see cref="Ability{T}.AbstractBuilder{TBuilder, TAbility}"/> with setter methods
	/// for values defined in TargetedAbility. Enables inheritors of TargetedAbility to further extend the builder.
	/// </summary>
	/// <typeparam name="TBuilder"></typeparam> Any builder extending this AbstractBuilder.
	/// <typeparam name="TAbility"></typeparam> Any ability extending TargetedAbility.
	public new abstract class AbstractBuilder<TBuilder, TAbility> :
		ActiveAbility<State>.AbstractBuilder<TBuilder, TAbility>,
		AbstractBuilder<TBuilder, TAbility>.IAbilitiesStep,
		AbstractBuilder<TBuilder, TAbility>.IInitiativeStep
		where TBuilder : AbstractBuilder<TBuilder, TAbility>
		where TAbility : TriggeredAbility, new()
	{
		public interface IAbilitiesStep
		{
			IInitiativeStep WithAbilities(params Ability[] abilities);
		}

		public IInitiativeStep WithAbilities(params Ability[] abilities)
		{
			Obj.Abilities = abilities.ToList();
			return (TBuilder)this;
		}

		public interface IInitiativeStep
		{
			TBuilder WithInitiative(int initiative);
		}

		public TBuilder WithInitiative(int initiative)
		{
			Obj.Initiative = initiative;
			return (TBuilder)this;
		}
	}

	/// <summary>
	/// A concrete implementation of the AbstractBuilder. Required to actually use the builder,
	/// as abstract builders cannot be instantiated.
	/// </summary>
	public class TriggeredAbilityBuilder : AbstractBuilder<TriggeredAbilityBuilder, TriggeredAbility>
	{
		internal TriggeredAbilityBuilder() { }
	}

	/// <summary>
	/// A convenience method that returns an instance of AttackBuilder.
	/// </summary>
	/// <returns></returns>
	public static TriggeredAbilityBuilder.IAbilitiesStep Builder()
	{
		return new TriggeredAbilityBuilder();
	}

	public TriggeredAbility() { }

	protected override async GDTask Perform(State abilityState)
	{
		await AskConfirmAndActivate(abilityState);
	}

	protected override async GDTask Activate(State abilityState)
	{
		await base.Activate(abilityState);

		ScenarioEvents.NextActiveFigureEvent.Subscribe(abilityState, this,
			parameters => parameters.PreviousActiveFigure.Initiative.SortingInitiative < Initiative * 10000000 + 9999999 &&
			              parameters.NextActiveFigure.Initiative.SortingInitiative >= Initiative * 10000000 + 9999999,
			async parameters =>
			{
				await new ActionState(abilityState.ActionState, abilityState.Performer, Abilities).Perform();
				ScenarioEvents.NextActiveFigureEvent.Unsubscribe(abilityState, this);
			});
		await GDTask.CompletedTask;
	}
}
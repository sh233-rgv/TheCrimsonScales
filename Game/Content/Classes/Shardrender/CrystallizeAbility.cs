using System;
using System.Collections.Generic;
using Fractural.Tasks;

/// <summary>
/// An <see cref="ActiveAbility{T}"/> that has a number of uses before it is discarded/lost.
/// </summary>
public class CrystallizeAbility : ActiveAbility<CrystallizeAbility.State>
{
	public class State : ActiveAbilityState
	{

	}

	/// <summary>
	/// A builder extending <see cref="ActiveAbility{T}.AbstractBuilder{TBuilder, TAbility}"/> with setter methods
	/// for values defined in CrystallizeAbility. Enables inheritors of CrystallizeAbility to further extend the builder.
	/// </summary>
	/// <typeparam name="TBuilder"></typeparam> Any builder extending this AbstractBuilder.
	/// <typeparam name="TAbility"></typeparam> Any ability extending CrystallizeAbility.
	public new class AbstractBuilder<TBuilder, TAbility> : ActiveAbility<State>.AbstractBuilder<TBuilder, TAbility>
		where TBuilder : AbstractBuilder<TBuilder, TAbility>
		where TAbility : CrystallizeAbility, new()
	{
	}

	/// <summary>
	/// A concrete implementation of the AbstractBuilder. Required to actually use the builder,
	/// as abstract builders cannot be instantiated.
	/// </summary>
	public class CrystallizeBuilder : AbstractBuilder<CrystallizeBuilder, CrystallizeAbility>
	{
		internal CrystallizeBuilder() { }
	}

	public CrystallizeAbility() { }

	protected override async GDTask Perform(State abilityState)
	{
		await AskConfirmAndActivate(abilityState);
	}

	protected override async GDTask Activate(State abilityState)
	{
		await base.Activate(abilityState);

		ScenarioEvents.JustBeforeSufferDamageEvent.Subscribe(abilityState, this,
			parameters => parameters.Prevented);
	}

	protected override async GDTask Deactivate(State abilityState)
	{
		await base.Deactivate(abilityState);
	}

}
using System;
using Fractural.Tasks;

public abstract class TempestCardModel<TTop, TBottom> : AbilityCardModel<TTop, TBottom>
	where TTop : TempestCardSide
	where TBottom : TempestCardSide
{
	protected override string TexturePath => "res://Content/Classes/Tempest/Cards.jpg";
	protected override int ColumnCount => 6;
	protected override int RowCount => 5;
}

public abstract class TempestCardSide : AbilityCardSideModel
{
	public static OtherAbility.OtherBuilder GainSparkAbilityBuilder(int count = 1)
	{
		return OtherAbility.Builder()
			.WithPerformAbility(async state =>
			{
				await GainSparks(state, count);
				state.SetPerformed();
			});
	}

	public static async GDTask GainSparks(AbilityState state, int count)
	{
		if(state.Performer is Tempest tempest)
		{
			tempest.GainSparks(count);
		}

		await GDTask.CompletedTask;
	}

	public static async GDTask GainSpark(AbilityState state)
	{
		await GainSparks(state, 1);
	}

	public static void LoseSparks(Figure figure, int count = 1)
	{
		if(figure is Tempest tempest)
		{
			tempest.LoseSparks(count);
		}
	}

	public static bool HasXSparks(Figure figure, int x)
	{
		if(figure is Tempest tempest)
		{
			return tempest.HasXSparks(x);
		}

		return false;
	}

	protected static async GDTask<bool> LoseSparksConditionalAbilityCheck(Figure figure, int count, EffectInfoViewParameters effectInfoViewParameters)
	{
		bool lostSparks = false;
		await AbilityCmd.GenericChoice(figure,
		[
			ScenarioEvents.GenericChoice.Subscription.New(
				_ => HasXSparks(figure, count),
				async _ =>
				{
					LoseSparks(figure, count);
					lostSparks = true;
					await GDTask.CompletedTask;
				},
				EffectType.Selectable,
				effectButtonParameters: new TextEffectButton.Parameters($"{count}{Icons.HintText(Tempest.SparkIconPath)}"),
				effectInfoViewParameters: effectInfoViewParameters,
				canApplyMultipleTimesDuringSubscription: false)
		]);
		return lostSparks;
	}

	protected static ScenarioEvent<T>.Subscription LoseSparksSubscription<T>(int count, Func<T, GDTask> applyFunction,
		EffectInfoViewParameters effectInfoViewParameters)
		where T : ScenarioEvent.ParametersBaseWithAbilityState
	{
		return ScenarioEvent<T>.Subscription.New(
			parameters => HasXSparks(parameters.BaseAbilityState.Performer, count),
			async parameters =>
			{
				LoseSparks(parameters.BaseAbilityState.Performer, count);
				await applyFunction(parameters);
			},
			EffectType.Selectable,
			canApplyMultipleTimesDuringSubscription: false,
			effectButtonParameters: new TextEffectButton.Parameters($"{count}{Icons.HintText(Tempest.SparkIconPath)}"),
			effectInfoViewParameters: effectInfoViewParameters);
	}
}
using System.Collections.Generic;
using Fractural.Tasks;

public abstract class ArtificerCardModel<TTop, TBottom> : AbilityCardModel<TTop, TBottom>
	where TTop : ArtificerCardSide
	where TBottom : ArtificerCardSide
{
	protected override string TexturePath => "res://Content/Classes/Artificer/Cards.jpg";
	protected override int ColumnCount => 6;
	protected override int RowCount => 5;
}

public abstract class ArtificerCardSide : AbilityCardSideModel
{
	public AbilityCardAbility TimedTrack(List<UseSlot> useSlots)
	{
		return new AbilityCardAbility(UseSlotAbility.Builder()
			.WithOnActivate(async state =>
			{
				ScenarioEvents.FigureTurnEndedEvent.Subscribe(state, this,
					parameters => parameters.Figure == state.Performer && !parameters.Figure.TurnPerformedActionStates.Contains(state.ActionState),
					async parameters =>
					{
						await state.AdvanceUseSlot();
					});
				await GDTask.CompletedTask;
			})
			.WithOnDeactivate(async state =>
			{
				ScenarioEvents.FigureTurnEndedEvent.Unsubscribe(state, this);
				await GDTask.CompletedTask;
			})
			.WithUseSlots(useSlots)
			.WithMandatory(true)
			.Build());
	}

	public async GDTask GainScrapToken(AbilityState state)
	{
		if(state.Performer is Artificer artificer)
		{
			artificer.GainScrapToken();
		}

		await GDTask.CompletedTask;
	}

	public void LoseScrapTokens(Figure figure, int count = 1)
	{
		if(figure is Artificer artificer)
		{
			artificer.LoseScrapTokens(count);
		}
	}

	public bool HasXScrapTokens(Figure figure, int x)
	{
		if(figure is Artificer artificer)
		{
			return artificer.HasXScrapTokens(x);
		}

		return false;
	}

	public bool TryLoseScrapTokens(Figure figure, int count)
	{
		if(HasXScrapTokens(figure, count))
		{
			LoseScrapTokens(figure, count);
			return true;
		}

		return false;
	}
}
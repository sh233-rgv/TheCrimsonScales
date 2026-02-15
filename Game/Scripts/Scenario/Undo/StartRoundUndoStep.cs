public class StartRoundUndoStep(CardSelectionState cardSelectionState) : UndoStep
{
	public override void Undo(SavedScenario savedScenario)
	{
		cardSelectionState.Completed = false;

		// Currently the full card selection state is removed to make sure no cards are still selected that should not be
		savedScenario.CardSelectionStates.Remove(cardSelectionState);
	}
}
using System.Collections.Generic;
using System.Linq;

public class UndoManager
{
	private readonly List<UndoStep> _stack = new List<UndoStep>();

	public bool CanUndo => !GameController.Instance.ScenarioEnded && _stack.Count > 0;
	public bool CanUndoTurn => false;
	public bool CanUndoRound => false;

	private SavedScenario SavedScenario => GameController.Instance.SavedScenario;

	public void AddStep(UndoStep undoStep)
	{
		_stack.Add(undoStep);
	}

	public void Undo()
	{
		if(!CanUndo)
		{
			return;
		}

		while(CanUndo && Pop().Silent)
		{
		}

		//GameController.Instance.SavedCampaign.SetSavedScenario(SavedScenario);
		AppController.Instance.SceneLoader.RequestSceneChange(new GameSceneRequest(GameController.Instance.SavedCampaign, true));
	}

	public void UndoTurn()
	{
		if(!CanUndoTurn)
		{
			return;
		}
	}

	public void UndoRound()
	{
		if(!CanUndoRound)
		{
			return;
		}
	}

	private UndoStep Pop()
	{
		UndoStep lastStep = _stack.Last();
		lastStep.Undo(SavedScenario);
		_stack.RemoveLast();
		return lastStep;
	}
}
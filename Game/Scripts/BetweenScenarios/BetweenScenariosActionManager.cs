using Godot;

public partial class BetweenScenariosActionManager : Control
{
	[Export]
	private BetweenScenariosAction[] _actions;
	[Export]
	private BetweenScenariosAction _startActiveAction;

	public BetweenScenariosAction ActiveAction { get; private set; }

	public override void _Ready()
	{
		base._Ready();

		foreach(BetweenScenariosAction action in _actions)
		{
			action.Button.BetterButton.Pressed += () => BetterButtonOnPressed(action);
		}

		SetActive(_startActiveAction);
	}

	private void BetterButtonOnPressed(BetweenScenariosAction action)
	{
		SetActive(action);
	}

	public void SetActive(BetweenScenariosAction action)
	{
		if(action == ActiveAction)
		{
			return;
		}

		if(action?.Transitioning == true || ActiveAction?.Transitioning == true)
		{
			return;
		}

		ActiveAction?.SetActive(false);

		ActiveAction = action;
		ActiveAction?.SetActive(true);
	}
}
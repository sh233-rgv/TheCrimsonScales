using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public partial class CurrentActionViewManager : Control, IEventSubscriber
{
	[Export]
	private Control _infoViewParent;

	private readonly Stack<CurrentActionViewBase> _views = new Stack<CurrentActionViewBase>();

	public override void _Ready()
	{
		base._Ready();

		ScenarioEvents.AbilityCardSideStartedEvent.Subscribe(this,
			canApply: parameters => true,
			apply: async parameters =>
			{
				if(parameters.AbilityCardSide == parameters.AbilityCardSide.AbilityCard.BasicTop ||
				   parameters.AbilityCardSide == parameters.AbilityCardSide.AbilityCard.BasicBottom)
				{
					CreateActionView(new BasicAbilityCardSideCurrentActionView.Parameters(parameters.AbilityCardSide));
				}
				else
				{
					CreateActionView(new AbilityCardSideCurrentActionView.Parameters(parameters.AbilityCardSide));
				}

				await GDTask.CompletedTask;
			},
			EffectType.Visuals
		);

		ScenarioEvents.AbilityCardSideEndedEvent.Subscribe(this,
			canApply: parameters => true,
			apply: async parameters =>
			{
				TryRemoveActionView(parameters.AbilityCardSide);

				await GDTask.CompletedTask;
			},
			EffectType.Visuals
		);

		ScenarioEvents.ItemUseStartedEvent.Subscribe(this,
			canApply: parameters => true,
			apply: async parameters =>
			{
				CreateActionView(new ItemCurrentActionView.Parameters(parameters.Item));

				await GDTask.CompletedTask;
			},
			EffectType.Visuals
		);

		ScenarioEvents.ItemUseEndedEvent.Subscribe(this,
			canApply: parameters => true,
			apply: async parameters =>
			{
				TryRemoveActionView(parameters.Item);

				await GDTask.CompletedTask;
			},
			EffectType.Visuals
		);
	}

	public void CreateActionView(CurrentActionViewParameters parameters)
	{
		PackedScene buttonScene = ResourceLoader.Load<PackedScene>(parameters.ScenePath);
		CurrentActionViewBase currentActionView = buttonScene.Instantiate<CurrentActionViewBase>();
		_infoViewParent.AddChild(currentActionView);

		currentActionView.Init(parameters);
		_views.Push(currentActionView);
	}

	public void TryRemoveActionView(object source)
	{
		if(!_views.TryPeek(out CurrentActionViewBase view) || view.Source != source)
		{
			Log.Error("Trying to remove an action view that is not on top of the stack.");
			return;
		}

		view.Destroy();

		_views.Pop();
	}
}
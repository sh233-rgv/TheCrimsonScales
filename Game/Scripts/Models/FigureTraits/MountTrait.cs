using Fractural.Tasks;
using System;
using Godot;
using GTweens.Easings;
using GTweensGodot.Extensions;

public class MountTrait(Func<Figure, Figure, GDTask> onMounted = null, Func<Figure, Figure, GDTask> onDismounted = null) : FigureTrait
{
	private const string MountedAnchorName = "MountedAnchor";

	private bool _mounted = false;

	public override async GDTask Activate(Figure figure)
	{
		await base.Activate(figure);

		Node2D mountedAnchor = new Node2D();
		mountedAnchor.SetName(MountedAnchorName);
		mountedAnchor.SetPosition(new Vector2(70f, -70f));
		mountedAnchor.SetScale(0.7f * Vector2.One);
		figure.AddChild(mountedAnchor);

		Figure characterOwner = ((Summon)figure).CharacterOwner;

		// Allow entering the same hex to mount
		ScenarioCheckEvents.CanEnterHexWithFigureCheckEvent.Subscribe(figure, this,
			parameters => parameters.OtherFigure == figure && parameters.Figure == characterOwner,
			parameters =>
			{
				parameters.SetCanEnter();
			}
		);

		// Control the mount
		ScenarioCheckEvents.IsSummonControlledCheckEvent.Subscribe(figure, this,
			parameters => parameters.Summon == figure && _mounted,
			parameters =>
			{
				parameters.SetIsControlled();
			}
		);

		// Follow the mount when it moves or being forcefully moved
		ScenarioEvents.MoveTogetherCheckEvent.Subscribe(figure, this,
			parameters => parameters.Performer == figure && _mounted,
			parameters =>
			{
				parameters.SetOtherFigure(characterOwner);
				parameters.SetTriggerHexEffects(false);

				return GDTask.CompletedTask;
			}
		);

		// Mounted summon goes just before the character
		ScenarioCheckEvents.InitiativeCheckEvent.Subscribe(figure, this,
			parameters => parameters.Figure == figure && _mounted,
			parameters => parameters.SetSortingInitiative(characterOwner.Initiative.SortingInitiative - 1)
		);

		// Trigger mounted effect
		ScenarioEvents.FigureEnteredHexEvent.Subscribe(figure, this,
			parameters => parameters.Figure == characterOwner,
			async parameters =>
			{
				if(!_mounted && parameters.Hex == figure.Hex)
				{
					_mounted = true;
					figure.UpdateInitiative();

					parameters.Figure.Reparent(figure.GetNode<Node2D>(MountedAnchorName));
					parameters.Figure.TweenScale(1f, 0.3f).SetEasing(Easing.InOutBack).PlayFastForwardable();
					parameters.Figure.TweenPosition(Vector2.Zero, 0.3f).SetEasing(Easing.OutBack).PlayFastForwardable();
					await GDTask.Delay(0.3f);

					if(onMounted != null)
					{
						await onMounted(characterOwner, figure);
					}
				}
			}
		);

		ScenarioEvents.FigureExitingHexEvent.Subscribe(figure, this,
			parameters => parameters.Figure == characterOwner && _mounted && parameters.Hex == figure.Hex,
			async parameters =>
			{
				// Owner is exiting the hex the mount is on, so they are no longer mounted
				_mounted = false;
				figure.UpdateInitiative();

				await Dismount(figure);
			}
		);

		// Returning mounted status for other effects and abilities
		ScenarioCheckEvents.IsMountedCheckEvent.Subscribe(figure, this,
			parameters => parameters.Figure == characterOwner,
			parameters =>
			{
				if(_mounted)
				{
					parameters.SetMount(figure);
				}
			}
		);

		// Mounted summon can open doors
		ScenarioCheckEvents.CanOpenDoorsCheckEvent.Subscribe(figure, this,
			parameters => parameters.Figure == figure,
			parameters =>
			{
				if(_mounted)
				{
					parameters.SetCanOpenDoors();
				}
			}
		);
	}

	public override async GDTask Deactivate(Figure figure)
	{
		await base.Deactivate(figure);

		_mounted = false;

		ScenarioCheckEvents.CanEnterHexWithFigureCheckEvent.Unsubscribe(figure, this);
		ScenarioCheckEvents.IsSummonControlledCheckEvent.Unsubscribe(figure, this);
		ScenarioEvents.MoveTogetherCheckEvent.Unsubscribe(figure, this);
		ScenarioCheckEvents.InitiativeCheckEvent.Unsubscribe(figure, this);
		ScenarioEvents.FigureEnteredHexEvent.Unsubscribe(figure, this);
		ScenarioCheckEvents.IsMountedCheckEvent.Unsubscribe(figure, this);
		ScenarioCheckEvents.CanOpenDoorsCheckEvent.Unsubscribe(figure, this);

		await Dismount(figure);
	}

	private async GDTask Dismount(Figure figure)
	{
		Figure characterOwner = ((Summon)figure).CharacterOwner;

		if(onDismounted != null)
		{
			await onDismounted.Invoke(characterOwner, figure);
		}

		characterOwner.Reparent(GameController.Instance.Map);
		characterOwner.TweenScale(1f, 0.3f).SetEasing(Easing.OutBack).PlayFastForwardable();
		characterOwner.TweenGlobalPosition(characterOwner.Hex.GlobalPosition, 0.2f)
			.SetEasing(Easing.InBack).PlayFastForwardable();
		await GDTask.Delay(0.3f);
	}
}
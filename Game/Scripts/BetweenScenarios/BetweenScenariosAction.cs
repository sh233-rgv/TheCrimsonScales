using Godot;
using GTweens.Builders;
using GTweensGodot.Extensions;

public abstract partial class BetweenScenariosAction : Control
{
	[Export]
	public BetweenScenariosActionButton Button { get; private set; }

	private bool _transitioning;

	public bool Active { get; private set; }

	public bool Transitioning => _transitioning || CustomTransitioning;

	protected abstract bool SelectCharacter { get; }
	protected virtual bool CustomTransitioning => false;

	public override void _Ready()
	{
		base._Ready();

		SetVisible(false);
	}

	public void SetActive(bool active)
	{
		Active = active;

		Button.SetSelected(Active);

		_transitioning = true;
		GTweenSequenceBuilder sequenceBuilder = GTweenSequenceBuilder.New();
		if(Active)
		{
			AnimateIn(sequenceBuilder);
		}
		else
		{
			AnimateOut(sequenceBuilder);
		}

		sequenceBuilder.AppendCallback(() => _transitioning = false);

		sequenceBuilder.AppendCallback(Active ? AfterAnimateIn : AfterAnimateOut);

		sequenceBuilder.Build().Play();

		BetweenScenariosController.Instance.CharacterPortraitManager.SetSelectionMode(SelectCharacter);
	}

	protected virtual void AnimateIn(GTweenSequenceBuilder sequenceBuilder)
	{
		SetVisible(true);
	}

	protected virtual void AfterAnimateIn()
	{
	}

	protected virtual void AnimateOut(GTweenSequenceBuilder sequenceBuilder)
	{
	}

	protected virtual void AfterAnimateOut()
	{
		SetVisible(false);
	}
}
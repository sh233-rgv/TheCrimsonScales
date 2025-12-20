using System;
using Fractural.Tasks;
using Godot;

public partial class Ruinmaw : Character, IHasEmpower
{
	public bool Sated { get; private set; }
	private bool _satedAppliedThisTurn;
	[Export]
	private SatedIndicator _satedIndicator;
	public int RemainingEmpowerCount { get; set; } = 12;
	public event Func<Ruinmaw, GDTask> SateEvent;

	public static EmpowerRuinmaw EmpowerRuinmaw { get; } = ModelDB.Condition<EmpowerRuinmaw>();

	public override void Spawn(SavedCharacter savedCharacter, int index)
	{
		base.Spawn(savedCharacter, index);
		_satedIndicator.Hide();
	}

	public override async GDTask OnScenarioSetupCompleted()
	{
		await base.OnScenarioSetupCompleted();

		object subscriber = new();

		ScenarioEvents.InflictConditionEvent.Subscribe(this, subscriber,
			canApply: parameters => parameters.Condition is EmpowerRuinmaw,
			apply: async parameters =>
			{
				((EmpowerRuinmaw)parameters.Condition).SetEmpowerOwner(this);
				await GDTask.CompletedTask;
			}
		);
	}

	public void Sate()
	{
		if(TakingTurn)
		{
			_satedAppliedThisTurn = true;
		}

		if(Sated)
		{
			return;
		}

		_satedIndicator.ShowAnimated();
		object subscriber = new object();
		ScenarioEvents.FigureTurnEndedEvent.Subscribe(this, subscriber,
			canApplyParameters => canApplyParameters.Figure == this,
			async applyParameters =>
			{
				if(_satedAppliedThisTurn)
				{
					_satedAppliedThisTurn = false;
				}
				else
				{
					_satedIndicator.HideAnimated();

					ScenarioEvents.FigureTurnEndedEvent.Unsubscribe(this, subscriber);
					ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Unsubscribe(this, subscriber);
				}

				await GDTask.CompletedTask;
			});
		ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Subscribe(this, subscriber,
			parameters => parameters.Figure == this,
			parameters =>
			{
				parameters.Add(new FigureInfoTextExtraEffect.Parameters($"{Icons.Inline("res://Content/Classes/Ruinmaw/RuinmawSated.png")}"));
			}
		);

		Sated = true;
		SateEvent?.Invoke(this);
	}

	public AMDCardModel CreateEmpower()
	{
		return ModelDB.AMDCard<RuinmawEmpowerAMDCard>();
	}
}
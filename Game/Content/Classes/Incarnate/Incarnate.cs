using System;
using Fractural.Tasks;
using Godot;

public partial class Incarnate : Character, IHasEmpower, IHasEnfeeble
{
	public enum IncarnateSpirit
	{
		Ritualist,
		Conqueror,
		Reaver,
	}

	public static EmpowerIncarnate Empower { get; } = ModelDB.Condition<EmpowerIncarnate>();
	public static EnfeebleIncarnate Enfeeble { get; } = ModelDB.Condition<EnfeebleIncarnate>();

	[Export]
	private IncarnateSpiritIndicator _spiritIndicator;

	private bool _satedAppliedThisTurn;

	public IncarnateSpirit Spirit { get; private set; }
	public int RemainingEmpowerCount { get; set; } = 10;
	public int RemainingEnfeebleCount { get; set; } = 10;

	public override async GDTask Spawn(SavedCharacter savedCharacter, int index)
	{
		await base.Spawn(savedCharacter, index);

		_spiritIndicator.Hide();
	}

	public async GDTask SwitchSpirit(IncarnateSpirit spirit)
	{
		if(Spirit == spirit)
		{
			return;
		}

		_spiritIndicator.ShowAnimated();
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
					_spiritIndicator.HideAnimated();

					ScenarioEvents.FigureTurnEndedEvent.Unsubscribe(this, subscriber);
					ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Unsubscribe(this, subscriber);
				}

				await GDTask.CompletedTask;
			});
		ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Subscribe(this, subscriber,
			parameters => parameters.Figure == this,
			parameters =>
			{
				parameters.Add(new InfoTextExtraEffect.Parameters(textParameters =>
					$"{Icons.Inline("res://Content/Classes/Ruinmaw/RuinmawSated.png")}"));
			}
		);

		Spirit = true;
		if(SateEvent != null)
		{
			await SateEvent(this);
		}

		await GDTask.CompletedTask;
	}

	public AMDCardModel CreateEmpower()
	{
		return ModelDB.AMDCard<IncarnateEmpowerAMDCard>();
	}

	public AMDCardModel CreateEnfeeble()
	{
		return ModelDB.AMDCard<incarnate>();
	}
}
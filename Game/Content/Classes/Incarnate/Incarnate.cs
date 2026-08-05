using Fractural.Tasks;

public partial class Incarnate : Character, IHasEmpower, IHasEnfeeble
{
	public const string RitualistIconPath = "res://Content/Classes/Incarnate/Ritualist.svg";
	public const string ConquerorIconPath = "res://Content/Classes/Incarnate/Conqueror.svg";
	public const string ReaverIconPath = "res://Content/Classes/Incarnate/Reaver.svg";

	public static EmpowerIncarnate Empower { get; } = ModelDB.Condition<EmpowerIncarnate>();
	public static EnfeebleIncarnate Enfeeble { get; } = ModelDB.Condition<EnfeebleIncarnate>();

	//[Export]
	//private IncarnateSpiritIndicator _spiritIndicator;

	private bool _satedAppliedThisTurn;

	public IncarnateSpirit Spirit { get; private set; }
	public int RemainingEmpowerCount { get; set; } = 10;
	public int RemainingEnfeebleCount { get; set; } = 10;

	public async GDTask SwitchSpirit(IncarnateSpirit spirit)
	{
		if(Spirit == spirit)
		{
			return;
		}

		object subscriber = new object();
		ScenarioEvents.FigureTurnEndedEvent.Subscribe(this, subscriber,
			canApplyParameters => canApplyParameters.Figure == this,
			async _ =>
			{
				if(_satedAppliedThisTurn)
				{
					_satedAppliedThisTurn = false;
				}
				else
				{

					ScenarioEvents.FigureTurnEndedEvent.Unsubscribe(this, subscriber);
					ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Unsubscribe(this, subscriber);
				}

				await GDTask.CompletedTask;
			});
		ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Subscribe(this, subscriber,
			parameters => parameters.Figure == this,
			parameters =>
			{
				parameters.Add(new InfoTextExtraEffect.Parameters(_ =>
					$"{Icons.Inline("res://Content/Classes/Ruinmaw/RuinmawSated.png")}"));
			}
		);

		Spirit = spirit;

		await GDTask.CompletedTask;
	}

	public AMDCardModel CreateEmpower()
	{
		return ModelDB.AMDCard<IncarnateEmpowerAMDCard>();
	}

	public AMDCardModel CreateEnfeeble()
	{
		return ModelDB.AMDCard<IncarnateEnfeebleAMDCard>();
	}
}
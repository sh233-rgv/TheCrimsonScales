using System.Linq;
using Fractural.Tasks;

public class GravityInversionPads : CSAddonQA
{
	public override string Name => "Gravity Inversion Pads";
	public override int ItemNumber => 4;
	public override int ShopCount => 1;
	public override int Cost => 40;
	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Spend;

	protected override int AtlasIndex => 8;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringTurn(
			canApply: character => character == Owner,
			apply: async character =>
			{
				await Use(async user =>
				{
					Trap trap = (await AbilityCmd.CreateTraps(0, user, range: 3)).FirstOrDefault();

					if(trap == null)
					{
						return;
					}
					//TODO: Place character token on trap

					ScenarioEvents.TrapTriggeredEvent.Subscribe(this, trap,
						trapParameters => trap == trapParameters.Trap && trapParameters.Figure.AlliedWith(user, true),
						async trapParameters =>
						{
							ScenarioCheckEvents.FlyingCheckEvent.Subscribe(this, trap,
								parameters => parameters.Figure == trapParameters.Figure,
								parameters => parameters.SetFlying(true));

							ScenarioEvents.RoundEndedEvent.Subscribe(this, trap,
								_ => true,
								async _ =>
								{
									ScenarioCheckEvents.FlyingCheckEvent.Unsubscribe(this, trap);
									await GDTask.CompletedTask;
								});
							await GDTask.CompletedTask;
						});

					ScenarioEvents.FigureEnteredHexEvent.Subscribe(this, trap,
						enterHexParameters => enterHexParameters.Figure.EnemiesWith(user) && trap.Hex == enterHexParameters.Hex,
						async enterHexParameters =>
						{
							ScenarioCheckEvents.FlyingCheckEvent.Subscribe(this, trap,
								parameters => parameters.Figure == enterHexParameters.Figure,
								parameters => parameters.SetFlying(false), order: 100);

							ScenarioEvents.RoundEndedEvent.Subscribe(this, trap,
								_ => true,
								async _ =>
								{
									ScenarioCheckEvents.FlyingCheckEvent.Unsubscribe(this, trap);
									await GDTask.CompletedTask;
								});
							await trap.Trigger(enterHexParameters.PotentialAbilityState, enterHexParameters.Figure);
						});

					await GDTask.CompletedTask;
				});
			}
		);
	}
}
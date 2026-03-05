using Fractural.Tasks;

public class SpiderClimbGloves : CSAddonQA
{
	public override string Name => "Spider Climb Gloves";
	public override int ItemNumber => 3;
	public override int ShopCount => 2;
	public override int Cost => 30;
	public override ItemType ItemType => ItemType.OneHand;
	public override ItemUseType ItemUseType => ItemUseType.Always;

	protected override int AtlasIndex => 6;

	private object _subscriber;

	public override void Init(Character owner)
	{
		_subscriber = new object();

		base.Init(owner);
	}

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeAbilityStarted<MoveAbility.State>(
			canApply: state => state.Performer == Owner,
			apply: async state =>
			{
				await Use(async user =>
				{
					ScenarioCheckEvents.CanEnterObstacleCheckEvent.Subscribe(this, _subscriber,
						parameters =>
							parameters.Figure == state.Performer,
						parameters =>
						{
							parameters.SetCanEnter();
						}
					);

					ScenarioEvents.AbilityEndedEvent.Subscribe(this, _subscriber,
						parameters => parameters.AbilityState == state,
						async _ =>
						{
							ScenarioCheckEvents.CanEnterObstacleCheckEvent.Unsubscribe(this, _subscriber);
							await GDTask.CompletedTask;
						});

					await GDTask.CompletedTask;
				});
			}
		);
	}
}
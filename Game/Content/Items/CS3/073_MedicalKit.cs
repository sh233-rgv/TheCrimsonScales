using Fractural.Tasks;

public class MedicalKit : CS3Item
{
	public override string Name => "Medical Kit";
	public override int ItemNumber => 73;
	public override int ShopCount => 1;
	public override int Cost => 30;
	public override ItemType ItemType => ItemType.OneHand;
	public override ItemUseType ItemUseType => ItemUseType.Always;

	protected override int AtlasIndex => 0;

	private object _subscriber;

	public override void Init(Character owner)
	{
		_subscriber = new object();

		base.Init(owner);
	}

	protected override void Subscribe()
	{
		base.Subscribe();

		ScenarioEvents.AbilityStartedEvent.Subscribe(this, _subscriber,
			parameters =>
				parameters.Performer == Owner &&
				parameters.AbilityState is AttackAbility.State attackAbilityState &&
				attackAbilityState.ActionState.ActionSource is AbilityCardSide abilityCardSide &&
				abilityCardSide.AbilityCardSideType == AbilityCardSideType.BasicTop,
			async parameters =>
			{
				await Use(async user =>
				{
					parameters.AbilityState.SetBlocked();
					await HealAbility.Builder().WithHealValue(2).WithRange(1).Build().Perform(parameters.AbilityState.ActionState);
					await GDTask.CompletedTask;
				});
			}, EffectType.Selectable
		);
	}

	protected override void Unsubscribe()
	{
		base.Unsubscribe();

		ScenarioEvents.AbilityStartedEvent.Unsubscribe(this, _subscriber);
	}
}
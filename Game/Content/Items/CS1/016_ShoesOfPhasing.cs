using System.Linq;
using Fractural.Tasks;

public class ShoesOfPhasing : CS1Item
{
	public override string Name => "Shoes Of Phasing";
	public override int ItemNumber => 16;
	public override int ShopCount => 1;
	public override int Cost => 20;
	public override ItemType ItemType => ItemType.Feet;
	public override ItemUseType ItemUseType => ItemUseType.Always;
	private object _subscriber;

	protected override int AtlasIndex => 28;
	
	public override void Init(Character owner)
	{
		_subscriber = new object();

		base.Init(owner);
	}

	protected override void Subscribe()
	{
		base.Subscribe();

		ScenarioCheckEvents.CanPassEnemyCheckEvent.Subscribe(this, _subscriber,
			canApplyParameters => canApplyParameters.AbilityState.Performer == Owner,
			async applyParameters =>
			{
				await Use(async user =>
				{
					applyParameters.SetCanPass();

					await GDTask.CompletedTask;
				});
			});
	}

	protected override void Unsubscribe()
	{
		base.Unsubscribe();
		
		ScenarioCheckEvents.CanPassEnemyCheckEvent.Unsubscribe(this, _subscriber);
	}
}
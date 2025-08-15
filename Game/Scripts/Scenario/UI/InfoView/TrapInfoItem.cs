using Godot;

public partial class TrapInfoItem : InfoItem<TrapInfoItem.Parameters>
{
	public class Parameters(Trap hexObject) : InfoItemParameters<Trap>(hexObject)
	{
		public override string ScenePath => "res://Scenes/Scenario/UI/InfoView/TrapInfoItem.tscn";
	}

	[Export]
	private Control _tileParent;

	public override void Init(Parameters parameters)
	{
		base.Init(parameters);

		Trap trap = parameters.HexObject;

		PackedScene trapScene = ResourceLoader.Load<PackedScene>(trap.SceneFilePath);
		Trap trapClone = trapScene.Instantiate<Trap>();
		_tileParent.AddChild(trapClone);
		trapClone.ScaledDamage = trap.ScaledDamage;
		trapClone.CustomDamage = trap.CustomDamage;
		trapClone.ConditionModels = trap.ConditionModels;
		trapClone.UpdateVisuals();
	}
}
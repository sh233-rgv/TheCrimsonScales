using Godot;

public abstract class ScenarioChain : AbstractModel<ScenarioChain>
{
	public virtual ScenarioChain BaseScenarioChain => this;

	public abstract Color Color { get; }
}
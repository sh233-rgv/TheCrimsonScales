using Godot;

[GlobalClass, Tool]
public partial class ScenarioModelResource : Resource
{
	[Export, ModelId(typeof(ScenarioModel))]
	public string ModelIdString { get; private set; } = ModelId.None.ToString();

	public ScenarioModel Model => ModelDB.GetById<ScenarioModel>(new ModelId(ModelIdString));
}
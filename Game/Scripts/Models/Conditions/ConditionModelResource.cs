using Godot;

[GlobalClass, Tool]
public partial class ConditionModelResource : Resource
{
	[Export, ModelId(typeof(ConditionModel))]
	public string ModelIdString { get; set; } = ModelId.None.ToString();

	public ConditionModel Model => ModelDB.GetById<ConditionModel>(new ModelId(ModelIdString));
}
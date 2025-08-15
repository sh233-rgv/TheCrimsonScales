using System;

public class ModelIdAttribute : Attribute
{
	public Type ModelType { get; }

	public ModelIdAttribute(Type modelType)
	{
		ModelType = modelType;
	}
}
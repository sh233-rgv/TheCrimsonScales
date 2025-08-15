#if TOOLS
using System;
using System.Reflection;
using Godot;

public partial class ModelIdDropdownInspectorPlugin : EditorInspectorPlugin
{
	public override bool _CanHandle(GodotObject @object)
	{
		// We support all objects in this example.
		return true;
	}

	public override bool _ParseProperty(GodotObject @object, Variant.Type type,
		string name, PropertyHint hintType, string hintString,
		PropertyUsageFlags usageFlags, bool wide)
	{
		if(type == Variant.Type.String)
		{
			Type objectType = @object.GetType();

			PropertyInfo propertyInfo = objectType.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo fieldInfo = objectType.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			ModelIdAttribute modelIdAttribute = propertyInfo?.GetCustomAttribute<ModelIdAttribute>() ?? fieldInfo?.GetCustomAttribute<ModelIdAttribute>();
			if(modelIdAttribute != null)
			{
				Type modelType = modelIdAttribute.ModelType;

				ModelIdEditor modelIdEditor = new ModelIdEditor();
				modelIdEditor.SetType(modelType);
				AddPropertyEditor(name, modelIdEditor);
				return true;
			}
		}

		return false;
	}
}
#endif
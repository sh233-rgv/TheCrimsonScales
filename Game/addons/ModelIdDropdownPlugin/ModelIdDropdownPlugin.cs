#if TOOLS
using Godot;

[Tool]
public partial class ModelIdDropdownPlugin : EditorPlugin
{
	private ModelIdDropdownInspectorPlugin _plugin;

	public override void _EnterTree()
	{
		GD.Print("Activated Model Id plugin.");

		_plugin = new ModelIdDropdownInspectorPlugin();
		AddInspectorPlugin(_plugin);
	}

	public override void _ExitTree()
	{
		GD.Print("Deactivated Model Id plugin.");

		RemoveInspectorPlugin(_plugin);
	}
}
#endif
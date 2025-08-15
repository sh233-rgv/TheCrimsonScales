using System;
using Fractural.Tasks;
using Godot;

public class ItemUseSlot
{
	public Vector2 NormalizedPosition { get; private set; }

	public Func<ItemModel, GDTask> OnExit { get; private set; }

	public ItemUseSlot(Vector2 normalizedPosition, Func<ItemModel, GDTask> onExit = null)
	{
		NormalizedPosition = normalizedPosition;
		OnExit = onExit;

		Log.Write("");
	}
}
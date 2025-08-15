using System;
using Fractural.Tasks;
using Godot;

public class UseSlot
{
	public Vector2? NormalizedPosition { get; private set; }

	public Func<AbilityState, GDTask> OnExit { get; private set; }

	public UseSlot(Vector2? normalizedPosition, Func<AbilityState, GDTask> onExit = null)
	{
		NormalizedPosition = normalizedPosition;
		OnExit = onExit;
	}
}
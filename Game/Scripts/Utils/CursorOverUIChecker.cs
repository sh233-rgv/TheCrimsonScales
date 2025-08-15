using Godot;

public partial class CursorOverUIChecker : Control
{
	public bool CursorOverUI { get; private set; }

	public override void _Ready()
	{
		base._Ready();

		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
	}

	public override void _ExitTree()
	{
		MouseEntered -= OnMouseEntered;
		MouseExited -= OnMouseExited;
	}

	private void OnMouseEntered()
	{
		CursorOverUI = false;
	}

	private void OnMouseExited()
	{
		CursorOverUI = true;
	}
}
using System;
using Godot;

public partial class WorldButton : Node2D
{
	[Export]
	private bool _canDrag;
	[Export]
	private CollisionObject2D _collisionObject2D;

	private bool _pressed;

	public event Action PressedEvent;
	public event Action<Vector2, Vector2> DraggedEvent;
	public event Action<Vector2> DrageEndEvent;

	public override void _EnterTree()
	{
		base._EnterTree();

		_collisionObject2D.InputEvent += OnInputEvent;
		_collisionObject2D.MouseEntered += OnMouseEntered;
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		_collisionObject2D.InputEvent -= OnInputEvent;
		_collisionObject2D.MouseEntered -= OnMouseEntered;
	}

	public void SetCanDrag(bool canDrag)
	{
		_canDrag = canDrag;
	}

	public void OnDrag(Vector2 previousPosition, Vector2 currentPosition)
	{
		DraggedEvent?.Invoke(previousPosition, currentPosition);
	}

	public void OnDragEnd(Vector2 position)
	{
		DrageEndEvent?.Invoke(position);
	}

	private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
	{
		if(!AppController.Instance.InputController.CanClick)
		{
			return;
		}

		if(@event is InputEventMouseButton button)
		{
			if(button.ButtonIndex == MouseButton.Left)
			{
				if(button.Pressed)
				{
					_pressed = true;

					if(_canDrag)
					{
						AppController.Instance.InputController.SetDraggingObject(this);
					}
				}
				else
				{
					if(_pressed)
					{
						PressedEvent?.Invoke();
					}
				}
			}
		}
	}

	private void OnMouseEntered()
	{
		_pressed = false;
	}
}
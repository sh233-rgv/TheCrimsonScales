using System;
using Godot;

public abstract partial class PopupBase : Control
{
	public abstract Type RequestType { get; }

	public event Action<PopupRequest> PopupOpenEvent;
	public event Action<PopupRequest> PopupClosedEvent;

	public virtual void Open(PopupRequest request)
	{
	}

	public virtual void Close()
	{
	}

	protected void FirePopupOpenEvent(PopupRequest request)
	{
		PopupOpenEvent?.Invoke(request);
	}

	protected void FirePopupClosedEvent(PopupRequest request)
	{
		PopupClosedEvent?.Invoke(request);
	}
}
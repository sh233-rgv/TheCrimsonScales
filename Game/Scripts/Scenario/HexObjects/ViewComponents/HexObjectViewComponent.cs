using Godot;

public abstract partial class HexObjectViewComponent : Node2D
{
	public HexObject HexObject { get; private set; }

	//public Hex Hex => HexObject.Hex;

	public virtual void Init(HexObject hexObject)
	{
		HexObject = hexObject;

		HexObject.HexesChangedEvent += OnHexesChanged;
		OnHexesChanged(HexObject);
	}

	public virtual void OnHexesChanged(HexObject hexObject)
	{
	}
}
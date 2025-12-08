using System;

public class AMDCardWithOwner : AMDCard
{
	public Figure OriginalOwner;

	public new event Action<AMDCard, Figure> DrawnEvent;
	public override void Drawn()
	{
		DrawnEvent?.Invoke(this, OriginalOwner);
	}

	public AMDCardWithOwner(AMDCardModel model, AMDCardOwner owner, Figure originalOwner) : base(model, owner)
	{
		OriginalOwner = originalOwner;
	}
}
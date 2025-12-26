public abstract partial class HexObjectEffectView<T> : HexObjectEffectViewBase
	where T : HexObjectEffectViewParameters
{
	public sealed override void Init(HexObjectEffectViewParameters parameters)
	{
		base.Init(parameters);

		Init((T)parameters);
	}

	public virtual void Init(T parameters)
	{
	}
}
public abstract partial class EffectButton<T> : EffectButtonBase
	where T : EffectButtonParameters
{
	protected sealed override void Init(EffectButtonParameters parameters)
	{
		Init((T)parameters);
	}

	protected virtual void Init(T parameters)
	{
	}
}
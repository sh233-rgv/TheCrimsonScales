public partial class FigureInfoExtraEffect<T> : FigureInfoExtraEffectBase
	where T : FigureInfoExtraEffectParameters
{
	public sealed override void Init(FigureInfoExtraEffectParameters parameters)
	{
		Init((T)parameters);
	}

	protected virtual void Init(T parameters)
	{
	}
}
using Godot;

public abstract class EnhancementModel<TState> : EnhancementModel
	where TState : AbilityState
{
	public override void Enhance(AbilityState abilityState)
	{
		Enhance((TState)abilityState);
	}

	protected abstract void Enhance(TState state);
}

public abstract class EnhancementModel : AbstractModel
{
	protected abstract string TexturePath { get; }
	public abstract int BaseCost { get; }

	public Texture2D GetTexture()
	{
		return ResourceLoader.Load<Texture2D>(TexturePath);
	}

	public abstract void Enhance(AbilityState abilityState);
}
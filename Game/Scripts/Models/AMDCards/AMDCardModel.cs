using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public abstract class AMDCardModel : AbstractModel<AMDCardModel>
{
	protected abstract string TexturePath { get; }
	protected abstract int AtlasIndex { get; }
	protected abstract int ColumnCount { get; }
	protected abstract int RowCount { get; }

	public virtual AMDCardType Type => AMDCardType.Value;

	public virtual bool Reshuffles => false;
	public virtual bool RemoveAfterDraw => false;

	public virtual bool GetRolling(AttackAbility.State state) => false;

	public virtual int? GetValue(AttackAbility.State state) => null;

	public virtual int? Pierce => null;
	public virtual int? Push => null;
	public virtual int? Pull => null;
	public virtual int? Swing => null;

	public virtual List<Element> Elements => [];
	public virtual List<ConditionModel> GetConditionModels(AttackAbility.State state) => [];
	public virtual List<Ability> GetAbilities(AttackAbility.State state) => [];

	public virtual Func<AttackAbility.State, GDTask> GetExtraEffects(AttackAbility.State state) => null;

	public Texture2D GetTexture()
	{
		return AtlasTextureHelper.CreateAtlasTexture(
			AtlasIndex, ColumnCount, RowCount,
			ResourceLoader.Load<Texture2D>(TexturePath));
	}
}
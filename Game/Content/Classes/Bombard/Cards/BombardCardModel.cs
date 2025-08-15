public abstract class BombardCardModel<TTop, TBottom> : AtlasAbilityCardModel<TTop, TBottom>
	where TTop : BombardCardSide, new()
	where TBottom : BombardCardSide, new()
{
	protected override string TexturePath => "res://Content/Classes/Bombard/Cards.jpg";
	protected override int ColumnCount => 6;
	protected override int RowCount => 5;
}

public abstract class BombardCardSide : AbilityCardSide
{
}
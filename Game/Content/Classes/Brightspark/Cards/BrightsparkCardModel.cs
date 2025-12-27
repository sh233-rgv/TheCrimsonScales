public abstract class BrightsparkCardModel<TTop, TBottom> : AtlasAbilityCardModel<TTop, TBottom>
	where TTop : BrightsparkCardSide, new()
	where TBottom : BrightsparkCardSide, new()
{
	protected override string TexturePath => "res://Content/Classes/Brightspark/Cards.jpg";
	protected override int ColumnCount => 8;
	protected override int RowCount => 4;
}

public abstract class BrightsparkCardSide : AbilityCardSide
{
	
}
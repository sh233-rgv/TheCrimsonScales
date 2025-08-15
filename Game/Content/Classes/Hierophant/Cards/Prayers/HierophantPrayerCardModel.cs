public abstract class HierophantPrayerCardModel<TTop, TBottom> : AtlasAbilityCardModel<TTop, TBottom>
	where TTop : HierophantPrayerCardSide, new()
	where TBottom : HierophantPrayerCardSide, new()
{
	protected override string TexturePath => "res://Content/Classes/Hierophant/PrayerCards.jpg";
	protected override int ColumnCount => 3;
	protected override int RowCount => 3;

	public override int Level => 1;
	public override int Initiative => 50;
}

public abstract class HierophantPrayerCardSide : AbilityCardSide
{
}
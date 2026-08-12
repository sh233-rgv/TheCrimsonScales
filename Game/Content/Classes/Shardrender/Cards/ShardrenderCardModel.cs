using Fractural.Tasks;
using Godot;

public abstract class ShardrenderCardModel<TTop, TBottom> : AbilityCardModel<TTop, TBottom>
	where TTop : ShardrenderCardSide
	where TBottom : ShardrenderCardSide
{
	protected override string TexturePath => "res://Content/Classes/Shardrender/Cards.jpg";
	protected override int ColumnCount => 8;
	protected override int RowCount => 4;
}

public abstract class ShardrenderCardSide : AbilityCardSideModel
{

}
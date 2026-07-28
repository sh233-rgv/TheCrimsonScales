using System;
using Fractural.Tasks;
using Godot;

public abstract class ThornreaperCardModel<TTop, TBottom> : AbilityCardModel<TTop, TBottom>
	where TTop : ThornreaperCardSide
	where TBottom : ThornreaperCardSide
{
	protected override string TexturePath => "res://Content/Classes/Thornreaper/Cards.jpg";
	protected override int ColumnCount => 8;
	protected override int RowCount => 4;
}

public abstract class ThornreaperCardSide : AbilityCardSideModel
{
	protected static readonly Func<Figure, GDTask<bool>> ActionConsumeEarth =
		async figure => await AbilityCmd.AskConsumeElement(figure, Element.Earth, effectInfoText: "Perform action");
}
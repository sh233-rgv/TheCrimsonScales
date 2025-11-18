using System.Collections.Generic;

public abstract class LuminaryCardModel<TTop, TBottom> : AtlasAbilityCardModel<TTop, TBottom>
	where TTop : LuminaryCardSide, new()
	where TBottom : LuminaryCardSide, new()
{
	protected override string TexturePath => "res://Content/Classes/Luminary/Cards.jpg";
	protected override int ColumnCount => 8;
	protected override int RowCount => 4;
}

public abstract class LuminaryCardSide : AbilityCardSide
{
	protected AbilityCardAbility Scuttle(int distance, IReadOnlyCollection<Element> possibleElements)
	{
		return new AbilityCardAbility(MoveAbility.Builder()
			.WithDistance(distance)
			.WithOnAbilityEndedPerformed(async state =>
			{
				await AbilityCmd.InfuseElement(state.Performer, possibleElements);
			})
			.Build());
	}
	
}
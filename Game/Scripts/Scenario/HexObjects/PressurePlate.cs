using System.Collections.Generic;

public partial class PressurePlate : OverlayTile
{
	public override void AddInfoItemParameters(List<InfoItemParameters> parametersList)
	{
		base.AddInfoItemParameters(parametersList);

		parametersList.Add(new GenericInfoItem.Parameters(this, "Pressure Plate", "Read the Special Rules to find out what this Pressure Plate does."));
	}
}
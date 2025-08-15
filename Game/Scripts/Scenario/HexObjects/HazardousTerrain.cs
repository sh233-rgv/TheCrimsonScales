using System.Collections.Generic;
using Godot;

public partial class HazardousTerrain : OverlayTile
{
	public static int DamageAmount => 1 + Mathf.CeilToInt(GameController.Instance.SavedScenario.ScenarioLevel / 3f);

	public override void AddInfoItemParameters(List<InfoItemParameters> parametersList)
	{
		base.AddInfoItemParameters(parametersList);

		parametersList.Add(new GenericInfoItem.Parameters(this, "Hazardous Terrain",
			$"Any normal or forced movement through this hex will cause the figure to suffer {DamageAmount} damage, except when jumping or flying.",
			xOffset: Hexes.Length > 1 ? -Map.HexWidth / 2 : 0));
	}
}
using System;
using System.Collections.Generic;
using Fractural.Tasks;

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

	public static bool LightStrongOrWaning =>
		GameController.Instance.ElementManager.GetState(Element.Light) is ElementState.Strong or ElementState.Waning;

	protected static CreateOverlayTileAbility<ThornsThornreaper>.CreateOverlayTileBuilder CreateThornsAbilityBuilder()
	{
		return CreateOverlayTileAbility<ThornsThornreaper>.Builder()
			.WithCustomAsset("res://Content/Classes/Thornreaper/ThornsThornreaper1H.tscn")
			.WithCustomName("Thorns")
			.WithBeforePlacingTile(figure => RemoveExcessThorns(figure, true));
	}

	public static async GDTask CreateThorns(Figure figure, Hex hex)
	{
		if(await RemoveExcessThorns(figure, false))
		{
			await AbilityCmd.CreateOverlayTile<ThornsThornreaper>(hex,
				SceneLoader.LoadPackedScene("res://Content/Classes/Thornreaper/ThornsThornreaper1H.tscn"));
		}
	}

	private static async GDTask<bool> RemoveExcessThorns(Figure figure, bool mandatory)
	{
		List<ThornsThornreaper> thorns = GameController.Instance.Map.GetChildrenOfType<ThornsThornreaper>();
		if(thorns.Count <= 6)
		{
			return true;
		}

		OverlayTile overlayTile = await AbilityCmd.SelectOverlayTile(figure, overlayTiles =>
		{
			overlayTiles.AddRange(thorns);
		}, mandatory, "Select a thorns to remove");

		if(overlayTile == null)
		{
			return false;
		}

		overlayTile.RemoveFromMap();
		return true;
	}

	protected static OtherAbility InfuseElementIfLightAbility(params Element[] elements)
	{
		return OtherAbility.Builder()
			.WithPerformAbility(async state =>
			{
				foreach(Element element in elements)
				{
					await AbilityCmd.InfuseElement(state, element);
				}

				state.SetPerformed();
			})
			.WithConditionalAbilityCheck(async _ =>
			{
				await GDTask.CompletedTask;

				return LightStrongOrWaning;
			})
			.Build();
	}
}
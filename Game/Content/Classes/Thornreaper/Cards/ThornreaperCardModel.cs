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

	protected static bool LightStrongOrWaning =>
		GameController.Instance.ElementManager.GetState(Element.Light) is ElementState.Strong or ElementState.Waning;

	protected static CreateOverlayTileAbility<ThornsThornreaper>.CreateOverlayTileBuilder CreateThornsAbilityBuilder()
	{
		return CreateOverlayTileAbility<ThornsThornreaper>.Builder()
			.WithCustomAsset("res://Content/Classes/Thornreaper/ThornsThornreaper1H.tscn")
			.WithCustomName("Thorns");
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
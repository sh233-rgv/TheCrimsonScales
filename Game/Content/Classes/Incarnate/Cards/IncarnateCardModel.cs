using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public abstract class IncarnateCardModel<TTop, TBottom> : AbilityCardModel<TTop, TBottom>
	where TTop : IncarnateCardSide
	where TBottom : IncarnateCardSide
{
	protected override string TexturePath => "res://Content/Classes/Incarnate/Cards.jpg";
	protected override int ColumnCount => 8;
	protected override int RowCount => 4;
}

public abstract class IncarnateCardSide : AbilityCardSideModel
{
	protected virtual IEnumerable<IncarnateSpirit> SwitchSpiritChoices => [];

	public override async GDTask OnActionPerformed(Figure figure)
	{
		await base.OnActionPerformed(figure);

		if(SwitchSpiritChoices.Any())
		{
			await ChooseSpirit(figure, SwitchSpiritChoices);
		}
	}

	protected static bool InSpirit(Figure figure, IncarnateSpirit spirit)
	{
		if(figure is Incarnate incarnate)
		{
			return incarnate.Spirit == spirit;
		}

		return false;
	}

	protected static async GDTask<bool> InSpirit(AbilityState state, IncarnateSpirit spirit)
	{
		await GDTask.CompletedTask;
		if(state.Performer is Incarnate incarnate)
		{
			return incarnate.Spirit == spirit;
		}

		return false;
	}

	private static async GDTask ChooseSpirit(Figure figure, IEnumerable<IncarnateSpirit> spiritChoices)
	{
		if(figure is Incarnate incarnate)
		{
			await incarnate.ChooseSpirit(spiritChoices);
		}

		await GDTask.CompletedTask;
	}
}
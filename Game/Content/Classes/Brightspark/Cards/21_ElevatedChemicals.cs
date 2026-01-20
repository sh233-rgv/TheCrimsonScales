using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class ElevatedChemicals : BrightsparkCardModel<ElevatedChemicals.CardTop, ElevatedChemicals.CardBottom>
{
	public override string Name => "Elevated Chemicals";
	public override int Level => 5;
	public override int Initiative => 44;
	protected override int AtlasIndex => 21;

	public class CardTop : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			//TODO
		];
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			//TODO
		];
	}
}
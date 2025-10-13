using System.Collections.Generic;
using Fractural.Tasks;

public partial class Objective : Figure
{
	private string _name;

	public override string DisplayName => _name;
	public override string DebugName => _name;

	public override AMDCardDeck AMDCardDeck => null;

	public void Init(int health, string name)
	{
		_name = name;

		SetMaxHealth(health);
		SetHealth(health);
	}

	public override async GDTask Init(Hex originHex, int rotationIndex = 0, bool hexCanBeNull = false)
	{
		await base.Init(originHex, rotationIndex, hexCanBeNull);

		//_summonViewComponent = GetViewComponent<SummonViewComponent>();

		SetAlignment(Alignment.Enemies);
		SetEnemies(Alignment.Characters);

		GameController.Instance.Map.RegisterFigure(this);

		UpdateInitiative();
	}

	public override void RoundEnd()
	{
		// Don't set CanTakeTurn to true, as objectives can never take turns
	}

	protected override Initiative GetInitiative()
	{
		return new Initiative()
		{
			MainInitiative = 99,
			SortingInitiative = 99 * 10000000
		};
	}

	public override void AddInfoItemParameters(List<InfoItemParameters> parametersList)
	{
		base.AddInfoItemParameters(parametersList);

		parametersList.Add(new ObjectiveInfoItem.Parameters(this));
	}
}
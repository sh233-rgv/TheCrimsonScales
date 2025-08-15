using Godot;

public partial class AbilityCardSideCurrentActionView : CurrentActionView<AbilityCardSideCurrentActionView.Parameters>
{
	public class Parameters : CurrentActionViewParameters
	{
		public override string ScenePath => "res://Scenes/Scenario/UI/CurrentActionViews/AbilityCardSideCurrentActionView.tscn";
		public override object Source => AbilityCardSide;

		public AbilityCardSide AbilityCardSide { get; }
		public bool ShowTop { get; }

		public Parameters(AbilityCardSide abilityCardSide)
		{
			AbilityCardSide = abilityCardSide;
			ShowTop = abilityCardSide.AbilityCard.Top == abilityCardSide;
		}
	}

	[Export]
	private CardSideView _cardSideView;

	public override void Init(Parameters parameters)
	{
		base.Init(parameters);

		_cardSideView.SetCard(parameters.AbilityCardSide);
	}
}
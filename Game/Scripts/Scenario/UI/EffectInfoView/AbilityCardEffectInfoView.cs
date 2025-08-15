using Godot;

public partial class AbilityCardEffectInfoView : EffectInfoView<AbilityCardEffectInfoView.Parameters>
{
	public class Parameters : EffectInfoViewParameters
	{
		public override string ScenePath => "res://Scenes/Scenario/UI/EffectInfoViews/AbilityCardEffectInfoView.tscn";

		public AbilityCard AbilityCard { get; }
		public bool ShowTop { get; }

		public Parameters(AbilityCardSide abilityCardSide)
		{
			AbilityCard = abilityCardSide.AbilityCard;
			ShowTop = abilityCardSide.AbilityCard.Top == abilityCardSide;
		}
	}

	[Export]
	private CardSideView _cardSideView;

	public override void Init(Parameters parameters)
	{
		base.Init(parameters);

		_cardSideView.SetCard(parameters.AbilityCard, parameters.ShowTop);
	}
}
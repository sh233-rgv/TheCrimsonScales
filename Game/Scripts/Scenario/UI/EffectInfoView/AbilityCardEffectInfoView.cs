using Godot;

public partial class AbilityCardEffectInfoView : EffectInfoView<AbilityCardEffectInfoView.Parameters>
{
	public class Parameters : EffectInfoViewParameters
	{
		public override string ScenePath => "res://Scenes/Scenario/UI/EffectInfoViews/AbilityCardEffectInfoView.tscn";

		public AbilityCardModel AbilityCardModel { get; }
		public bool ShowTop { get; }

		public Parameters(AbilityCardSideModel abilityCardSideModel)
		{
			AbilityCardModel = abilityCardSideModel.AbilityCardModel;
			ShowTop = abilityCardSideModel.AbilityCardSideType == AbilityCardSideType.Top;
		}
	}

	[Export]
	private CardSideView _cardSideView;

	public override void Init(Parameters parameters)
	{
		base.Init(parameters);

		_cardSideView.SetCard(parameters.AbilityCardModel, parameters.ShowTop);
	}
}
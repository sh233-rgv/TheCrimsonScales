using Godot;

public partial class AbilityCardEffectInfoView : EffectInfoView<AbilityCardEffectInfoView.Parameters>
{
	public class Parameters : EffectInfoViewParameters
	{
		public override string ScenePath => "res://Scenes/Scenario/UI/EffectInfoViews/AbilityCardEffectInfoView.tscn";

		public AbilityCardSideModel AbilityCardSideModel { get; }

		public Parameters(AbilityCardSideModel abilityCardSideModel)
		{
			AbilityCardSideModel = abilityCardSideModel;
		}
	}

	[Export]
	private AbilityCardSideView _cardSideView;

	public override void Init(Parameters parameters)
	{
		base.Init(parameters);

		_cardSideView.SetCard(parameters.AbilityCardSideModel);
	}
}
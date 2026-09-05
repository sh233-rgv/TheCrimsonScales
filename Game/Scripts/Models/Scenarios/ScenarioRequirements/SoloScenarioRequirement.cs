using System.Linq;

public class SoloScenarioRequirement : ScenarioRequirement
{
	private readonly ClassModel _classModel;

	public SoloScenarioRequirement(ClassModel classModel)
	{
		_classModel = classModel;
	}

	public override bool GetMet(SavedCampaign savedCampaign)
	{
		SavedCharacter savedCharacter =
			GameController.Instance.SavedCampaign.Characters.FirstOrDefault(character => character.ClassModel == _classModel);
		return savedCharacter != null && savedCharacter.Level >= 5;
	}

	public override string NotMetMessage()
	{
		SavedCharacter savedCharacter =
			GameController.Instance.SavedCampaign.Characters.FirstOrDefault(character => character.ClassModel == _classModel);
		return savedCharacter == null
			? $"You require a {_classModel.Name} character to play its solo scenario"
			: $"You require {_classModel.Name} to be at least level 5 to play its solo scenario";
	}
}
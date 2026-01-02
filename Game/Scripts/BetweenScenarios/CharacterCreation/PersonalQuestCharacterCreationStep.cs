using System.Collections.Generic;
using Godot;

public partial class PersonalQuestCharacterCreationStep : CharacterCreationStep
{
	[Export]
	private PackedScene _questScene;
	[Export]
	private Control _questParent;

	private readonly List<CharacterCreationPersonalQuest> _quests = new List<CharacterCreationPersonalQuest>();

	public override bool ConfirmButtonActive => true;

	public override void Activate()
	{
		base.Activate();

		foreach(CharacterCreationPersonalQuest quest in _quests)
		{
			quest.QueueFree();
		}

		_quests.Clear();

		List<PersonalQuestModel> usableClassModels = [ModelDB.PersonalQuest<ProtectAndServe>(), ModelDB.PersonalQuest<WeaponsSpecialist>()];

		for(int i = 0; i < usableClassModels.Count; i++)
		{
			PersonalQuestModel personalQuestModel = usableClassModels[i];
			CharacterCreationPersonalQuest characterCreationClass = _questScene.Instantiate<CharacterCreationPersonalQuest>();
			_questParent.AddChild(characterCreationClass);
			characterCreationClass.Init(personalQuestModel, 0.3f + i * 0.3f);
			characterCreationClass.Fade(1f, 0.3f);
			_quests.Add(characterCreationClass);

			//characterCreationClass.PressedEvent += OnQuestPressed;
		}
	}

	public override void Deactivate()
	{
		base.Deactivate();

		foreach(CharacterCreationPersonalQuest quest in _quests)
		{
			quest.Fade(0f, 0.3f);
		}
	}
}
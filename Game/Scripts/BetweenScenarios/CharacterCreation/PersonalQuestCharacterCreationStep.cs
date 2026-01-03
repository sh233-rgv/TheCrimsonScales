using System.Collections.Generic;
using Godot;

public partial class PersonalQuestCharacterCreationStep : CharacterCreationStep
{
	[Export]
	private PackedScene _questScene;
	[Export]
	private Control _questParent;

	private readonly List<CharacterCreationPersonalQuest> _quests = new List<CharacterCreationPersonalQuest>();
	private CharacterCreationPersonalQuest _selectedQuest;

	public override bool ConfirmButtonActive => _selectedQuest != null;

	public override void Activate()
	{
		base.Activate();

		foreach(CharacterCreationPersonalQuest quest in _quests)
		{
			quest.QueueFree();
		}

		_quests.Clear();

		List<PersonalQuestModel> personalQuests = [ModelDB.PersonalQuest<ProtectAndServe>(), ModelDB.PersonalQuest<WeaponsSpecialist>()];

		for(int i = 0; i < personalQuests.Count; i++)
		{
			PersonalQuestModel personalQuestModel = personalQuests[i];
			CharacterCreationPersonalQuest personalQuest = _questScene.Instantiate<CharacterCreationPersonalQuest>();
			_questParent.AddChild(personalQuest);
			personalQuest.Init(personalQuestModel, 0.3f + i * 0.3f);
			personalQuest.Fade(1f, 0.3f);
			_quests.Add(personalQuest);

			personalQuest.PressedEvent += OnQuestPressed;
		}
	}

	public override void Deactivate()
	{
		base.Deactivate();

		_selectedQuest = null;

		foreach(CharacterCreationPersonalQuest quest in _quests)
		{
			quest.Fade(0f, 0.3f);
		}
	}

	private void OnQuestPressed(CharacterCreationPersonalQuest personalQuest)
	{
		if(personalQuest == _selectedQuest)
		{
			return;
		}

		_selectedQuest = personalQuest;

		foreach(CharacterCreationPersonalQuest otherQuest in _quests)
		{
			otherQuest.SetSelected(false, true);
		}

		_selectedQuest.SetSelected(true, true);

		_characterCreationOverlay.SetPersonalQuestModel(_selectedQuest.PersonalQuestModel);
		_characterCreationOverlay.UpdateConfirmVisible();
	}
}
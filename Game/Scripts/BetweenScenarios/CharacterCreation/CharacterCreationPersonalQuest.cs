using Godot;
using GTweensGodot.Extensions;

public partial class CharacterCreationPersonalQuest : Control
{
	[Export]
	private RotatingCardView _rotatingCardView;
	[Export]
	private PersonalQuestView _personalQuestView;

	public PersonalQuestModel QuestModel { get; private set; }

	public void Init(PersonalQuestModel questModel, float rotationDelay)
	{
		QuestModel = questModel;

		_personalQuestView.SetModulate(Colors.Transparent);
		_rotatingCardView.GetRotationTween(() =>
		{
			_personalQuestView?.SetPersonalQuest(QuestModel);
		}, rotationDelay).Play();
	}

	public void Fade(float target, float duration)
	{
		_personalQuestView.TweenModulateAlpha(target, duration).Play();
	}
}
using Godot;

public partial class Events : BetweenScenariosAction
{
	[Export]
	private EventCard _eventCard;

	protected override bool SelectCharacter => false;

	public override void _Ready()
	{
		base._Ready();

		_eventCard.SetModel(ModelDB.Event<City01>());
	}
}
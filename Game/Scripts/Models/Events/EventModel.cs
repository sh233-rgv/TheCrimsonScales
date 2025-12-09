public abstract class EventModel<TA, TB> : EventModel
	where TA : EventChoiceModel //, new()
	where TB : EventChoiceModel //, new()
{
}

public abstract class EventModel : AbstractModel<EventModel>
{
	public abstract int Number { get; }
	public abstract string Text { get; }
}
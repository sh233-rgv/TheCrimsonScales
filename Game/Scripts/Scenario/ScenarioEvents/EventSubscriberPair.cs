public class EventSubscriberPair : IEventSubscriber
{
	public object SubscriberA { get; }
	public object SubscriberB { get; }

	public EventSubscriberPair(object subscriberA, object subscriberB)
	{
		SubscriberA = subscriberA;
		SubscriberB = subscriberB;
	}
}
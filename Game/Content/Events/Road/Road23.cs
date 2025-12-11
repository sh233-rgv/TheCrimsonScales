using System.Collections.Generic;

public class Road23 : RoadEventModel<Road23.ChoiceA, Road23.ChoiceB>
{
	public override int Number => 01;

	public override string Text =>
		"""
		While wandering the road at night, you suddenly see a flash of bright light, followed by a loud shattering noise originating from a cave nearby. Near it, an abandoned merchant cart is parked.

		As you head inside the cave, the sound of sword slashes, breaking glass and the occasional flash of light guides you deeper, when suddenly you see a merchant and his guard fighting an Orchid. On the ground near the Orchid, a sack filled with glowing red crystals that could be of either party.

		As you inspect the Orchid, which has a crystal skin far larger than you have ever seen, he breaks off a piece of his crystal skin which he threatens to throw at you. At the same time, the merchant manages to grab the sack and attempts to flee.
		""";

	public class ChoiceA : EventChoiceModel
	{
		public override string ChoiceText => "Engage the Orchid.";

		public override string GetStoryText(SavedEventState state) => //TODO
			"""
			You ask the Orchid for directions and she points you toward the right path. You wish her well with her endeavors and reach your destination in peace.
			""";

		public override EventResolveType GetEventResolveType(SavedEventState state) => EventResolveType.ReturnCardToBottom;

		public override List<EventReward> GetRewards(SavedEventState state) => [];
	}

	public class ChoiceB : EventChoiceModel
	{
		public override string ChoiceText => "Stop the merchant in his attempt to flee with the crystals.";

		public override string GetStoryText(SavedEventState state) => //TODO
			"""
			You offer to help the Orchid on her mission and her face lights up with contentment. She relays the coordinates of where to find the beasts and wishes you luck as she excuses herself to tend to her tribe.
			""";

		public override List<EventReward> GetRewards(SavedEventState state) =>
		[
			//new ScenarioUnlockEventReward(ModelDB.Scenario<Scenario042>())
		];
	}
}
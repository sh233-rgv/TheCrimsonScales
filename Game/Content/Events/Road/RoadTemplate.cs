using System.Collections.Generic;

public class RoadTemplate : CityEventModel<RoadTemplate.ChoiceA, RoadTemplate.ChoiceB>
{
	public override int Number => 00;

	public override string Text =>
		"""
		TODO
		""";

	public class ChoiceA : EventChoiceModel
	{
		public override string ChoiceText => "TODO";

		public override string GetStoryText(SavedEventState state) =>
			"""
			TODO
			""";

		public override List<EventReward> GetRewards(SavedEventState state) => [];
	}

	public class ChoiceB : EventChoiceModel
	{
		public override string ChoiceText => "TODO";

		public override string GetStoryText(SavedEventState state) =>
			"""
			TODO
			""";

		public override List<EventReward> GetRewards(SavedEventState state) => [];
	}
}
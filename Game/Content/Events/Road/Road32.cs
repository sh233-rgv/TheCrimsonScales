using System.Collections.Generic;

public class Road32 : CityEventModel<Road32.ChoiceA, Road32.ChoiceB>
{
	public override int Number => 32;

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
using System.Collections.Generic;

public class Road37 : RoadEventModel<Road37.ChoiceA, Road37.ChoiceB>
{
	public override int Number => 37;

	public override string Text =>
		"""
		A faint groaning sound coming from ahead catches your attention. As you approach closer, you take notice of an injured bear on the side of the road. You cautiously approach the animal with one hand on your blade in case it decides to attack.

		Further inspection reveals that the bear is in poor condition. Its feet appear to be twisted and its body severely wounded. There are birds of prey circling above. If left alone, the bear won't survive the night.
		""";

	public class ChoiceA : EventChoiceModel
	{
		public override string ChoiceText => "Tend to its wounds.";

		public override string GetStoryText(SavedEventState state) =>
			"""
			You take the time to nurse the bear back to health. Confident that you've given it a fighting chance for survival, you begin to leave, only to notice the bear following you. It's limping, but you sense gratitude and it doesn't seem to want to leave your side.
			""";

		public override List<EventReward> GetRewards(SavedEventState state) =>
		[
			new OnScenarioStartedEventReward(
				async () =>
				{
					Hex hex = await AbilityCmd.SelectHex(GameController.Instance.CharacterManager.GetCharacter(0),
						list =>
						{
							foreach(Character character in GameController.Instance.CharacterManager.Characters)
							{
								foreach(Hex hex in RangeHelper.GetHexesInRange(character.Hex, 1, false))
								{
									if(hex.IsEmpty())
									{
										list.AddIfNew(hex);
									}
								}
							}
						}, hintText: "Select a hex to spawn the allied Vermling Scout"
					);

					if(hex != null)
					{
						Monster monster = await AbilityCmd.SpawnMonster(ModelDB.Monster<VermlingScout>(), MonsterType.Normal, hex);
						if(monster != null)
						{
							monster.SetAlignment(Alignment.Characters);
							monster.SetEnemies(Alignment.Enemies);
							monster.SetMaxHealth(5);
							monster.SetHealth(5);
						}
					}
				},
				color =>
					"At the start of the next scenario, an allied Cave Bear with a maximum hit point value of 5 will spawn next to any character."
			)
		];
	}

	public class ChoiceB : EventChoiceModel
	{
		public override string ChoiceText => "Don't poke the bear, it's too risky.";

		public override string GetStoryText(SavedEventState state) =>
			"""
			You ignore the bear's pleading for help and carry on with your journey. You feel a slight sense of remorse as you wonder what could have been had you helped the bear, but there's no looking back now.
			""";

		public override List<EventReward> GetRewards(SavedEventState state) =>
		[
			new AllStartScenarioWithConditionEventReward(Conditions.Curse)
		];
	}
}
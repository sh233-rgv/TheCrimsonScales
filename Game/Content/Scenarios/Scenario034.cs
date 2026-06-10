using System.Collections.Generic;
using Fractural.Tasks;

public class Scenario034 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario034.tscn";

	public override int ScenarioNumber => 34;
	public override string Name => "Great Oak Rescue";

	public override List<ScenarioLink> Links => [new ScenarioLink<Scenario033>()];

	protected override List<ScenarioRequirement> Requirements => [new PersonalQuestRequirement(ModelDB.PersonalQuest<ProtectAndServe>())];
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<PersonalQuestScenarioChain>();

	public override string IntroductionText =>
		"""
		After the short-lived celebration, it isn’t long until commotion is once again stirred up in the city. Shouts of guards can be heard in the distance with soldiers one again donning their armor and weapons to ready defenses. You spot the Captain of the Guard giving orders to the ramshackle city defenses.

		The thunderous booms begin once more. Boom! Boom! Boom!

		Quatryl Bombards can be seen higher on the western wall igniting their multicannons. Other soldiers around him cover their ears yet the Captain simply spits and continues barking orders over the commotion.

		“Where are those reinforcements?” He says to no one in particular. You are caught up in the commotion yourself not sure quite what to do. Just then a woman in armor runs up to the Captain. She removes the conical helmet from her head, revealing darkbraided hair.

		“My Captain, they’re attacking at Temple of the Oak, a small number have infiltrated the northern gate!” She says wincing at the sound of continued canon fire. “I can’t spare a single soldier Lieutenant!” He says, holding up a finger to her as he yells one again at the men in his squad. The Lieutenant turns to you.

		“I heard your group has demonstrated their loyalty this day, will you help us once again?” She seems to observe your ambivalence on the matter. “I’m sure the Council will reward it’s levies greatly, now come!”

		You make your way to the temple to find it surrounded by Inox. One very large Inox stands with his fist raised in the air giving orders to his fellow Inox. You don’t speak this dialect of Inoxian but you do catch the words “burn the city dwellers!”

		That’s enough for you to hear to make sure they don’t succeed this day. This must be Ogrum Bonebreaker. He mercilessly kills a temple guardian before you and then marches inside. His guardsmen have spotted you and will not give up their siege so easily.
		""";

	public override string ConclusionText =>
		"""
		Orgrum lies dead and so does his contingent. It will be some time until order can be restored after this Inox raid. You sit and think about the implications, Orgrum was representing his people but you couldn’t simply let him have Gloomhaven in revenge. You try to stay out of these political matters. Leaving the Temple you rejoin the Lieutenant and the Captain who thank you for your service. After a few words of gratitude are exchanged their attention turns away from you.

		A small Quatryl with soot-covered face and hands scampers by. “Fizgar,” the Captain says, “I think we owe you all a great deal of gratitude to you and your team of Bombards. I will speak with the Lord Captain but I am most reassured he will agree with me that you are welcome amongst our ranks.”

		You take your earnings from the day and head back to your part of town. Your head isn’t pounding as much as it was this morning. For a moment you think maybe you’ll swear off ale.

		Entering into the Sleeping Lion you smell the sweet aroma of fine ales and decide to quit another time.
		""";

	public override List<MonsterModel> MonsterModels { get; } =
	[
		ModelDB.Monster<Hound>(),
		ModelDB.Monster<InoxArcher>(),
		ModelDB.Monster<InoxGuard>(),
		ModelDB.Monster<InoxShaman>(),
	];

	public override List<SavedReward> Rewards =>
	[
		new OpenEnvelopeReward(ModelDB.PersonalQuest<ProtectAndServe>()),
		new AllStartScenarioWithConditionReward(Conditions.Bless),
		new GainReputationReward(2),
	];

	public override async GDTask InitializeBeforeFirstRoomRevealed()
	{
		await base.InitializeBeforeFirstRoomRevealed();

		List<CharacterStartHex> hexes = GameController.Instance.Map.GetChildrenOfType<CharacterStartHex>();

		// Remove some start hexes because the start rules are a bit weird
		await RemoveStartHex(hexes, 2);
		await RemoveStartHex(hexes, 3);

		await RemoveStartHex(hexes, 6);
		await RemoveStartHex(hexes, 7);

		// Remove start hexes for player counts smaller than 4
		if(CharacterCount <= 3)
		{
			await RemoveStartHex(hexes, 5);

			if(CharacterCount == 2)
			{
				await RemoveStartHex(hexes, 4);
			}
		}
	}

	public override async GDTask InitializeAfterFirstRoomRevealed()
	{
		await base.InitializeAfterFirstRoomRevealed();

		await AddGoal(new KillAllEnemiesScenarioGoal(true));

		//AddScenarioRule("The same pressure plate cannot be activated twice in a row.");
	}

	private async GDTask RemoveStartHex(List<CharacterStartHex> hexes, int index)
	{
		CharacterStartHex hex = hexes[index];

		await hex.Destroy(true);
		// hex.SetVisible(false);
		// hex.QueueFree();
	}
}
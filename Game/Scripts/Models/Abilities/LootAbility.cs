using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class LootAbility : Ability<LootAbility.State>
{
	public class State : AbilityState
	{
		public List<LootableObject> LootedObjects { get; } = new List<LootableObject>();
		public int LootedCoinCount { get; set; }
		public int TotalLootedCount { get; set; }
	}

	private readonly Func<State, Figure> _customGetLootObtainer;
	public int Range { get; }

	public LootAbility(int range, Func<State, Figure> customGetLootObtainer = null,
		Func<State, GDTask> onAbilityStarted = null, Func<State, GDTask> onAbilityEnded = null, Func<State, GDTask> onAbilityEndedPerformed = null,
		ConditionalAbilityCheckDelegate conditionalAbilityCheck = null,
		List<ScenarioEvents.AbilityStarted.Subscription> abilityStartedSubscriptions = null,
		List<ScenarioEvents.AbilityEnded.Subscription> abilityEndedSubscriptions = null,
		List<ScenarioEvents.AbilityPerformed.Subscription> abilityPerformedSubscriptions = null)
		: base(onAbilityStarted, onAbilityEnded, onAbilityEndedPerformed, conditionalAbilityCheck, abilityStartedSubscriptions, abilityEndedSubscriptions, abilityPerformedSubscriptions)
	{
		_customGetLootObtainer = customGetLootObtainer;
		Range = range;
	}

	protected override async GDTask Perform(State abilityState)
	{
		Figure lootObtainer = abilityState.Performer;

		if(_customGetLootObtainer != null)
		{
			lootObtainer = _customGetLootObtainer(abilityState);
		}

		LootPrompt.Answer confirmAnswer = await PromptManager.Prompt(new LootPrompt(list =>
		{
			foreach(Hex hex in RangeHelper.GetHexesInRange(abilityState.Performer.Hex, Range))
			{
				foreach(HexObject hexObject in hex.HexObjects)
				{
					if(hexObject is LootableObject lootableObject && lootableObject.CanLoot(lootObtainer))
					{
						list.AddIfNew(hex);
					}
				}
			}
		}, null, () => "Perform loot?"), abilityState.Authority);

		if(!confirmAnswer.Skipped)
		{
			foreach(Vector2I coords in confirmAnswer.HexCoords)
			{
				Hex hex = GameController.Instance.Map.GetHex(coords);

				await LootHex(abilityState, hex, lootObtainer);
			}

			abilityState.SetPerformed();
		}
	}

	public static async GDTask LootHex(State abilityState, Hex hex, Figure lootObtainer)
	{
		for(int i = hex.HexObjects.Count - 1; i >= 0; i--)
		{
			HexObject hexObject = hex.HexObjects[i];
			if(hexObject is LootableObject lootableObject && lootableObject.CanLoot(lootObtainer))
			{
				abilityState.LootedObjects.Add(lootableObject);
				if(lootableObject is CoinStack coinStack)
				{
					abilityState.LootedCoinCount += coinStack.CoinCount;
					abilityState.TotalLootedCount += coinStack.CoinCount;
				}
				else
				{
					abilityState.TotalLootedCount++;
				}

				await lootableObject.Loot(lootObtainer);

				abilityState.SetPerformed();
			}
		}
	}
}
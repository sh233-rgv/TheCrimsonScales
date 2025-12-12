using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public abstract class ScenarioModel : AbstractModel<ScenarioModel>, IEventSubscriber
{
	public ScenarioGoals ScenarioGoals { get; private set; }

	public abstract string ScenePath { get; }
	public abstract int ScenarioNumber { get; }
	public abstract ScenarioChain ScenarioChain { get; }
	public virtual IEnumerable<ScenarioConnection> Connections { get; } = [];
	public virtual int[] TreasureNumbers { get; } = [];

	protected virtual IEnumerable<ScenarioRequirement> ScenarioRequirements { get; } = [];

	public virtual string BGMPath => "res://Audio/BGM/Floral-Woods.ogg";
	public virtual string BGSPath => null;

	public virtual async GDTask StartBeforeFirstRoomRevealed()
	{
		ScenarioGoals = CreateScenarioGoals();
		UpdateScenarioText(null);

		await GDTask.CompletedTask;
	}

	public virtual async GDTask StartAfterFirstRoomRevealed()
	{
		ScenarioGoals.Start();

		ScenarioEvents.RoomRevealedEvent.Subscribe(this, parameters => true, OnRoomRevealed);

		await GDTask.CompletedTask;
	}

	protected virtual async GDTask OnRoomRevealed(ScenarioEvents.RoomRevealed.Parameters parameters)
	{
		await GDTask.CompletedTask;
	}

	protected abstract ScenarioGoals CreateScenarioGoals();

	protected virtual void UpdateScenarioText(string text)
	{
		string displayText;
		if(text != null)
		{
			displayText = $"{ScenarioGoals.Text}\n\n{text}";
		}
		else
		{
			displayText = ScenarioGoals.Text;
		}

		GameController.Instance.SpecialRulesView.SetText(displayText);
	}

	protected async GDTask SpawnMonster(Figure authority, MonsterModel monsterModel, MonsterType monsterType, IEnumerable<Hex> spawnHexes, int? monsterLevel = null)
	{
		if (authority == null)
        {
            authority = GameController.Instance.Map.Figures.First(figure => figure is Character);
        }
		List<Hex> hexes = RangeHelper.GetHexesInRange(spawnHexes.First(), 100, requiresLineOfSight: false).ToList();		

		Hex chosenHex = await AbilityCmd.SelectHex(authority,
			list =>
            {
                int? minDistance = null;
				foreach(Hex spawnHex in spawnHexes)
				{
					hexes.Shuffle(GameController.Instance.StateRNG);
					hexes.Sort((otherHexA, otherHexB) => RangeHelper.Distance(spawnHex, otherHexA).CompareTo(RangeHelper.Distance(spawnHex, otherHexB)));Hex firstHex = null;
					foreach(Hex hex in hexes)
					{
						if(hex.IsEmpty())
						{
							firstHex = hex;
							break;
						}
					}

					if(firstHex == null)
					{
						return;
					}

					int distance = RangeHelper.Distance(spawnHex, firstHex);
					if (minDistance == null || distance <= minDistance)
					{
						if (minDistance == null || distance < minDistance)
                        {
                            list.Clear();
							minDistance = distance;
                        }
						foreach(Hex otherHex in hexes)
						{
							int otherDistance = RangeHelper.Distance(spawnHex, otherHex);
							if(otherHex.IsEmpty() && otherDistance == distance)
							{
								list.Add(otherHex);
							}
						}
					}		
				}
            }, true, $"Select where to spawn the {monsterType} {monsterModel.Name}"
		);

		if(chosenHex == null)
		{
			return;
		}

		await AbilityCmd.SpawnMonster(monsterModel, monsterType, chosenHex, monsterLevel);
	}
}
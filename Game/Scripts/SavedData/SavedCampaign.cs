using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class SavedCampaign
{
	public static int[] ProsperityLevelThresholds =
	[
		0, // 1
		4, // 2
		9, // 3
		15, // 4
		22, // 5
		30, // 6
		39, // 7
		49, // 8
		59, // 9
	];

	public static int[] ReputationPriceCostThresholds =
	[ // -5
		-18, // -4
		-14, // -3
		-10, // -2
		-6, // -1
		-2, // -0
		3, // 1
		7, // 2
		11, // 3
		15, // 4
		19 // 5
	];

	[JsonProperty]
	public string PartyName { get; private set; }

	[JsonProperty]
	public StartingGroup StartingGroup { get; private set; }

	[JsonProperty]
	public List<SavedCharacter> Characters { get; private set; } = new List<SavedCharacter>();

	[JsonProperty]
	public Dictionary<string, SavedClass> SavedClasses { get; private set; } = new Dictionary<string, SavedClass>();

	[JsonProperty]
	public SavedScenarioProgresses SavedScenarioProgresses { get; private set; }

	[JsonProperty]
	public Dictionary<string, SavedItem> SavedItems { get; private set; } = new Dictionary<string, SavedItem>();

	[JsonProperty]
	public SavedScenario SavedScenario { get; private set; }

	[JsonProperty]
	public List<PartyAchievement> CollectedPartyAchievements { get; private set; } = [];

	[JsonProperty]
	public SavedSanctuaryOfTheGreatOak SanctuaryOfTheGreatOak { get; private set; } = new SavedSanctuaryOfTheGreatOak();

	[JsonProperty]
	public SavedEvents SavedEvents { get; private set; } = new SavedEvents();

	[JsonProperty]
	public int Reputation { get; private set; }

	[JsonProperty]
	public int Prosperity { get; private set; }

	public event Action CharactersChangedEvent;
	public event Action ReputationChangedEvent;
	public event Action ProsperityChangedEvent;

	public static SavedCampaign New(string partyName, StartingGroup startingGroup)
	{
		SavedCampaign savedCampaign = new SavedCampaign()
		{
			PartyName = partyName,
			StartingGroup = startingGroup,
			Characters =
			[
			],
			SavedScenarioProgresses = new SavedScenarioProgresses()
			{
			},
		};

		ClassModel[] unlockedClassModels = savedCampaign.StartingGroup switch
		{
			StartingGroup.Militants =>
			[
				ModelDB.Class<BombardModel>(),
				ModelDB.Class<FireKnightModel>(),
				ModelDB.Class<HierophantModel>(),
				ModelDB.Class<MirefootModel>()
			],
			StartingGroup.Protectors =>
			[
				ModelDB.Class<ChainguardModel>(),
				ModelDB.Class<ChieftainModel>(),
				ModelDB.Class<FireKnightModel>(),
				ModelDB.Class<HierophantModel>()
			],
			StartingGroup.Explorers =>
			[
				//ModelDB.Class<BrightsparkModel>(),
				ModelDB.Class<ChainguardModel>(),
				//ModelDB.Class<HollowpactModel>(),
				ModelDB.Class<StarslingerModel>()
			],
			StartingGroup.Trailblazers =>
			[
				ModelDB.Class<BombardModel>(),
				//ModelDB.Class<BrightsparkModel>(),
				//ModelDB.Class<LuminaryModel>(),
				ModelDB.Class<StarslingerModel>()
			],
			StartingGroup.Naturalists =>
			[
				ModelDB.Class<ChieftainModel>(),
				//ModelDB.Class<HollowpactModel>(),
				//ModelDB.Class<LuminaryModel>(),
				ModelDB.Class<MirefootModel>()
			],
			_ => throw new ArgumentOutOfRangeException(nameof(startingGroup), startingGroup, null)
		};

		foreach(ClassModel unlockedClassModel in unlockedClassModels)
		{
			savedCampaign.UnlockClass(unlockedClassModel);
		}

		// Unlock the first scenario
		SavedScenarioProgress firstScenario = new SavedScenarioProgress();
		firstScenario.Discover();
		savedCampaign.SavedScenarioProgresses.ScenarioProgresses.Add(ModelDB.GetId<Scenario001>().ToString(), firstScenario);

		// Unlock the first set of items
		ItemModel[] itemModels =
		[
			ModelDB.Item<AmuletOfLife>(),
			ModelDB.Item<CircletOfElements>(),
			ModelDB.Item<HideArmor>(),
			ModelDB.Item<LeatherArmor>(),
			ModelDB.Item<WeatheredBoots>(),
			ModelDB.Item<WingedShoes>(),
			ModelDB.Item<ShoesOfHappiness>(),
			ModelDB.Item<BootsOfSpeed>(),
			ModelDB.Item<HeaterShield>(),
			ModelDB.Item<PoisonDagger>(),
			ModelDB.Item<HookedChain>(),
			ModelDB.Item<IronSpear>(),
			ModelDB.Item<MinorHealingPotion>(),
			ModelDB.Item<MinorPowerPotion>(),
			ModelDB.Item<MinorManaPotion>(),
		];

		foreach(ItemModel itemModel in itemModels)
		{
			SavedItem savedItem = new SavedItem(itemModel);
			savedItem.AddUnlocked(itemModel.ShopCount);
			savedItem.AddStock(itemModel.ShopCount);
			savedCampaign.SavedItems.Add(itemModel.Id.ToString(), savedItem);
		}

		return savedCampaign;
	}

	public static SavedCampaign Test()
	{
		SavedCampaign savedCampaign = New("Party Time", StartingGroup.Militants);

		savedCampaign.AddCharacter(ModelDB.Class<MirefootModel>(), "Swampguy");
		//savedCampaign.AddCharacter(ModelDB.Class<BombardModel>(), "Bombo");
		//savedCampaign.AddCharacter(ModelDB.Class<HierophantModel>(), "Conclave Man");
		//savedCampaign.AddCharacter(ModelDB.Class<FireKnightModel>(), "Vuur Knecht");
		savedCampaign.AddCharacter(ModelDB.Class<StarslingerModel>(), "Sterrenwerper");
		//savedCampaign.AddCharacter(ModelDB.Class<ChieftainModel>(), "Dierenzitter");

		//savedCampaign.Characters[0].AddItem(ModelDB.Item<MinorManaPotion>());
		savedCampaign.Characters[0].SetEquippedSmallSlotItem(0, ModelDB.Item<TranslocationDevice>());
		//savedCampaign.Characters[1].SetEquippedSmallSlotItem(0, ModelDB.Item<ScrollOfCharisma>());
		//savedCampaign.Characters[1].AddItem(ModelDB.Item<MinorManaPotion>());
		savedCampaign.Characters[0].AddItem(ModelDB.Item<PoisonDagger>());

		// SavedScenarioProgress testScenario = new SavedScenarioProgress();
		// testScenario.Discover();
		// savedCampaign.SavedScenarioProgresses.ScenarioProgresses.Add(ModelDB.GetId<Scenario029>().ToString(), testScenario);

		return savedCampaign;
	}

	public void SetPartyName(string name)
	{
		PartyName = name;
	}

	public void SetSavedScenario(SavedScenario savedScenario)
	{
		SavedScenario = savedScenario;
	}

	public SavedItem GetSavedItem(ItemModel itemModel)
	{
		if(!SavedItems.TryGetValue(itemModel.Id.ToString(), out SavedItem savedItem))
		{
			savedItem = new SavedItem(itemModel);
			SavedItems.Add(itemModel.Id.ToString(), savedItem);
		}

		return savedItem;
	}

	public void UnlockClass(ClassModel classModel)
	{
		string classModelId = classModel.Id.ToString();
		if(!SavedClasses.TryGetValue(classModelId, out SavedClass savedClass))
		{
			savedClass = new SavedClass();
			SavedClasses.Add(classModelId, savedClass);
		}

		savedClass.Unlock();
	}

	public void AddCharacter(ClassModel classModel, string name)
	{
		SavedCharacter character = new SavedCharacter(classModel, name);
		Characters.Add(character);

		CharactersChangedEvent?.Invoke();
	}

	public void DeleteCharacter(SavedCharacter savedCharacter)
	{
		// Move all items from this character (back) to the shop
		foreach(string itemId in savedCharacter.ItemIds)
		{
			ItemModel itemModel = ModelDB.GetById<ItemModel>(itemId);
			SavedItem savedItem = GetSavedItem(itemModel);
			savedItem.AddStock(1);
		}

		SanctuaryOfTheGreatOak.ReturnCards(savedCharacter);

		Characters.Remove(savedCharacter);

		CharactersChangedEvent?.Invoke();
	}

	public void AdjustProsperity(int prosperityAmount)
	{
		int oldProsperityLevel = GetProsperityLevel();
		int thresholdProsperityAmount = ProsperityLevelThresholds[Mathf.Min(oldProsperityLevel, ProsperityLevelThresholds.Length - 1)];
		Prosperity += prosperityAmount;
		Prosperity = Mathf.Max(Prosperity, thresholdProsperityAmount);

		int newProsperityLevel = GetProsperityLevel();
		if(newProsperityLevel > oldProsperityLevel)
		{
			// New prosperity level, unlock new items
		}

		ProsperityChangedEvent?.Invoke();
	}

	public int GetProsperityLevel()
	{
		for(int i = 0; i < ProsperityLevelThresholds.Length; i++)
		{
			int threshold = ProsperityLevelThresholds[i];
			if(threshold > Prosperity)
			{
				return i;
			}
		}

		return ProsperityLevelThresholds.Length;
	}

	public void AddPartyAchievement(PartyAchievement partyAchievement)
	{
		CollectedPartyAchievements.AddIfNew(partyAchievement);
	}

	public void AdjustReputation(int reputationAmount)
	{
		Reputation += reputationAmount;
		Reputation = Mathf.Clamp(Reputation, -20, 20);

		ReputationChangedEvent?.Invoke();
	}

	public int GetItemPriceChange()
	{
		int thresholdIndex = 0;
		for(int i = 0; i < ReputationPriceCostThresholds.Length; i++)
		{
			int threshold = ReputationPriceCostThresholds[i];
			if(threshold > Reputation)
			{
				thresholdIndex = i;
				break;
			}

			if(i == ReputationPriceCostThresholds.Length - 1)
			{
				thresholdIndex = ReputationPriceCostThresholds.Length;
			}
		}

		return 5 - thresholdIndex;
	}
}
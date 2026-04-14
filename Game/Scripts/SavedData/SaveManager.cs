using System.Collections.Generic;
using Newtonsoft.Json;

public class SaveManager
{
	public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
	{
		Formatting = Formatting.Indented,
		TypeNameHandling = TypeNameHandling.Auto,
		NullValueHandling = NullValueHandling.Ignore,
		ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
		ContractResolver = SaveFileContractResolver.Instance,
		ObjectCreationHandling = ObjectCreationHandling.Replace
	};

	private readonly List<object> _saveBlockers = new List<object>();

	public SaveFile<DeviceSaveData> DeviceSaveFile { get; private set; }
	public SaveFile<CampaignSaveData> CampaignSaveFile { get; private set; }

	public bool CanSave => _saveBlockers.Count == 0;

	public SaveManager()
	{
		DeviceSaveFile = new SaveFile<DeviceSaveData>("user://DeviceSaveFile.save");
	}

	public void SetCampaign(string fileName)
	{
		DeviceSaveFile = new SaveFile<DeviceSaveData>($"user://Campaign-{fileName}.save");
	}

	public void SaveAll()
	{
		if(CanSave)
		{
			return;
		}

		SaveGame();
		SaveDevice();
	}

	public void SaveGame()
	{
		if(CanSave)
		{
			return;
		}

		CampaignSaveFile?.Save();
	}

	public void SaveDevice()
	{
		if(CanSave)
		{
			return;
		}

		DeviceSaveFile.Save();
	}

	public void BlockSaving(object blocker)
	{
		_saveBlockers.Add(blocker);
	}

	public void UnblockSaving(object blocker)
	{
		_saveBlockers.Remove(blocker);
	}
}
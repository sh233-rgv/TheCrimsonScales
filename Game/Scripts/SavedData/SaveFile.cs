using System;
using Godot;
using Newtonsoft.Json;

public class SaveFile<TSaveData>
	where TSaveData : SaveData, new()
{
	private readonly string _path;

	public TSaveData SaveData { get; }
	public bool RemovedSavedScenario { get; }

	public SaveFile(string path)
	{
		_path = path;

		if(FileAccess.FileExists(path))
		{
			string json = FileAccess.GetFileAsString(_path);
			try
			{
				json = Migrator.Migrate(json, GetVersion(), out bool removedSavedScenario);
				SaveData = JsonConvert.DeserializeObject<TSaveData>(json, SaveManager.JsonSerializerSettings);
				RemovedSavedScenario = removedSavedScenario;
			}
			catch(Exception e)
			{
				Log.Error(e);
				SaveData = null;
				return;
			}
		}

		if(SaveData == null)
		{
			SaveData = new TSaveData()
			{
				//PlayerId = Guid.NewGuid(),
				//SavedCampaign = null,
				MigrationVersion = Migrator.MigrationVersion
			};
		}
	}

	public void Save()
	{
		if(SaveData == null)
		{
			return;
		}

		SaveData.AppVersion = GetVersion();

		using FileAccess file = FileAccess.Open(_path, FileAccess.ModeFlags.Write);

		string json = JsonConvert.SerializeObject(SaveData, SaveManager.JsonSerializerSettings);
		file.StoreLine(json);
	}

	private static string GetVersion()
	{
		return ProjectSettings.GetSetting("application/config/version").AsString();
	}
}
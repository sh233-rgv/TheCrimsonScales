using Newtonsoft.Json.Linq;

public class Migration002 : Migration
{
	public override void Migrate(JObject saveData)
	{
		// Set the levels of all characters to 1
		if(saveData.TryGetValue("SavedCampaign", out JToken savedCampaign))
		{
			JToken savedCharacters = savedCampaign["Characters"];
			if(savedCharacters != null)
			{
				JArray savedCharactersArray = (JArray)savedCharacters;
				foreach(JToken savedCharacter in savedCharactersArray)
				{
					savedCharacter["Level"] = 1;
				}
			}
		}
	}
}
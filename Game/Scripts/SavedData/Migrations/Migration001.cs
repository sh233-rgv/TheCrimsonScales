using Newtonsoft.Json.Linq;

public class Migration001 : Migration
{
	public override void Migrate(JObject saveData)
	{
		// Remove the saved options
		saveData.Remove("Options");
	}
}
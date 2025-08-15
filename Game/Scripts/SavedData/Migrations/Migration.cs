using Newtonsoft.Json.Linq;

public abstract class Migration
{
	public abstract void Migrate(JObject saveData);
}
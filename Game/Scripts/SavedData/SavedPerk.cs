using System;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class SavedPerk
{
	public PerkModel PerkModel { get; }

	public SavedPerk()
	{
	}

	public SavedPerk(PerkModel perkModel)
	{
		PerkModel = perkModel;
	}
}
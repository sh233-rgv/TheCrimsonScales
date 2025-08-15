using System;
using Fractural.Tasks;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class ShortRestSyncedAction : SyncedAction
{
	protected ShortRestSyncedAction()
	{
	}

	public ShortRestSyncedAction(Character character)
		: base(character)
	{
	}

	public override async GDTask Perform()
	{
		await Owner.ShortRest();
	}
}
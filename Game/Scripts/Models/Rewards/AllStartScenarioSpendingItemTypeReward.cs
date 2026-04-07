using Fractural.Tasks;
using Godot;

public class AllStartScenarioSpendingItemTypeReward(ItemType itemType) : Reward
{
	public override RewardType Type => RewardType.ScenarioStart;

	public override string GetLabelText(RichTextParameters parameters) =>
		$"All characters start the next scenario spending a {Icons.Inline(Icons.GetItem(itemType), parameters)} each.";

	public override async GDTask OnScenarioSetupPhaseCompleted()
	{
		await base.OnScenarioSetupPhaseCompleted();

		foreach(Character character in GameController.Instance.CharacterManager.Characters)
		{
			ItemModel item =
				await AbilityCmd.SelectItem(character, ItemState.Spent, requiredItemType: itemType, hintText: "Select an item to spend");
			if(item != null)
			{
				await AbilityCmd.SpendItem(item);
			}
		}
	}
}
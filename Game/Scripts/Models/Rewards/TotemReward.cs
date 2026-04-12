// using System;
// using Fractural.Tasks;
// using Godot;
// using Newtonsoft.Json;
//
// [Serializable, JsonObject(MemberSerialization.OptIn)]
// public class TotemReward(Action<Obstacle> onTotemPlaced, Action<Obstacle> onTotemDestroyed, string name, Func<RichTextParameters, string> labelText)
// 	: Reward, IEventSubscriber
// {
// 	public override RewardType Type => RewardType.ScenarioStart;
//
// 	public override string GetLabelText(RichTextParameters textParameters) =>
// 		$"Once, during the next scenario, a character may place a {name} Totem obstacle in an adjacent empty hex during their turn. {labelText(textParameters)}";
//
// 	public override async GDTask OnScenarioSetupPhaseCompleted()
// 	{
// 		await base.OnScenarioSetupPhaseCompleted();
//
// 		AbilityCmd.SubscribeDuringCharacterTurn(this, EffectType.Selectable,
// 			character => true,
// 			async character =>
// 			{
// 				Hex hex = await AbilityCmd.SelectHex(GameController.Instance.CharacterManager.GetCharacter(0),
// 					list =>
// 					{
// 						foreach(Hex hex in RangeHelper.GetHexesInRange(character.Hex, 1, false))
// 						{
// 							if(hex.IsEmpty())
// 							{
// 								list.Add(hex);
// 							}
// 						}
// 					}, mandatory: true, hintText: $"Select a hex to place the {name} Totem"
// 				);
//
// 				if(hex != null)
// 				{
// 					Obstacle obstacle = await AbilityCmd.CreateOverlayTile<Obstacle>(hex,
// 						ResourceLoader.Load<PackedScene>("res://Content/OverlayTiles/Obstacles/Totem1H.tscn"));
// 					onTotemPlaced(obstacle);
//
// 					ScenarioCheckEvents.GenericInfoItemExtraEffectsCheckEvent.Subscribe(this,
// 						parameters => parameters.HexObject == obstacle,
// 						parameters =>
// 						{
// 							parameters.Add(new InfoTextExtraEffect.Parameters(labelText));
// 						}
// 					);
//
// 					ScenarioEvents.HexObjectDestroyedEvent.Subscribe(this,
// 						parameters => parameters.HexObject == obstacle,
// 						async parameters =>
// 						{
// 							ScenarioCheckEvents.GenericInfoItemExtraEffectsCheckEvent.Unsubscribe(this);
// 							ScenarioEvents.HexObjectDestroyedEvent.Unsubscribe(this);
//
// 							onTotemDestroyed?.Invoke(obstacle);
//
// 							await GDTask.CompletedTask;
// 						}
// 					);
// 				}
//
// 				AbilityCmd.UnsubscribeDuringTurn(this);
// 			}, new IconEffectButton.Parameters(Icons.Obstacle),
// 			new TextEffectInfoView.Parameters(
// 				$"Place a {name} Totem obstacle in an adjacent empty hex as a reward from the last Road Event.")
// 		);
// 	}
// }


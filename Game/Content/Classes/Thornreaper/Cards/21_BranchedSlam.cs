using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class BranchedSlam : ThornreaperCardModel<BranchedSlam.CardTop, BranchedSlam.CardBottom>
{
	public override string Name => "Branched Slam";
	public override int Level => 5;
	public override int Initiative => 45;
	protected override int AtlasIndex => 21;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.AfterAttackPerformedEvent.Subscribe(state, this,
						parameters => parameters.Performer == state.Performer && parameters.AbilityState.AbilityAOEPattern != null &&
						              parameters.AbilityState.GetRedAOEHexes().Contains(parameters.AbilityState.Target.Hex) &&
						              !parameters.AbilityState.GetCustomValue<bool>(this, "UsedBranchedSlam"),
						async parameters =>
						{
							AOEPrompt.Answer aoeAnswer =
								await PromptManager.Prompt(new AOEPrompt(state.Performer, new AOEPattern(
										[
											new AOEHex(Vector2I.Zero, AOEHexType.Gray),
											new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
											new AOEHex(Vector2I.Zero.Add(Direction.NorthEast).Add(Direction.East), AOEHexType.Red)
										]
									), parameters.AbilityState.Target.Hex, null, () => "Select hexes for additional area of effect hexes", 1),
									state.Authority);

							if(aoeAnswer.Skipped)
							{
								return;
							}

							foreach(AOEHex aoeHex in aoeAnswer.AOEHexes.Where(aoeHex => aoeHex.Type.HasFlag(AOEHexType.Red)))
							{
								parameters.AbilityState.TargetedAOEHexes.AddIfNew(aoeHex);
							}

							parameters.AbilityState.SetCustomValue(this, "UsedBranchedSlam", true);
						}, EffectType.Selectable,
						effectButtonParameters: new IconEffectButton.Parameters(Icons.GetAOEPattern(new AOEPattern(
							[
								new AOEHex(Vector2I.Zero, AOEHexType.Empty),
								new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
								new AOEHex(Vector2I.Zero.Add(Direction.NorthEast).Add(Direction.East), AOEHexType.Red)
							]
						))),
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"Add {Icons.Inline(Icons.GetAOEPattern(new AOEPattern(
							[
								new AOEHex(Vector2I.Zero, AOEHexType.Empty),
								new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
								new AOEHex(Vector2I.Zero.Add(Direction.NorthEast).Add(Direction.East), AOEHexType.Red)
							]
						)))} from the target"));

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.AfterAttackPerformedEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.Build())
		];

		public override int XP => 2;
		public override bool Persistent => true;
		public override bool Loss => true;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					await AbilityCmd.InfuseElement(state, Element.Earth);
					state.SetPerformed();
				})
				.WithConditionalAbilityCheck(async _ =>
				{
					await GDTask.CompletedTask;

					return GameController.Instance.ElementManager.GetState(Element.Earth) is ElementState.Strong or ElementState.Waning;
				})
				.Build()),
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3, new MoveSquare(this, new Vector2(0.6214308f, 0.8083103f)))
				.Build())
		];
	}
}
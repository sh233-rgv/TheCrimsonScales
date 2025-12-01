using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class GammaEnergy : LuminaryCardModel<GammaEnergy.CardTop, GammaEnergy.CardBottom>
{
	public override string Name => "Gamma Energy";
	public override int Level => 7;
	public override int Initiative => 65;
	protected override int AtlasIndex => 24;

	public class CardTop : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			Glow(new GlowAbilityModel([Element.Fire], GlowAbility,
				$"Perform {Icons.Inline(Icons.Damage)}2 ability", Icons.Damage))
		];
		
		protected override int XP => 1;
		protected override bool Persistent => true;

		protected Ability GlowAbility(List<Element> elements)
        {
            return OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					Dictionary<Vector2I, AOEHexType> aoeHexes = new Dictionary<Vector2I, AOEHexType>();

					AOEPrompt.Answer aoeAnswer =
						await PromptManager.Prompt(new AOEPrompt(state, new AOEPattern(
							[
								new AOEHex(Vector2I.Zero, AOEHexType.Gray),
								new AOEHex(Vector2I.Zero.Add(Direction.NorthWest), AOEHexType.Red),
								new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
								new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
							]
						), state.Performer.Hex, null, () => "Select the hexes for the area of effect"),
							state.Authority);

					if(aoeAnswer.Skipped)
					{
						return;
					}

					for(int i = 0; i < aoeAnswer.HexCoords.Count; i++)
					{
						aoeHexes.Add(aoeAnswer.HexCoords[i], aoeAnswer.HexTypes[i]);
					}

					foreach((Vector2I coords, AOEHexType type) in aoeHexes)
					{
						Hex hex = GameController.Instance.Map.GetHex(coords);

						if(hex != null && type.HasFlag(AOEHexType.Red))
						{
							foreach(Figure figure in hex.GetHexObjectsOfType<Figure>().Where(figure => figure.EnemiesWith(state.Performer)))
                            {
                                await AbilityCmd.SufferDamage(/*state*/null, figure, 2);
								//TODO: Change to state
                            }
						}
					}
					await GDTask.CompletedTask;
				})
				.WithOnAbilityStarted(async state =>
				{
					state.SetCustomValue(state.Performer, "Glow Ability", true);
					state.SetCustomValue(state.Performer, "Consumed Elements", elements);

					await GDTask.CompletedTask;
				})
				.Build();
		}
	}

	public class CardBottom : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					int consumedElements = 0;
					for(int i = 0; i < 6; i++)
					{
						if (await AbilityCmd.TryConsumeElement((Element)i))
                        {
							consumedElements++;
							state.SetPerformed();
                        }
					}
					state.SetCustomValue(this, "ConsumedElements", consumedElements);

					await GDTask.CompletedTask;
				})
				.Build()),
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(0)
				.WithTargets(0)
				.WithRange(3)
				.WithOnAbilityStarted(async state =>
                {
					int consumedElements = state.ActionState.GetAbilityState<OtherAbility.State>(0).GetCustomValue<int>(this, "ConsumedElements");
                    state.AbilityAdjustAttackValue(consumedElements);
					state.AdjustTargets(consumedElements);

					await GDTask.CompletedTask;
                })
				.Build()),
		];

		protected override int XP => 2;
		protected override bool Loss => true;
	}
}
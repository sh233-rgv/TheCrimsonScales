using System.Collections.Generic;
using Fractural.Tasks;

public class LightTheWay : LuminaryCardModel<LightTheWay.CardTop, LightTheWay.CardBottom>
{
	public override string Name => "Light the Way";
	public override int Level => 9;
	public override int Initiative => 93;
	protected override int AtlasIndex => 29;

	public class CardTop : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(GlowActiveAbility.Builder()
				.WithGlowAbility(new GlowAbilityModel([Element.Ice, Element.Dark], GlowAbility,
					"Summon Gleaming Squid", "Summon"))
				.Build())
		];

		protected override int XP => 1;
		protected override bool Persistent => true;

		private Ability GlowAbility(List<Element> elements)
		{
			return SummonAbility.Builder()
				.WithSummonStats(new SummonStats()
				{
					Health = 3,
					Move = 3,
					Attack = 2,
					Traits =
					[
						new PierceTrait(2),
						new InfuseElementAfterAttackTrait(Element.Light)
					]
				})
				.WithName("Gleaming Squid")
				.WithTexturePath("res://Content/Classes/Luminary/GleamingSquid.png")
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
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.FigureTurnEndedEvent.Subscribe(state, this,
						parameters => parameters.Figure == state.Performer && parameters.Figure.TurnMovedHexes.Count >= 4,
						async parameters =>
						{
							await AbilityCmd.InfuseElement(state, [Element.Fire, Element.Light]);
						}, EffectType.Selectable,
						effectButtonParameters: new TextEffectButton.Parameters(
							$"{Icons.Inline(Icons.GetElement(Element.Fire))} or {Icons.Inline(Icons.GetElement(Element.Light))}"),
						effectInfoViewParameters: new TextEffectInfoView.Parameters(
							$"{Icons.Inline(Icons.GetElement(Element.Fire))} or {Icons.Inline(Icons.GetElement(Element.Light))}"));

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.FigureTurnEndedEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.Build())
		];

		//TODO: protected override IEnumerable<Element> Elements => [Wild Element];
		protected override int XP => 2;
		protected override bool Persistent => true;
		public override bool Loss => true;
	}
}
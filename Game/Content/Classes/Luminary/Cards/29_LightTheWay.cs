using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class LightTheWay : LuminaryCardModel<LightTheWay.CardTop, LightTheWay.CardBottom>
{
	public override string Name => "Light the Way";
	public override int Level => 1;
	public override int Initiative => 93;
	protected override int AtlasIndex => 29;

	public class CardTop : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			Glow(new GlowAbilityModel([Element.Ice, Element.Dark], GlowAbility,
				$"Summon Gleaming Squid", "Summon"))
		];

		protected override int XP => 1;
		protected override bool Persistent => true;

		protected Ability GlowAbility(List<Element> elements)
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
						//new InfuseElementAfterAttackTrait(Element.Light)
						//TODO: Element on attack (requires FireKnightL9)
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
						parameters => parameters.Figure == state.Performer && parameters.Figure.TurnMovedHexCount >= 4,
						async parameters =>
                        {
							await AbilityCmd.InfuseElement([Element.Fire, Element.Light], parameters.Figure, state);
                        }, EffectType.Selectable,
						effectButtonParameters: new TextEffectButton.Parameters($"{Icons.Inline(Icons.GetElement(Element.Fire))} or {Icons.Inline(Icons.GetElement(Element.Light))}"),
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.GetElement(Element.Fire))} or {Icons.Inline(Icons.GetElement(Element.Light))}"));

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
		protected override bool Loss => true;
	}
}
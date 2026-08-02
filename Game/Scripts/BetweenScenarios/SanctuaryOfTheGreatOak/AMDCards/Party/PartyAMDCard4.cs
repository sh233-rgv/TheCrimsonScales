using System;
using Fractural.Tasks;

public class PartyAMDCard4 : PartyAMDCardModel
{
	public override string GetSimpleString(RichTextParameters richTextParameters) =>
		GetSimpleString(richTextParameters, +0,
			$"{Icons.Inline(Icons.Move, richTextParameters)}2 {Icons.Inline(Icons.Rolling, richTextParameters)}");

	public override string ToString(RichTextParameters richTextParameters) =>
		GetBasicString(richTextParameters, +0,
			extraText: $"Perform {Icons.Inline(Icons.Move, richTextParameters)}2 at the end of your turn",
			rolling: true);

	protected override int AtlasIndex => 12;

	public override Func<AttackAbility.State, Figure, GDTask> GetExtraEffects() =>
		async (state, _) =>
		{
			ScenarioEvents.FigureTurnEndingEvent.Subscribe(state, this,
				parameters => parameters.Figure == state.Performer,
				async parameters =>
				{
					ScenarioEvents.FigureTurnEndingEvent.Unsubscribe(state, this);

					ActionState actionState = new ActionState(state.Performer, [MoveAbility.Builder().WithDistance(2).Build()]);
					await actionState.Perform();
				}
			);

			await GDTask.CompletedTask;
		};
}
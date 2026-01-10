public static class Enhancements
{
	public static EnhancementModel[] NegativeConditions { get; } =
	[
		ModelDB.Enhancement<PoisonEnhancement>(),
		ModelDB.Enhancement<WoundEnhancement>(),
		ModelDB.Enhancement<MuddleEnhancement>(),
		ModelDB.Enhancement<ImmobilizeEnhancement>(),
		ModelDB.Enhancement<CurseEnhancement>()
	];

	public static EnhancementModel[] Elements { get; } =
	[
		//TODO
	];
}
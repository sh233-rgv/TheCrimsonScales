public static class Enhancements
{
	// public static PoisonEnhancement Poison { get; } = ModelDB.Enhancement<PoisonEnhancement>();
	// public static WoundEnhancement Wound { get; } = ModelDB.Enhancement<WoundEnhancement>();
	// public static MuddleEnhancement Muddle { get; } = ModelDB.Enhancement<MuddleEnhancement>();
	// public static ImmobilizeEnhancement Immobilize { get; } = ModelDB.Enhancement<ImmobilizeEnhancement>();
	// public static CurseEnhancement Curse { get; } = ModelDB.Enhancement<CurseEnhancement>();

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
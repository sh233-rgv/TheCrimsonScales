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

	public static EnhancementModel[] PositiveConditions { get; } =
	[
		ModelDB.Enhancement<BlessEnhancement>(),
		ModelDB.Enhancement<RegenerateEnhancement>(),
		ModelDB.Enhancement<StrengthenEnhancement>(),
		ModelDB.Enhancement<WardEnhancement>(),
	];

	public static EnhancementModel[] Elements { get; } =
	[
		ModelDB.Enhancement<FireElementEnhancement>(),
		ModelDB.Enhancement<IceElementEnhancement>(),
		ModelDB.Enhancement<AirElementEnhancement>(),
		ModelDB.Enhancement<EarthElementEnhancement>(),
		ModelDB.Enhancement<LightElementEnhancement>(),
		ModelDB.Enhancement<DarkElementEnhancement>(),
		ModelDB.Enhancement<WildElementEnhancement>(),
	];
}
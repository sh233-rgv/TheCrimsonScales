public static class Enhancements
{
	public static PlusOneAttackEnhancement PlusOneAttack { get; } = ModelDB.Enhancement<PlusOneAttackEnhancement>();

	public static PoisonEnhancement Poison { get; } = ModelDB.Enhancement<PoisonEnhancement>();
	public static WoundEnhancement Wound { get; } = ModelDB.Enhancement<WoundEnhancement>();
	public static MuddleEnhancement Muddle { get; } = ModelDB.Enhancement<MuddleEnhancement>();
	public static ImmobilizeEnhancement Immobilize { get; } = ModelDB.Enhancement<ImmobilizeEnhancement>();
	public static CurseEnhancement Curse { get; } = ModelDB.Enhancement<CurseEnhancement>();

	public static EnhancementModel[] NegativeConditions { get; } =
	[
		Poison,
		Wound,
		Muddle,
		Immobilize,
		Curse
	];

	public static EnhancementModel[] Elements { get; } =
	[
		//TODO
	];
}
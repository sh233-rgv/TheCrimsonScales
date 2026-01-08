using System.Collections.Generic;
using Godot;

public class FireKnightModel : ClassModel
{
	public override string Name => "Fire Knight";
	public override MaxHealthValues MaxHealthValues => MaxHealthValues.MediumHigh;
	public override int HandSize => 10;
	public override Ancestry Ancestry => Ancestry.Valrath;

	public override string AssetPath => "res://Content/Classes/FireKnight";
	public override Color PrimaryColor => Color.FromHtml("df391f");
	public override Color SecondaryColor => Color.FromHtml("531724");

	public override PackedScene Scene => ResourceLoader.Load<PackedScene>($"{AssetPath}/FireKnight.tscn");

	public override IList<AbilityCardModel> AbilityCards { get; } =
	[
		ModelDB.AbilityCard<ForcibleEntry>(),
		ModelDB.AbilityCard<BackupSupport>(),
		ModelDB.AbilityCard<CollectiveCombat>(),
		ModelDB.AbilityCard<ControlledAggression>(),
		ModelDB.AbilityCard<RapidRescue>(),
		ModelDB.AbilityCard<PlayingWithFire>(),
		ModelDB.AbilityCard<FireWhirl>(),
		ModelDB.AbilityCard<LightIrons>(),
		ModelDB.AbilityCard<FieldMedic>(),
		ModelDB.AbilityCard<FierceLeader>(),

		ModelDB.AbilityCard<CoordinatedAttack>(),
		ModelDB.AbilityCard<LoyalCompanion>(),
		ModelDB.AbilityCard<ProtectiveInstinct>(),

		ModelDB.AbilityCard<HeavyIrons>(),
		ModelDB.AbilityCard<TraumaCare>(),
		ModelDB.AbilityCard<LadderAssault>(),
		ModelDB.AbilityCard<CrewIntegrity>(),
		ModelDB.AbilityCard<JackOfAllTrades>(),
		ModelDB.AbilityCard<ForgedByFire>(),
		ModelDB.AbilityCard<HookAndLadder>(),
		ModelDB.AbilityCard<SearingBlaze>(),
		ModelDB.AbilityCard<SpontaneousCombustion>(),
		ModelDB.AbilityCard<MutualAid>(),
		ModelDB.AbilityCard<RollingFlames>(),
		ModelDB.AbilityCard<SearchAndRescue>(),
		ModelDB.AbilityCard<Backdraft>(),
		ModelDB.AbilityCard<FightTogether>(),
		ModelDB.AbilityCard<Flashover>(),
		ModelDB.AbilityCard<IncidentCommander>(),
	];

	public IList<ItemModel> AllItems { get; } =
	[
		ModelDB.Item<EmberCladding>(),
		ModelDB.Item<RescueShield>(),
		ModelDB.Item<RescueAxe>(),
		ModelDB.Item<PikeHook>(),
		ModelDB.Item<FireproofHelm>(),
		ModelDB.Item<KindledTonic>(),
		ModelDB.Item<ExplosiveTonic>(),
		ModelDB.Item<ScrollOfCharisma>(),
		ModelDB.Item<ScrollOfProtection>(),
		ModelDB.Item<ScrollOfInvigoration>(),
	];
}
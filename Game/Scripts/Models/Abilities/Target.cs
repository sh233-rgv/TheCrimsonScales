using System;

[Flags]
public enum Target
{
	Self = 1,
	Allies = 2,
	Enemies = 4,

	TargetAll = 8,
	MustTargetCharacters = 16,
	SelfCountsForTargets = 32,

	MustTargetSameWithAllTargets = 64,

	SelfOrAllies = Self | Allies,
	//SelfAndAllies = Self | Allies | SelfCountsForTargets,
	Any = Self | Allies | Enemies,
}
using System.Collections.Generic;

public partial class Hierophant : Character
{
	private HierophantModel _hierophantModel;

	public List<AbilityCard> PrayerCards { get; } = new List<AbilityCard>();

	public override void Spawn(SavedCharacter savedCharacter, int index)
	{
		base.Spawn(savedCharacter, index);

		_hierophantModel = (HierophantModel)savedCharacter.ClassModel;

		// Copy over all prayer cards from the character
		foreach(AbilityCardModel prayerCard in _hierophantModel.AllPrayerCards)
		{
			AbilityCard abilityCard = new AbilityCard(new SavedAbilityCard(prayerCard), this);
			PrayerCards.Add(abilityCard);
		}
	}
}
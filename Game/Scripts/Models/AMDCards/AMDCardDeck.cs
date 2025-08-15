using System.Collections.Generic;

public class AMDCardDeck : CardDeck<AMDCard>
{
	public bool CharacterDeck { get; }

	public AMDCardDeck(IEnumerable<AMDCard> cards, bool characterDeck)
		: base(cards)
	{
		CharacterDeck = characterDeck;
	}

	public static List<AMDCard> GetDefaultDeckCards(string textureAtlasPath)
	{
		return new List<AMDCard>()
		{
			new BasicAMDCard(textureAtlasPath, 0, 4, 5, 0),
			new BasicAMDCard(textureAtlasPath, 1, 4, 5, 0),
			new BasicAMDCard(textureAtlasPath, 2, 4, 5, 0),
			new BasicAMDCard(textureAtlasPath, 3, 4, 5, 0),
			new BasicAMDCard(textureAtlasPath, 4, 4, 5, 0),
			new BasicAMDCard(textureAtlasPath, 5, 4, 5, 0),

			new BasicAMDCard(textureAtlasPath, 6, 4, 5, 1),
			new BasicAMDCard(textureAtlasPath, 7, 4, 5, 1),
			new BasicAMDCard(textureAtlasPath, 8, 4, 5, 1),
			new BasicAMDCard(textureAtlasPath, 9, 4, 5, 1),
			new BasicAMDCard(textureAtlasPath, 10, 4, 5, 1),

			new BasicAMDCard(textureAtlasPath, 11, 4, 5, -1),
			new BasicAMDCard(textureAtlasPath, 12, 4, 5, -1),
			new BasicAMDCard(textureAtlasPath, 13, 4, 5, -1),
			new BasicAMDCard(textureAtlasPath, 14, 4, 5, -1),
			new BasicAMDCard(textureAtlasPath, 15, 4, 5, -1),

			new BasicAMDCard(textureAtlasPath, 16, 4, 5, -2),
			new BasicAMDCard(textureAtlasPath, 17, 4, 5, 2),

			new NullAMDCard(textureAtlasPath, 18, 4, 5),
			new CritAMDCard(textureAtlasPath, 19, 4, 5),
		};
	}

	public void AddMinusOne()
	{
		AddCard(new BasicAMDCard($"res://Art/AMDs/Other.jpg", 4, 4, 2, -1), true);
	}
}
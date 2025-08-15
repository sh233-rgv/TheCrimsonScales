using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class PortraitView : Control
{
	[Export]
	private PackedScene _characterPortraitScene;
	[Export]
	private PackedScene _monsterGroupPortraitScene;
	[Export]
	private Control _portraitParent;

	public List<PortraitViewPortrait> Portraits { get; } = new List<PortraitViewPortrait>();
	public List<PortraitViewCharacterPortrait> CharacterPortraits { get; } = new List<PortraitViewCharacterPortrait>();
	public List<PortraitViewMonsterGroupPortrait> MonsterGroupPortraits { get; } = new List<PortraitViewMonsterGroupPortrait>();

	public void Open()
	{
		GameController.Instance.Map.FigureAddedEvent += OnFigureAdded;
		GameController.Instance.Map.FigureRemovedEvent += OnFigureRemoved;

		foreach(Figure figure in GameController.Instance.Map.Figures)
		{
			OnFigureAdded(figure);
		}
	}

	public void Close()
	{
		foreach(PortraitViewPortrait portrait in Portraits)
		{
			portrait.Destroy();
		}

		Portraits.Clear();

		GameController.Instance.Map.FigureAddedEvent -= OnFigureAdded;
		GameController.Instance.Map.FigureRemovedEvent -= OnFigureRemoved;
	}

	public void Reorder()
	{
		Portraits.Sort((portraitA, portraitB) =>
		{
			return portraitA.Initiative.SortingInitiative.CompareTo(portraitB.Initiative.SortingInitiative);

			// Initiative initiativeA = portraitA.Initiative;
			// Initiative initiativeB = portraitB.Initiative;

			// if(initiativeA.Equals(initiativeB))
			// {
			// 	initiativeA = portraitA is PortraitViewCharacterPortrait ? 0 : 1;
			// 	initiativeB = portraitB is PortraitViewCharacterPortrait ? 0 : 1;
			// }
			// else
			// {
			// 	if(!initiativeA.HasValue)
			// 	{
			// 		initiativeA = 1000;
			// 	}
			//
			// 	if(!initiativeB.HasValue)
			// 	{
			// 		initiativeB = 1000;
			// 	}
			// }
			//
			// if(initiativeA == initiativeB && portraitA is PortraitViewCharacterPortrait characterPortraitA && portraitB is PortraitViewCharacterPortrait characterPortraitB)
			// {
			// 	return characterPortraitA.Character.Index.CompareTo(characterPortraitB.Character.Index);
			// }
			//
			// return initiativeA.Value.CompareTo(initiativeB.Value);
		});

		const float separation = 20f;

		for(int i = 0; i < Portraits.Count; i++)
		{
			PortraitViewPortrait portrait = Portraits[i];

			float portraitWidth = portrait.Size.X;
			float parentWidth = _portraitParent.Size.X;

			float totalWidth = (portraitWidth + separation) * Portraits.Count - separation;
			float totalLeftAnchor = (parentWidth - totalWidth) / 2f;

			float pos = totalLeftAnchor + (portraitWidth + separation) * i;
			portrait.Move(new Vector2(pos, portrait.Position.Y));
		}
	}

	private void OnFigureAdded(Figure figure)
	{
		if(figure is Monster monster)
		{
			PortraitViewMonsterGroupPortrait portrait = MonsterGroupPortraits.FirstOrDefault(portrait => portrait.MonsterGroup == monster.MonsterGroup);
			if(portrait == null)
			{
				portrait = _monsterGroupPortraitScene.Instantiate<PortraitViewMonsterGroupPortrait>();
				_portraitParent.AddChild(portrait);
				portrait.Init(monster.MonsterGroup);
				Portraits.Add(portrait);
				MonsterGroupPortraits.Add(portrait);

				Reorder();
			}
		}
		else if(figure is Character character)
		{
			PortraitViewCharacterPortrait portrait = _characterPortraitScene.Instantiate<PortraitViewCharacterPortrait>();
			_portraitParent.AddChild(portrait);
			portrait.Init(character);
			Portraits.Add(portrait);
			CharacterPortraits.Add(portrait);

			Reorder();
		}
	}

	private void OnFigureRemoved(Figure figure)
	{
		if(figure is Monster monster)
		{
			if(monster.MonsterGroup.Monsters.Count == 0)
			{
				PortraitViewMonsterGroupPortrait portrait = MonsterGroupPortraits.FirstOrDefault(portrait => portrait.MonsterGroup == monster.MonsterGroup);
				if(portrait != null)
				{
					Portraits.Remove(portrait);
					MonsterGroupPortraits.Remove(portrait);
					portrait.Destroy();

					Reorder();
				}
			}
		}
		else if(figure is Character character)
		{
			PortraitViewCharacterPortrait portrait = CharacterPortraits.FirstOrDefault(portrait => portrait.Character == character);

			if(portrait != null)
			{
				Portraits.Remove(portrait);
				CharacterPortraits.Remove(portrait);
				portrait.Destroy();

				Reorder();
			}
		}
	}
}
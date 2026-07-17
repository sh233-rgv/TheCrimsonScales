using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class AMDViewer : PopupPanel
{
	[Export]
	private GridContainer _grid;

	[Export]
	private PackedScene _amdViewerBox;

	public AMDCardDeck AMDCardDeck;

	public override void _Ready()
	{
		base._Ready();

		Unfocusable = true;
		
		Hide();
	}

	public void UpdateDeck()
	{
		foreach(Node child in _grid.GetChildren())
		{
			child.QueueFree();
		}

		Dictionary<AMDCardModel, int> cardCounts = new Dictionary<AMDCardModel, int>();

		foreach(AMDCardModel amdModel in AMDCardDeck.DrawPile.Select(amd => amd.Model))
		{
			if(cardCounts.ContainsKey(amdModel))
			{
				cardCounts[amdModel]++;
			}
			else
			{
				cardCounts.Add(amdModel, 1);
			}
		}

		List<KeyValuePair<AMDCardModel, int>> sortedCards = cardCounts
			.OrderBy(kvp => kvp.Key.Type switch
			{
				AMDCardType.Null => 0,
				AMDCardType.Value => 1,
				AMDCardType.Crit => 2,
				_ => 3
			})
			.ThenBy(kvp => kvp.Key.GetValue(null) ?? 0)
			.ThenBy(kvp => kvp.Key.ToString())
			.ToList();

		foreach(KeyValuePair<AMDCardModel, int> cardCount in sortedCards)
		{
			AMDViewerBox box = _amdViewerBox.Instantiate<AMDViewerBox>();
			box.SetAMD(cardCount.Key, cardCount.Value);
			_grid.AddChild(box);
		}
	}
}
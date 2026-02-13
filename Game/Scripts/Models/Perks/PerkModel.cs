using System;
using System.Collections.Generic;
using System.Linq;

public abstract class PerkModel : AbstractModel
{
	public virtual string ToString(RichTextParameters richTextParameters)
	{
		string returnValue = string.Empty;

		if(IgnoreNegativeScenarioEffects)
		{
			returnValue += "ignore negative scenario effects";
		}

		if(IgnoreNegativeItemEffects)
		{
			if(!string.IsNullOrEmpty(returnValue))
			{
				returnValue += " and ";
			}

			returnValue += "ignore negative item effects";
		}

		if(CardsToRemove.Count > 0)
		{
			if(!string.IsNullOrEmpty(returnValue))
			{
				returnValue += " and ";
			}

			if(CardsToAdd.Count > 0)
			{
				returnValue += "replace ";
			}
			else
			{
				returnValue += "remove ";
			}

			returnValue += GetCardsString(CardsToRemove, richTextParameters);

			if(CardsToAdd.Count > 0)
			{
				returnValue += $" with {GetCardsString(CardsToAdd, richTextParameters)}";
			}
		}
		else if(CardsToAdd.Count > 0)
		{
			if(!string.IsNullOrEmpty(returnValue))
			{
				returnValue += " and ";
			}

			returnValue += $"add {GetCardsString(CardsToAdd, richTextParameters)}";
		}

		returnValue = string.Concat(returnValue[0].ToString().ToUpper(), returnValue.AsSpan(1));
		return returnValue;
	}

	public virtual List<AMDCardModel> CardsToRemove => [];
	public virtual List<AMDCardModel> CardsToAdd => [];

	public virtual bool IgnoreNegativeScenarioEffects => false;
	public virtual bool IgnoreNegativeItemEffects => false;

	protected string GetNonAMDString(string title, string description, RichTextParameters richTextParameters)
	{
		return $"[b]{title}:[/b] {description}";
	}

	private string GetCardsString(List<AMDCardModel> cards, RichTextParameters richTextParameters)
	{
		string returnValue = string.Empty;

		IEnumerable<IGrouping<AMDCardModel, AMDCardModel>> amdCardGroups = cards.GroupBy(perkModel => perkModel);
		foreach(IGrouping<AMDCardModel, AMDCardModel> amdCardGroup in amdCardGroups)
		{
			int count = amdCardGroup.Count();
			string number;
			switch(count)
			{
				case 1:
					number = "one";
					break;
				case 2:
					number = "two";
					break;
				case 3:
					number = "three";
					break;
				case 4:
					number = "four";
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(count), count, null);
			}

			returnValue += $"{number} {amdCardGroup.Key.ToString(richTextParameters)} ";
			returnValue += count > 1 ? "cards" : "card";
		}

		return returnValue;
	}
}
using System.Text.RegularExpressions;

public static class SlugHelper
{
	private static readonly Regex CamelCaseRegex = new Regex("([A-Za-z0-9]|\\G(?!^))([A-Z])");
	private static readonly Regex WhitespaceRegex = new Regex("\\s+");
	//private static readonly Regex SpecialCharRegex = new Regex("[^A-Z_]");
	private static readonly Regex SnakeCaseRegex = new Regex("(.*?)_([a-zA-Z0-9])");

	public static string GetSlug(string text)
	{
		text = CamelCaseRegex.Replace(text.Trim(), "$1_$2");
		text = WhitespaceRegex.Replace(text.ToUpper(), "_");
		return text;
		//return SpecialCharRegex.Replace(text, string.Empty);
	}

	public static string GetUnslug(string slug)
	{
		string str1 = SnakeCaseRegex.Replace(slug.Trim().ToLower(), (MatchEvaluator)(match => match.Groups[1].ToString() + match.Groups[2].ToString().ToUpper()));
		string str2 = char.ToUpper(str1[0]).ToString();
		string str3 = str1;
		string str4 = str3.Substring(1, str3.Length - 1);
		return str2 + str4;
	}
}
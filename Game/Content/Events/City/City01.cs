public class City01 : EventModel<City01.ChoiceA, City01.ChoiceB>
{
	public override int Number => 01;

	public override string Text =>
		"""
		"Come one, come all, and welcome to the county fair!" a Quatryl with red-and-white facepaint and a clownish blue wig smiles as he waves you in through the entrance. You've decided to take the day off and visit the county fair, which you've enjoyed frequenting as a youth.

		"Step right up and try your luck!" an Inox strongman wielding a giant hammer beckons you forward. "Do you have what it takes to hit the bell?"

		On the other side, an Aesther throws a dart and pops a balloon. "Try your aim! Can you hit the balloon? Find out here!"
		""";

	public class ChoiceA : EventChoiceModel
	{
		public override string Text => "Blah blah";
	}

	public class ChoiceB : EventChoiceModel
	{
		public override string Text => "Blah blah 2";
	}
}
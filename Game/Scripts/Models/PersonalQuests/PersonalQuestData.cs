public class PersonalQuestData
{
	public int Progress { get; private set; }

	public void AdjustProgress(int value)
	{
		Progress += value;
	}
}
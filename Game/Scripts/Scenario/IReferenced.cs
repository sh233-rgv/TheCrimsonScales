public interface IReferenced
{
	int ReferenceId { get; set; }
}

public static class IReferencedExtensions
{
	public static void InitReference(this IReferenced referenced)
	{
		GameController.Instance.ReferenceManager.Register(referenced);
	}
}
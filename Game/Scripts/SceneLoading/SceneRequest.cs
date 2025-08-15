public abstract class SceneRequest
{
	public abstract string ScenePath { get; }

	//public virtual string LoadingScenePath => "res://Scenes/LoadingScreen.tscn";

	public bool IsFinished { get; private set; }

	public void Finish()
	{
		IsFinished = true;
	}
}
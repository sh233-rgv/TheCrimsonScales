using System;
using System.Threading;
using Fractural.Tasks;
using Godot;

public partial class SceneLoader : Node
{
	// private const bool UseThreads = false;

	[Export]
	private PackedScene _loadingScene;

	public SceneRequest CurrentSceneRequest { get; private set; }

	public bool IsTransitioning => CurrentSceneRequest != null && !CurrentSceneRequest.IsFinished;

	public bool RequestSceneChange(SceneRequest sceneRequest)
	{
		if(IsTransitioning)
		{
			return false;
		}

		CurrentSceneRequest = sceneRequest;

		ChangeScene().Forget();
		return true;
	}

	private async GDTaskVoid ChangeScene()
	{
		CancellationToken cancellationToken = AppController.Instance.DestroyCancellationToken;

		AppController.Instance.PopupManager.CloseAll();

		Node currentScene = GetTree().CurrentScene;

		// Add loading scene
		LoadingSceneController loadingSceneController = _loadingScene.Instantiate<LoadingSceneController>();
		GetTree().Root.AddChild(loadingSceneController);

		await loadingSceneController.FadeIn(cancellationToken);

		// Remove current scene
		currentScene.QueueFree();

		await GDTask.WaitWhile(() => AppController.Instance.PopupManager.IsPopupOpen(), cancellationToken: cancellationToken);

		await GDTask.Yield(cancellationToken);
		await GDTask.Yield(cancellationToken);

		// Add new scene
		PackedScene packedScene;

		// if(UseThreads)
		// {
		// 	Error error = ResourceLoader.LoadThreadedRequest(CurrentSceneRequest.ScenePath);
		//
		// 	Log.Write($"Sceneloader step 0: {error}");
		//
		// 	int maxTryCount = 3;
		// 	for(int i = 0; i < maxTryCount; i++)
		// 	{
		// 		GDTask loadThreadedTask = GDTask.WaitUntil(() => ResourceLoader.LoadThreadedGetStatus(CurrentSceneRequest.ScenePath) != ResourceLoader.ThreadLoadStatus.InProgress, cancellationToken: cancellationToken);
		// 		GDTask timerTask = GDTask.Delay(10f, cancellationToken: cancellationToken);
		// 		int winIndex = await GDTask.WhenAny(loadThreadedTask, timerTask);
		//
		// 		if(winIndex == 1)
		// 		{
		// 			Godot.Collections.Array progressArray = new Godot.Collections.Array();
		// 			Log.Write("Sceneloader failed! Timed out.");
		// 			Log.Write($"Sceneloader Thread Load Status: {ResourceLoader.LoadThreadedGetStatus(CurrentSceneRequest.ScenePath, progressArray)}, {progressArray[0]}");
		//
		// 			try
		// 			{
		// 				packedScene = ResourceLoader.LoadThreadedGet(CurrentSceneRequest.ScenePath) as PackedScene;
		//
		// 				Log.Write(packedScene?.ToString());
		// 			}
		// 			catch(Exception exception)
		// 			{
		// 				Log.Error(exception);
		// 				throw;
		// 			}
		// 		}
		// 	}
		//
		// 	Log.Write($"Sceneloader Thread Load Status: {ResourceLoader.LoadThreadedGetStatus(CurrentSceneRequest.ScenePath)}");
		//
		// 	Log.Write("Sceneloader step 1");
		// 	packedScene = (PackedScene)ResourceLoader.LoadThreadedGet(CurrentSceneRequest.ScenePath);
		// }
		// else
		{
			Log.Write($"Sceneloader step 0");
			packedScene = ResourceLoader.Load<PackedScene>(CurrentSceneRequest.ScenePath);
			Log.Write($"Sceneloader step 1");
		}

		Node newScene = packedScene.Instantiate();
		GetTree().Root.AddChild(newScene);
		GetTree().CurrentScene = newScene;

		Log.Write("Sceneloader step 2");
		GC.Collect();

		Log.Write("Sceneloader step 3");
		await GDTask.Yield(cancellationToken);
		await GDTask.Yield(cancellationToken);
		Log.Write("Sceneloader step 4");
		await GDTask.WaitUntil(() => ((ISceneController)newScene).AdditionalLoadingCompleted, cancellationToken: cancellationToken);
		Log.Write("Sceneloader step 5");
		await GDTask.Yield(cancellationToken);
		await GDTask.Yield(cancellationToken);

		Log.Write("Sceneloader step 6");
		await loadingSceneController.FadeOut(cancellationToken);
		Log.Write("Sceneloader step 7");
		loadingSceneController.QueueFree();

		CurrentSceneRequest.Finish();
	}

	//private Array _progressArray = new Array();

	// public override void _Process(double delta)
	// {
	// 	base._Process(delta);
	//
	// 	if(CurrentSceneRequest != null && !CurrentSceneRequest.IsFinished)
	// 	{
	// 		ResourceLoader.LoadThreadedGetStatus(CurrentSceneRequest.ScenePath, _progressArray);
	// 		GD.Print(_progressArray[0]);
	// 	}
	// }
}
using System;
using System.IO;
using Fractural.Tasks;
using Godot;

public partial class OverlayTileCreator : Node2D
{
	[Export]
	private Camera2D _camera;

	[Export]
	private Texture2D[] _2HTextures;
	[Export]
	private Sprite2D _2HSprite;

	public override void _Ready()
	{
		base._Ready();

		CreateTextures();
	}

	private async void CreateTextures()
	{
		try
		{
			await GDTask.Delay(1f);

			await CreateTextures(_2HTextures, _2HSprite, 512, 256);
		}
		catch(Exception e)
		{
			Log.Error(e);
		}
	}

	private async GDTask CreateTextures(Texture2D[] textures, Sprite2D sprite, int width, int height)
	{
		GetWindow().SetSize(new Vector2I(width, height));
		_camera.Zoom = (1080f / height) * Vector2.One;
		const string texturesPath = "res://../Game/Art/OverlayTiles/";
		string globalPath = ProjectSettings.GlobalizePath(texturesPath);
		if(!Directory.Exists(globalPath))
		{
			Directory.CreateDirectory(globalPath);
		}

		foreach(Texture2D texture in textures)
		{
			sprite.Texture = texture;

			await GDTask.Delay(0.1f);

			string fileName = Path.GetFileName(texture.ResourcePath);

			Viewport viewport = GetViewport();
			ViewportTexture viewportTexture = viewport.GetTexture();
			Image image = viewportTexture.GetImage();
			image.SavePng($"{texturesPath}{fileName}");
		}
	}
}
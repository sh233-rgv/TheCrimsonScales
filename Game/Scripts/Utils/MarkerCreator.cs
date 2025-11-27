using System;
using System.IO;
using Fractural.Tasks;
using Godot;
using Godot.Collections;

public partial class MarkerCreator : Node2D
{
	[Export]
	private Label _label;
	[Export]
	private int _spriteSize;

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

			const string screenshotPath = "res://../Game/Art/Markers/";
			string globalPath = ProjectSettings.GlobalizePath(screenshotPath);
			if(!Directory.Exists(globalPath))
			{
				Directory.CreateDirectory(globalPath);
			}

			LabelSettings labelSettings = _label.LabelSettings;
			Font font = labelSettings.Font;
			int fontSize = labelSettings.FontSize;
			Array<Rid> rids = font.GetRids();
			Rid fontRid = rids[0];
			TextServer ts = TextServerManager.GetPrimaryInterface();

			double ascent = ts.FontGetAscent(fontRid, fontSize);
			double descent = ts.FontGetDescent(fontRid, fontSize);
			double baselineCenter = (ascent - descent) * 0.5f;

			GetWindow().SetSize(_spriteSize * Vector2I.One);

			foreach(Marker.Type type in Enum.GetValues<Marker.Type>())
			{
				string markerText = type.ToString().Replace("_", string.Empty);
				_label.Text = markerText;

				int codepoint = char.ConvertToUtf32(markerText, 0);
				long glyphIndex = ts.FontGetGlyphIndex(fontRid, fontSize, codepoint, 0);
				Vector2 glyphSize = ts.FontGetGlyphSize(fontRid, new Vector2I(fontSize, fontSize), glyphIndex);
				Vector2 glyphOffset = ts.FontGetGlyphOffset(fontRid, new Vector2I(fontSize, fontSize), glyphIndex);

				float visualCenter = glyphOffset.Y + glyphSize.Y * 0.5f;

				float offset = (float)baselineCenter - visualCenter;
				_label.SetPosition(new Vector2(_label.Position.X, -_label.Size.Y + offset - 70));

				await GDTask.Delay(0.1f);

				Viewport viewport = GetViewport();
				ViewportTexture viewportTexture = viewport.GetTexture();
				Image image = viewportTexture.GetImage();
				image.SavePng($"{screenshotPath}{markerText}.png");
			}
		}
		catch(Exception e)
		{
			Log.Error(e);
		}
	}
}
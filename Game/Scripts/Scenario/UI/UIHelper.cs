using Godot;

public static class UIHelper
{
	private static readonly Color LostColor = Color.FromHtml("f8625a");

	public static void SetCardMaterial(TextureRect textureRect, CardState cardState)
	{
		float grayscaleFactor = 0f;
		Color modulateColor = Colors.White;
		const float persistentGrayscaleFactor = -1.5f;
		switch(cardState)
		{
			case CardState.PersistentLoss:
				grayscaleFactor = persistentGrayscaleFactor;
				break;
			case CardState.Persistent:
				grayscaleFactor = persistentGrayscaleFactor;
				break;
			case CardState.RoundLoss:
				grayscaleFactor = persistentGrayscaleFactor;
				break;
			case CardState.Round:
				grayscaleFactor = persistentGrayscaleFactor;
				break;
			case CardState.Playing:
				grayscaleFactor = 1f;
				break;
			case CardState.Hand:
				grayscaleFactor = 0f;
				break;
			case CardState.Discarded:
				grayscaleFactor = 1f;
				break;
			case CardState.Lost:
				grayscaleFactor = 1f;
				modulateColor = LostColor;
				break;
			default:
				grayscaleFactor = 1f;
				modulateColor = LostColor;
				break;
		}

		textureRect.SetInstanceShaderParameter("grayscaleFactor", grayscaleFactor);
		textureRect.SelfModulate = modulateColor;
	}
}
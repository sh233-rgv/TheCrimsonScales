using Godot;

public partial class GenericMonsterSpriteComponent : HexObjectViewComponent
{
	[Export]
	private Sprite2D _sprite2D;

	public override void Init(HexObject hexObject)
	{
		base.Init(hexObject);

		Texture2D texture = ResourceLoader.Load<Texture2D>(((Monster)hexObject).MonsterModel.MapIconTexturePath);
		_sprite2D.Texture = texture;

		if(texture != null)
		{
			float textureWidth = texture.GetWidth();
			_sprite2D.Scale = (330f / textureWidth) * Vector2.One;
		}
	}
}
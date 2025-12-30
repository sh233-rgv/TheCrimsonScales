using System.Threading;
using Fractural.Tasks;
using Godot;

public partial class UnlockCharacterController : Node
{
	[Export]
	private Tuckbox _tuckbox;
	[Export]
	private BetterButton _tuckboxButton;
	[Export]
	private Sprite3D[] _classIconSprites;

	[Export]
	private Control _characterMatContainer;
	[Export]
	private TextureRect _characterMatTextureRect;

	private bool _buttonPressed;

	public override void _Ready()
	{
		base._Ready();

		_tuckboxButton.Pressed += OnTuckboxPressed;

		//this.DelayedCall(() => Open(ModelDB.Class<MirefootModel>()), 2f);
	}

	public void Open(ClassModel classModel)
	{
		foreach(Sprite3D classIconSprite in _classIconSprites)
		{
			classIconSprite.SetTexture(classModel.IconTexture);
		}

		_characterMatTextureRect.SetTexture(classModel.MatFrontTexture);

		Open(AppController.Instance.DestroyCancellationToken).Forget();
	}

	private async GDTaskVoid Open(CancellationToken cancellationToken)
	{
		_tuckboxButton.SetEnabled(false, false);
		_buttonPressed = false;

		await _tuckbox.AnimateIn(cancellationToken);

		_tuckboxButton.SetEnabled(true, false);

		await GDTask.Delay(1f, cancellationToken: cancellationToken);
		//await GDTask.WaitUntil(() => _buttonPressed, cancellationToken: cancellationToken);

		_tuckbox.OpenAnimation(cancellationToken).Forget();


		this.DelayedCall(() => Open(ModelDB.Class<BombardModel>()), 2f);
	}

	private void OnTuckboxPressed()
	{
		_buttonPressed = true;
	}
}
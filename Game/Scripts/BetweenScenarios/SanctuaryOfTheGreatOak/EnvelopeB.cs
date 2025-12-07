using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class EnvelopeB : Control
{
	private static int[] DonationYellowNumbers =
	[
		5,
		10,
		15,
		20,
		25,
		30,
		40,
		50,
		60,
		70,
		80,
		90,
	];

	private const int DonationCountPerSet = 45;

	[Export]
	private PackedScene _normalCircleScene;
	[Export]
	private PackedScene _yellowCircleScene;
	[Export]
	private Control _circleParent;

	[Export]
	public Node3D EnvelopeB3DRoot;
	[Export]
	public SubViewport SubViewport;
	[Export]
	private Node3D _animationContainer;

	private readonly List<EnvelopeBCircle> _circles = new List<EnvelopeBCircle>();

	public override void _Ready()
	{
		base._Ready();

		for(int i = 0; i < DonationCountPerSet; i++)
		{
			PackedScene circleScene = DonationYellowNumbers.Contains(i) ? _yellowCircleScene : _normalCircleScene;
			EnvelopeBCircle circle = circleScene.Instantiate<EnvelopeBCircle>();
			_circleParent.AddChild(circle);
			circle.Init(i * 10, false);
			_circles.Add(circle);
		}
	}
}
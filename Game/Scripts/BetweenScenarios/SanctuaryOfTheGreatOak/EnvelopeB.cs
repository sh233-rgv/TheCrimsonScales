using System.Collections.Generic;
using System.Linq;
using Godot;
using GTweensGodot.Extensions;

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
	private static Vector3 AnimationAwayPosition = new Vector3(3, 0, -5);

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

		//TODO: Support multiple (2) sets of 45 circles/checkmarks

		int donationsCount = BetweenScenariosController.Instance.SavedCampaign.SanctuaryOfTheGreatOak.TotalDonationCount;
		for(int i = 0; i < DonationCountPerSet; i++)
		{
			int number = i + 1;
			PackedScene circleScene = DonationYellowNumbers.Contains(number) ? _yellowCircleScene : _normalCircleScene;
			EnvelopeBCircle circle = circleScene.Instantiate<EnvelopeBCircle>();
			_circleParent.AddChild(circle);
			circle.Init(number * 10, i < donationsCount);
			_circles.Add(circle);
		}

		_animationContainer.SetPosition(AnimationAwayPosition);
	}

	public void Donate()
	{
		UpdateChecks();
	}

	public void AnimateIn()
	{
		_animationContainer.SetPosition(AnimationAwayPosition);
		_animationContainer.TweenPosition(Vector3.Zero, 0.8f).Play();
	}

	public void AnimateOut()
	{
		_animationContainer.TweenPosition(AnimationAwayPosition, 0.8f).Play();
	}

	private void UpdateChecks()
	{
		int donationsCount = BetweenScenariosController.Instance.SavedCampaign.SanctuaryOfTheGreatOak.TotalDonationCount;
		for(int i = 0; i < donationsCount; i++)
		{
			_circles[i].Check();
		}
	}
}
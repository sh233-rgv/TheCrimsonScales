using Godot;

public partial class DonationCoin : RigidBody3D
{
	public override void _Ready()
	{
		base._Ready();

		//SetSleeping(false);
	}

	public void Launch()
	{
		ApplyImpulse((float)GD.RandRange(0.1f, 1f) * Vector3.Right * 0.3f, Vector3.Zero);
	}
}
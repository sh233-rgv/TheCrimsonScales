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
		ApplyImpulse(Vector3.Right * 0.2f, Vector3.Zero);
	}
}
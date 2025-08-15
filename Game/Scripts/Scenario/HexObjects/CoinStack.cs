using System.Collections.Generic;
using Fractural.Tasks;
using Godot;
using GTweens.Easings;
using GTweensGodot.Extensions;

public partial class CoinStack : LootableObject
{
	[Export]
	private PackedScene _coinScene;
	[Export]
	private Coin _originalCoin;

	private readonly List<Coin> _coins = new List<Coin>();

	public int CoinCount => _coins.Count;

	public override void _Ready()
	{
		base._Ready();

		_originalCoin.QueueFree();

		AddCoin();
	}

	public void AddCoin()
	{
		Coin newCoin = _coinScene.Instantiate<Coin>();
		AddChild(newCoin);
		_coins.Add(newCoin);

		newCoin.Scale = Vector2.Zero;
		newCoin.TweenScale(1f, 0.3f).SetEasing(Easing.OutBack).PlayFastForwardable();

		ReorganizeCoins();
	}

	public override async GDTask Loot(Figure lootObtainer)
	{
		AppController.Instance.AudioController.PlayFastForwardable(SFX.CoinPickup, delay: 0.1f);

		await base.Loot(lootObtainer);

		foreach(Coin coin in _coins)
		{
			lootObtainer.AddCoin();
		}
	}

	private void ReorganizeCoins()
	{
		if(_coins.Count == 1)
		{
			_coins[0].TweenPosition(Vector2.Zero, 0.2f);
		}
		else if(_coins.Count > 1)
		{
			for(int i = 0; i < _coins.Count; i++)
			{
				Node2D coin = _coins[i];
				float t = (float)i / _coins.Count;
				Vector2 position = Vector2.Right;
				position = position.Rotated(Mathf.DegToRad(t * 360f));
				coin.TweenPosition(position * 30f, 0.2f).PlayFastForwardable();
			}
		}
	}

	public override void AddInfoItemParameters(List<InfoItemParameters> parametersList)
	{
		base.AddInfoItemParameters(parametersList);

		parametersList.Add(new CoinInfoItem.Parameters(this));
	}
}
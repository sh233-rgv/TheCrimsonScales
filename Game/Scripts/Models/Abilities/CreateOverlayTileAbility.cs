using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

/// <summary>
/// An <see cref="Ability{T}"/> that allows a figure to create an Overlay Tile of a specific kind in an empty hex
/// </summary>
public class CreateOverlayTileAbility<T> : Ability<CreateOverlayTileAbility<T>.State>
	where T : OverlayTile
{
	public class State : AbilityState
	{
		public List<T> CreatedOverlayTiles { get; set; } = [];
	}

	public int Range { get; private set; } = 1;
	public int Count { get; private set; } = 1;
	public string AssetPath = "res://Content/OverlayTiles/Obstacles/Boulder1H.tscn";
	public string OverlayTileName = "Obstacle";

	public Action<State, List<Hex>> CustomSelectHexes { get; private set; } = null;
	public bool Mandatory = false;

	/// <summary>
	/// A builder extending <see cref="Ability{T}.AbstractBuilder{TBuilder, TAbility}"/> with setter methods
	/// for values defined in CreateOverlayTileAbility. Enables inheritors of CreateOverlayTileAbility to further extend the builder.
	/// </summary>
	/// <typeparam name="TBuilder"></typeparam> Any builder extending this AbstractBuilder.
	/// <typeparam name="TAbility"></typeparam> Any ability extending CreateOverlayTileAbility.
	public new abstract class AbstractBuilder<TBuilder, TAbility> : Ability<State>.AbstractBuilder<TBuilder, TAbility>
		where TBuilder : AbstractBuilder<TBuilder, TAbility>
		where TAbility : CreateOverlayTileAbility<T>, new()
	{
		public TBuilder WithRange(int range)
		{
			Obj.Range = range;
			return (TBuilder)this;
		}

		public TBuilder WithCount(int count)
		{
			Obj.Count = count;
			return (TBuilder)this;
		}

		public TBuilder WithCustomSelectHexes(Action<State, List<Hex>> selectHexes)
		{
			Obj.CustomSelectHexes = selectHexes;
			return (TBuilder)this;
		}

		public TBuilder WithCustomAsset(string assetPath)
		{
			Obj.AssetPath = assetPath;
			return (TBuilder)this;
		}

		public TBuilder WithCustomName(string name)
		{
			Obj.OverlayTileName = name;
			return (TBuilder)this;
		}

		public TBuilder WithMandatory(bool mandatory)
		{
			Obj.Mandatory = mandatory;
			return (TBuilder)this;
		}
	}

	/// <summary>
	/// A concrete implementation of the AbstractBuilder. Required to actually use the builder,
	/// as abstract builders cannot be instantiated.
	/// </summary>
	public class CreateOverlayTileBuilder : AbstractBuilder<CreateOverlayTileBuilder, CreateOverlayTileAbility<T>>
	{
		internal CreateOverlayTileBuilder() { }
	}

	/// <summary>
	/// A convenience method that returns an instance of CreateTrapBuilder.
	/// </summary>
	/// <returns></returns>
	public static CreateOverlayTileBuilder Builder()
	{
		return new CreateOverlayTileBuilder();
	}

	protected override async GDTask Perform(State abilityState)
	{
		for(int i = 0; i < Count; i++)
		{
			Hex hex = await AbilityCmd.SelectHex(abilityState, list =>
				{
					if(CustomSelectHexes != null)
					{
						CustomSelectHexes(abilityState, list);
					}
					else
					{
						if(typeof(Obstacle).IsAssignableFrom(typeof(T)))
						{
							list.AddRange(RangeHelper.GetHexesInRange(abilityState.Performer.Hex, Range).Where(hex => hex.IsEmpty()));
						}
						else
						{
							list.AddRange(RangeHelper.GetHexesInRange(abilityState.Performer.Hex, Range).Where(hex => hex.IsFeatureless()));
						}
					}

					for(int j = list.Count - 1; j >= 0; j--)
					{
						Hex hex = list[j];

						if(typeof(Obstacle).IsAssignableFrom(typeof(T)) && !RangeHelper.CheckCanPlaceObstacle(hex))
						{
							list.RemoveAt(j);
						}
					}
				},
				mandatory: Mandatory,
				hintText: $"Select a hex to place the {OverlayTileName}");

			if(hex == null)
			{
				return;
			}

			abilityState.CreatedOverlayTiles.Add(await AbilityCmd.CreateOverlayTile<T>(hex, SceneLoader.LoadPackedScene(AssetPath)));

			abilityState.SetPerformed();
		}
	}
}
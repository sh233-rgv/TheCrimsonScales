using System;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class SavedDeviceOptions
{
	[JsonProperty]
	public SavedOption<int> BGMVolume { get; private set; } = new SavedOption<int>(80);

	[JsonProperty]
	public SavedOption<int> BGSVolume { get; private set; } = new SavedOption<int>(80);

	[JsonProperty]
	public SavedOption<int> SFXVolume { get; private set; } = new SavedOption<int>(80);

	[JsonProperty]
	public SavedOption<bool> VibrationsEnabled { get; private set; } = new SavedOption<bool>(true);

	[JsonProperty]
	public SavedOption<bool> AnimatedCharacters { get; private set; } = new SavedOption<bool>(true);

	[JsonProperty]
	public SavedOption<int> GameplaySpeed { get; private set; } = new SavedOption<int>(0);

	public static LabeledOptions<float> GameplaySpeedOptions { get; } = new LabeledOptions<float>(
	[
		new("1x", 1f),
		new("1.25x", 1.25f),
		new("1.5x", 1.5f),
		new("1.75x", 1.75f),
		new("2x", 2f),
	]);
}
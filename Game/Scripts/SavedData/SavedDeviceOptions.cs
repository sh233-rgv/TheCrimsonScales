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
	public SavedOption<int> AnimationSpeed { get; private set; } = new SavedOption<int>(1);

	public static LabeledOptions<float> AnimationSpeedOptions { get; } = new LabeledOptions<float>(
	[
		new("1x", 1f),
		new("2x", 2f),
		new("4x", 4f),
	]);
}
global using DragData = Godot.Collections.Dictionary<string, Godot.Variant>;

using Godot;

namespace SadChromaLib.UI;

public static class Vector2Serialiser
{
	public static DragData Serialise(this Vector2 vec)
	{
		return new() {
			["x"] = vec.X,
			["y"] = vec.Y
		};
	}

	public static Vector2 Deserialise(DragData data)
	{
		return new Vector2(
			x: (float) data["x"],
			y: (float) data["y"]
		);
	}
}

public static class Vector3Serialiser
{
	public static DragData Serialise(this Vector3 vec)
	{
		return new() {
			["x"] = vec.X,
			["y"] = vec.Y,
			["z"] = vec.Z
		};
	}

	public static Vector3 Deserialise(DragData data)
	{
		return new Vector3(
			x: (float) data["x"],
			y: (float) data["y"],
			z: (float) data["z"]
		);
	}
}

public static class ColourSerialiser
{
	public static DragData Serialise(this Color colour)
	{
		return new() {
			["r"] = colour.R,
			["g"] = colour.G,
			["b"] = colour.B,
			["a"] = colour.A
		};
	}

	public static Color Deserialise(DragData data)
	{
		return new Color(
			r: (float) data["r"],
			g: (float) data["g"],
			b: (float) data["b"],
			a: (float) data["a"]
		);
	}
}
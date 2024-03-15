using Godot;
using Godot.Collections;

namespace SadChromaLib.UI;

/// <summary>
/// Base class for drag-and-drop data consumers.
/// </summary>
public abstract partial class BaseDropTarget: Panel
{
    #region Drop Target

    /// <summary>
    /// Returns whether or not a draggable item is compatible with this drop target.
    /// </summary>
    ///
    /// <example>
    /// <code>
    /// public override bool IsItemCompatible(string id)
    /// {
    ///     return id.ReferenceEquals("custom_data");
    /// }
    /// </code>
    /// </example>
    /// <param name="id">The identifier of the item being dropped.</param>
    /// <returns></returns>
    public abstract bool IsItemCompatible(StringName id);

    /// <summary>
    /// Processes the dropped data.
    /// </summary>
    /// <param name="id">The identifier of the item being dropped.</param>
    /// <param name="data">The serialised data of the dropped item.</param>
    public abstract void ConsumeDrop(StringName id, Dictionary<StringName, Variant> data);

    #endregion

    #region Configuration

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return IsItemCompatible(GetDropDataIdentifier(data));
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        var droppedData = DecodeData((string) data);

        if (droppedData is null)
            return;

        ConsumeDrop(droppedData.Value.Item1, droppedData.Value.Item2);
    }

    #endregion

    #region Generic Drop Target Utils

    public static (StringName, Dictionary<StringName, Variant>)?
    DecodeData(string data)
    {
        var passedData = (Dictionary<StringName, Variant>) Json.ParseString(data);

        // Malformed data
        if (!passedData.ContainsKey("id") || !passedData.ContainsKey("data"))
            return null;

        return ((StringName) passedData["id"],
                (Dictionary<StringName, Variant>) passedData["data"]);
    }

    /// <summary>
    /// Extracts a draggable item's identifier from a serialised string.
    /// </summary>
    /// <param name="data">The serialised string.</param>
    /// <returns></returns>
    public static StringName
    GetDropDataIdentifier(Variant data)
    {
        if (data.VariantType != Variant.Type.String)
            return null;

        return DecodeData((string) data)?.Item1;
    }

    /// <summary>
    /// Extracts a draggable item's data from a serialised string.
    /// </summary>
    /// <param name="data">The serialised string.</param>
    /// <returns></returns>
    public static Dictionary<StringName, Variant>
    GetDropDataContents(Variant data)
    {
        if (data.VariantType != Variant.Type.String)
            return null;

        return DecodeData((string) data)?.Item2;
    }

    #endregion
}
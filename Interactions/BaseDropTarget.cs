using Godot;

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
    public abstract void ConsumeDrop(StringName id, Godot.Collections.Dictionary<string, Variant> data);

    #endregion

    #region Configuration

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        if (data.VariantType != Variant.Type.String)
            return false;

        (string id, Godot.Collections.Dictionary<string, Variant> _)?
        droppedData = DecodeData((string) data);

        if (droppedData is null)
            return false;

        return IsItemCompatible(droppedData.Value.id);
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        (string id, Godot.Collections.Dictionary<string, Variant> data)?
        droppedData = DecodeData((string) data);

        if (droppedData is null)
            return;

        ConsumeDrop(droppedData.Value.id, droppedData.Value.data);
    }

    #endregion

    #region Helpers

    private (StringName, Godot.Collections.Dictionary<string, Variant>)?
    DecodeData(string data)
    {
        Godot.Collections.Dictionary<string, Variant> passedData = (Godot.Collections.Dictionary<string, Variant>) Json.ParseString(data);

        // Malformed data
        if (!passedData.ContainsKey("id") || !passedData.ContainsKey("data"))
            return null;

        return ((StringName) passedData["id"],
                (Godot.Collections.Dictionary<string, Variant>) passedData["data"]);
    }

    #endregion
}
using Godot;

namespace SadChromaLib.UI;

/// <summary>
/// Base class for drag-and-drop data consumers.
/// </summary>
public abstract partial class BaseDropTarget: Control
{
    /// <summary>
    /// Returns whether or not a draggable item is compatible with this drop target.
    /// For fast comparisons, use ReferenceEquals (or SadChromaLib.Utils.Convenience.IDUtils.Test)
    /// when checking against interned ID strings.
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
    public abstract bool IsItemCompatible(string id);

    /// <summary>
    /// Processes the dropped data.
    /// </summary>
    /// <param name="id">The identifier of the item being dropped.</param>
    /// <param name="data">The serialised data of the dropped item.</param>
    public abstract void ConsumeDrop(string id, string data);
}
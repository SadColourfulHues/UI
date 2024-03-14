using Godot;

namespace SadChromaLib.UI;

/// <summary>
/// Base class for draggable control components.
/// </summary>
public abstract partial class BaseDraggableItem: Control
{
    /// <summary>
    /// <summary>The item's unique 'drag-and-drop function' identifier.</summary>
    /// </summary>
    /// <returns></returns>
    public abstract string GetDragIdentifier();

    /// <summary>
    /// Returns the item's drag-and-drop data. Use JSON and SadChromaLib.Persistence for serialising native Godot primitives.
    /// </summary>
    /// <returns></returns>
    public abstract string GetData();

    /// <summary>
    /// Returns whether or not the item can be picked up in the current moment.
    /// </summary>
    /// <returns></returns>
    public virtual bool IsDraggable() {
        return true;
    }
}
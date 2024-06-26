using Godot;

namespace SadChromaLib.UI;

/// <summary>
/// Base class for draggable control components.
/// </summary>
public abstract partial class BaseDraggableItem: Button
{
    #region Draggable Item

    /// <summary>
    /// <summary>The item's unique 'drag-and-drop function' identifier.</summary>
    /// </summary>
    /// <returns></returns>
    public abstract string GetDragIdentifier();

    /// <summary>
    /// Returns the item's drag-and-drop data. Use the extensions provided in SadChromaLib::UI::CommonSerialisers
    /// for serialising native Godot primitives.
    /// </summary>
    /// <returns></returns>
    public abstract DragData GetData();

    /// <summary>
    /// Returns whether or not the item can be picked up in the current moment.
    /// </summary>
    /// <returns></returns>
    public virtual bool IsDraggable() {
        return true;
    }

    /// <summary>
    /// Override this to change its 'preview' item.
    /// </summary>
    /// <returns></returns>
    public virtual Control GetDragPreview() {
        return (Control) Duplicate();
    }

    #endregion

    #region Configuration

    public override Variant _GetDragData(Vector2 atPosition)
    {
        if (!IsDraggable())
            return default;

        Control preview = GetDragPreview();
        SetDragPreview(preview);

        return EncodeData();
    }

    #endregion

    #region Helpers

    private string EncodeData()
    {
        return Json.Stringify(new DragData {
            ["id"] = GetDragIdentifier(),
            ["data"] = GetData()
        });
    }

    #endregion
}
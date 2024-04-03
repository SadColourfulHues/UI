using Godot;

namespace SadChromaLib.UI;

/// <summary>
/// A draggable item that is meant to be used for list drag-and-drop functionality.
/// Attach this script to the item view's activator to enable this behaviour.
/// </summary>
public sealed partial class ListDraggableItem: BaseDraggableItem
{
    public static string Identifier => "list_drag_item";
    public static string KeySourceIndex => "source_idx";

    public override string GetDragIdentifier()
        => Identifier;

    public override DragData GetData()
    {
        return new DragData {
            [KeySourceIndex] = GetParent<Node>().GetIndex()
        };
    }

    #region Forward Drop Events to Item View

    public override bool _CanDropData(Vector2 atPosition, Variant data)
        => GetParent<Control>()._CanDropData(atPosition, data);

    public override void _DropData(Vector2 atPosition, Variant data)
        => GetParent<Control>()._DropData(atPosition, data);

    #endregion
}
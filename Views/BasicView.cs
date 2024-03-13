using Godot;

namespace SadChromaLib.UI;


/// <summary> The base-class for all view-types. </summary>
public class BasicView: Control
{
    /// <summary> Sets the visibility of this view. (Can be overridden.)</summary>
    public virtual void SetVisibility(bool visible)
    {
        // TODO: Implement
    }

    /// <summary> Returns the first control node in this view. </summary>
    public Control GetRootControl()
    {
        if (GetChildCount() < 1)
            return null;

        return GetChildOrNull<Control>(0);
    }
}

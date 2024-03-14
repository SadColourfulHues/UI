using Godot;

namespace SadChromaLib.UI;


/// <summary> The base-class for all view-types. In desktop-terms, think of them as windows or frames. </summary>
public partial class BasicView: Control
{
    /// <summary> Sets the visibility of this view. (Can be overridden.)</summary>
    public virtual void SetVisibility(bool visible)
    {
        Control rootControl = GetRootControl();

        if (!IsInstanceValid(rootControl))
            return;

        rootControl.Visible = visible;
    }

    /// <summary> Returns the first control node in this view. </summary>
    public Control GetRootControl()
    {
        return GetChildOrNull<Control>(0);
    }
}

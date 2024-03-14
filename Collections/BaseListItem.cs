using System.Diagnostics;
using Godot;

namespace SadChromaLib.UI;

public abstract partial class BaseListItem<DataType>: Control
{
    [Signal]
    public delegate void ActivatedEventHandler(int index);

    protected Panel _highlight = null;
    protected Button _activator = null;

    #region List Item Methods

    /// <summary>
    /// Called by the owning list to update the components of this item.
    /// </summary>
    /// <param name="data"></param>
    public abstract void Update(DataType data);

    /// <summary>
    /// Called whenever the user activates this item.
    /// </summary>
    public virtual void OnItemSelected(BaseListView<DataType> list) {
        _highlight.Visible = true;
    }

    /// <summary>
    /// Called when this item is deselected.
    /// </summary>
    public virtual void OnItemDeselected(BaseListView<DataType> list) {
        _highlight.Visible = false;
    }

    #endregion

    #region Utility Methods

    public void SetHighlightVisibility(bool highlighted)
    {
        _highlight.Visible = highlighted;
    }

    #endregion

    #region Configuration

    public override void _Ready()
    {
        _highlight = GetNodeOrNull<Panel>("%Highlight");
        _activator = GetNodeOrNull<Button>("%Activator");

        Debug.Assert(
            condition: _highlight is not null,
            message: "BaseListItem: A valid list item view must have a scene-unique Panel node named 'Highlight'. This component will be used as a visual indicator for list focus."
        );

        Debug.Assert(
            condition: _activator is not null,
            message: "BaseListItem: A valid list item view must have a scene-unique Button node named 'Activator'. This component will be used to catch activation events from user input."
        );

        _highlight.Visible = false;
        _activator.Pressed += OnActivated;
    }

    #endregion

    #region Event Handlers

    private void OnActivated()
    {
        EmitSignal(SignalName.Activated, GetIndex());
    }

    #endregion
}
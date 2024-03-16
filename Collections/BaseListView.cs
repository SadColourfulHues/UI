using System;
using System.Diagnostics;

using Godot;

namespace SadChromaLib.UI;

public abstract partial class BaseListView<DataType>: Control
{
    [Signal]
    public delegate void ItemSelectionChangedEventHandler(int index);

    [ExportGroup("List View")]
    [Export]
    Control _viewport;

    [Export]
    PackedScene _pkgItemViewTemplate;

    private BaseListItem<DataType>[] _items;
    protected bool _canSelect = true;
    protected int _selectedIndex = -1;

    #region List Methods

    protected abstract int GetDataCount();
    protected abstract DataType GetDataAt(int i);

    /// <summary>
    /// (Optional) This is called when an item view has been initialised for the very first time.
    /// </summary>
    /// <param name="item">A reference to the newly-created item view.</param>
    /// <param name="data">The data adjacent to the item view's index.</param>
    /// <param name="index">The index of the item view in the main viewport.</param>
    /// <returns></returns>
    protected virtual void OnConfigureItem(BaseListItem<DataType> item, DataType data, int index) { }

    /// <summary>
    /// (Optional) This is called by certain item views that support drag-and-drop functionality. The expected functionality is to swap the data at the specified indices.
    /// </summary>
    /// <param name="indexA"></param>
    /// <param name="indexB"></param>
    protected virtual void OnRequestSwap(int indexA, int indexB) { }

    /// <summary>
    /// (Optional) This is called when an item view has been selected.
    /// </summary>
    /// <param name="index"></param>
    protected virtual void OnItemActivated(int index) { }

    #endregion

    #region Configuration

    public override void _Ready()
    {
        Debug.Assert(
            condition: IsInstanceValid(_pkgItemViewTemplate),
            message: $"BaseListView: List View \"{Name}\" must have a valid item view package!"
        );
    }

    #endregion

    #region Organisation Methods

    /// <summary>
    /// Returns a span pointing to currently-active item views.
    /// </summary>
    /// <returns></returns>
    protected ReadOnlySpan<BaseListItem<DataType>> GetItemRefs() {
        return _items.AsSpan();
    }

    /// <summary>
    /// Returns the maximum valid index for this list.
    /// </summary>
    /// <value></value>
    protected int MaxAllowedIndex
    {
        get
        {
            if (_items is null)
                return 0;

            return Math.Min(_items.Length, GetDataCount());
        }
    }

    #endregion

    #region utility Methods

    public void SetListItemsSelectable(bool canSelect)
    {
        _canSelect = canSelect;

        if (canSelect)
            return;

        Deselect();
    }

    #endregion

    #region Organisation Methods

    /// <summary>
    /// Deletes all item views from the scene. (Must be regenerated before the next cycle.)
    /// </summary>
    public void Clear()
    {
        if (_items is null)
            return;

        for (int i = _items.Length; i --> 0;) {
            if (!IsInstanceValid(_items[i]))
                continue;

            _viewport.GetChild(i).QueueFree();
            _items[i] = null;
        }
    }

    /// <summary>
    /// Allocates a set amount of space for item views ahead of time.
    /// </summary>
    /// <param name="count"></param>
    public void ReserveItems(int count)
    {
        if (_items is not null) {
            Clear();
        }

        _items = new BaseListItem<DataType>[count];
    }

    /// <summary>
    /// Creates new item views from the provided template package.
    /// This method does not update the displayed data.
    /// To refresh the content, use the 'Update' method, instead.
    /// </summary>
    public void Regenerate()
    {
        CheckArrayConditions();

        int count = GetDataCount();

        for (int i = 0; i < _items.Length; ++i) {
            if (IsInstanceValid(_items[i])) {
                _items[i].QueueFree();
                _items[i] = null;
            }

            BaseListItem<DataType> itemView = (BaseListItem<DataType>) _pkgItemViewTemplate.Instantiate<Control>();
            _viewport.AddChild(itemView);

            // Automatically forward activation events to the main list view
            itemView.Visible = false;

            itemView.Activated += OnItemActivatedInternal;
            itemView.SwapRequested += OnItemRequestSwapInternal;

            _items[i] = itemView;

            if (i >= count)
                continue;

            OnConfigureItem(itemView, GetDataAt(i), i);
        }
    }

    /// <summary>
    /// Updates the contents of the list view.
    /// This method will never create new item views.
    /// To create and initialise new views, use the 'Regenerate' method, instead.
    /// </summary>
    public void Update()
    {
        ReadOnlySpan<BaseListItem<DataType>> items = _items.AsSpan();
        int count = GetDataCount();

        for (int i = 0; i < items.Length; ++i) {
            if (!IsInstanceValid(items[i]))
                continue;

            if (i >= count) {
                items[i].Visible = false;
                continue;
            }

            items[i].Update(GetDataAt(i));
            items[i].Visible = true;
        }
    }

    /// <summary>
    /// Clears the list view's selected state
    /// </summary>
    public void Deselect()
    {
        if (IsValidIndex(_selectedIndex) &&
            IsInstanceValid(_items[_selectedIndex]))
        {
            _items[_selectedIndex].SetHighlightVisibility(false);
        }

        _selectedIndex = -1;
        EmitSignal(SignalName.ItemSelectionChanged, -1);
    }

    /// <summary>
    /// Simulates an activation event on the target item view index.
    /// </summary>
    /// <param name="index">The index of the item view to select</param>
    public void Select(int index)
    {
        OnItemActivatedInternal(index);
    }

    #endregion

    #region Helpers

    private void CheckArrayConditions()
    {
        int countRequirement = GetDataCount();

        Debug.Assert(
            condition: _items is not null,
            message: "BaseListView: Call 'ReserveItems' to set the maximum number of items in this list before calling regenerate/update."
        );

        // Deactivate un-used cells
        for (int i = 0; i < _items.Length; ++i) {
            if (!IsInstanceValid(_items[i]))
                continue;

            _items[i].Visible = i < countRequirement;
        }
    }

    /// <summary>
    /// Returns whether or not a given index is valid in the context of this list.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool IsValidIndex(int index)
    {
        return index >= 0 && index < MaxAllowedIndex;
    }

    #endregion

    #region Event Handlers

    private void OnItemActivatedInternal(int index)
    {
        if (!_canSelect)
            return;

        if (IsValidIndex(_selectedIndex) &&
            IsInstanceValid(_items[_selectedIndex]))
        {
            _items[_selectedIndex].SetHighlightVisibility(false);
        }

        OnItemActivated(index);
        _selectedIndex = index;

        EmitSignal(SignalName.ItemSelectionChanged, _selectedIndex);

        if (!IsValidIndex(index) || !IsInstanceValid(_items[index]))
            return;

        _items[index].SetHighlightVisibility(true);
    }

    private void OnItemRequestSwapInternal(int source, int target)
    {
        if (!_canSelect)
            return;

        OnRequestSwap(source, target);
    }

    #endregion
}
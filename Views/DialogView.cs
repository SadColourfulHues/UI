using System;

namespace SadChromaLib.UI;

/// <summary>
/// A special view type where only one could ever exist at any given time.
/// </summary>
public partial class DialogView: BasicView
{
    public static event Action<DialogView> OnActiveChanged;

    static DialogView _activeRef = null;

    public override void _Notification(int what)
    {
        base._Notification(what);

        // Automaticaly resign before free
        if (what != NotificationPredelete)
            return;

        ResignActive();
    }

    public override void SetVisibility(bool visible)
    {
        base.SetVisibility(visible);

        if (visible) {
            BecomeActive();
        }
        else {
            ResignActive();
        }
    }

    #region Modal Functions

    public void BecomeActive()
    {
        GetActiveDialogRef()?.SetVisibility(false);

        _activeRef = this;
        OnActiveChanged?.Invoke(_activeRef);
    }

    public void ResignActive()
    {
        if (!Object.ReferenceEquals(_activeRef, this))
            return;

        _activeRef = null;
        OnActiveChanged?.Invoke(null);
    }

    public static DialogView GetActiveDialogRef()
    {
        if (!IsInstanceValid(_activeRef))
            return null;

        return _activeRef;
    }

    #endregion
}
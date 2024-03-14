using System;

namespace SadChromaLib.UI;

/// <summary>
/// A special view type where only one could ever exist at any given time.
/// </summary>
public sealed partial class DialogView: BasicView
{
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
    }

    public void ResignActive()
    {
        if (!Object.ReferenceEquals(_activeRef, this))
            return;

        _activeRef = null;
    }

    public static DialogView GetActiveDialogRef()
    {
        if (!IsInstanceValid(_activeRef))
            return null;

        return _activeRef;
    }

    #endregion
}
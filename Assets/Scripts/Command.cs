using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command : MonoBehaviour
{
    [SerializeField] bool leftClick;
    public bool IsLeftClick() { return leftClick; }

    [SerializeField] bool doTwice;
    public bool IsDoneTwice() { return doTwice; }


    [SerializeField] Sprite icon;
    public Sprite GetIcon() { return icon;}

    [SerializeField] Sprite cursor;

    protected string reasonLocked;
    public string GetReasonLocked() { return reasonLocked; }
    [SerializeField] string description;
    public string GetDescription() { return description; }

    public string GetDescriptionAndReasonLocked() { return description + "\n\n" + reasonLocked; }

    public Sprite GetCursor() { return cursor;}
    public abstract void Execute(SelectableObject[] active, object optionalInfo = null);
    public abstract bool IsAvailable(SelectableObject[] active);

    public virtual bool IsToggled(SelectableObject[] active, object opiotnalInfo = null) { return false; }
}

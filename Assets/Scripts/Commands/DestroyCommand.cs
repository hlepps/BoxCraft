using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCommand : Command
{
    public override void Execute(SelectableObject[] active, object optionalInfo = null)
    {
        foreach (SelectableObject o in active)
        {
            Destroy(o.gameObject);
        }
    }

    public override bool IsAvailable(SelectableObject[] active)
    {
        return true;
    }
}

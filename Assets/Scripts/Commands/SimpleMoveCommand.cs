using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveCommand : Command
{
    public override void Execute(SelectableObject[] active, object optionalInfo = null)
    {
        foreach (SelectableObject o in active)
        {
            ((Entity)o).Move((Vector3)(optionalInfo));
        }
    }

    public override bool IsAvailable(SelectableObject[] active)
    {
        return true;
    }
}

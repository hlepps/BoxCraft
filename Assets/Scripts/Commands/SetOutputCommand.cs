using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOutputCommand : Command
{
    public override void Execute(SelectableObject[] active, object optionalInfo = null)
    {
        foreach (SelectableObject o in active)
        {
            ((Building)o).SetOutput((Vector3)(optionalInfo));
        }
    }

    public override bool IsAvailable(SelectableObject[] active)
    {
        return true;
    }
}

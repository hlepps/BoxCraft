using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldFormationMoveCommand : Command
{
    public override void Execute(SelectableObject[] active, object optionalInfo = null)
    {
        Vector3 destination = (Vector3)(optionalInfo);
        Vector3 center = new Vector3();
        foreach (SelectableObject o in active)
        {
            if(o != null)
                center += o.transform.position;

        }
        center /= active.Length;
        foreach (Entity o in active)
        {
            if (o != null)
            {
                Vector3 offset = o.transform.position - center;
                o.Move(destination + offset);
            }

        }
    }

    public override bool IsAvailable(SelectableObject[] active)
    {
        return true;
    }
}

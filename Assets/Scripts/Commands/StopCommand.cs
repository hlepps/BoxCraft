using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCommand : Command
{
    public override void Execute(SelectableObject[] active, object optionalInfo = null)
    {
        foreach (SelectableObject o in active)
        {
            Entity e = o as Entity;
            if (e != null)
            {
                e.Move(e.transform.position);
                if (o.GetType() == typeof(Worker))
                {
                    Worker worker = o as Worker;
                    worker.FinishJob();
                }
            }
        }
    }

    public override bool IsAvailable(SelectableObject[] active)
    {
        return true;
    }
}

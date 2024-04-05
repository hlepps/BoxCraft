using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAttackMode : Command
{
    public override void Execute(SelectableObject[] active, object optionalInfo = null)
    {
        bool a = ((Soldier)active[0]).GetAttackMode();
        foreach (Soldier o in active)
        {
            if (o != null)
            {
                o.ToggleAttackMode(!a);
            }
        }
    }

    public override bool IsAvailable(SelectableObject[] active)
    {
        return true;
    }

    public override bool IsToggled(SelectableObject[] active, object optionalInfo = null)
    {
        bool a = ((Soldier)active[0]).GetAttackMode();
        foreach (Soldier o in active)
        {
            if (o.GetAttackMode() != a)
                return false;
        }
        return a;
    }
}

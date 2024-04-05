using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalQueue : MonoBehaviour
{
    List<(Action action, float maxTime, float time, string name, Sprite icon)> queue = new List<(Action, float, float, string, Sprite)> ();
    public List<(Action action, float maxTime, float time, string name, Sprite icon)> GetQueue() { return queue; }

    [SerializeField] int maxEntriesInQueue = 5;
    public int GetMax() { return maxEntriesInQueue; }
    public int GetQueueCount() { return queue.Count; }
    public bool IsQueueFull() { /*Debug.Log(queue.Count); Debug.Log(queue.Count >= maxEntriesInQueue); Debug.Log(maxEntriesInQueue);*/ return queue.Count >= maxEntriesInQueue; }

    public void AddToQueue(Action action, float time, string name, Sprite icon)
    {
        if (!IsQueueFull())
        {
            queue.Add((action, time, time, name, icon));
            UIQueue.GetInstance().SetQueue(HumanController.GetInstance().GetSelectedObjects()[0] as Building);
        }
    }

    private void Update()
    {
        if (queue.Count > 0)
        {
            HumanController.GetInstance().CheckForNullInSelectableObjects();
            queue[0] = (queue[0].action, queue[0].maxTime, queue[0].time - Time.deltaTime, queue[0].name, queue[0].icon);
            //Debug.Log(queue[0].time);
            if (queue[0].time <= 0)
            {
                queue[0].action?.Invoke();
                //UIQueue.GetInstance().ClearQueue();
                queue.RemoveAt(0);
                if(HumanController.GetInstance().GetSelectedObjects().Length == 1)
                    UIQueue.GetInstance().SetQueue(HumanController.GetInstance().GetSelectedObjects()[0] as Building);
                
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIQueue : MonoBehaviour
{
    static UIQueue instance;
    public static UIQueue GetInstance() {  return instance; }
    List<UIQueueEntry> uiQueue = new List<UIQueueEntry>();
    [SerializeField] GameObject prefab;
    [SerializeField] Transform content;

    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        instance = this;
    }

    Building current;
    public bool IsBuildingCurrentlyShowing(Building b)
    { return current == b; }
    public void SetQueue(Building b)
    {
        ClearQueue();
        if (b == null)
            return;
        if (HumanController.GetInstance().GetSelectedObjects().Length == 1)
        {
            //Debug.Log(HumanController.GetInstance().GetSelectedObjects().Length);

            foreach (var item in b.GetUniversalQueue().GetQueue())
            {
                GameObject temp = Instantiate(prefab, content,false);
                Vector2 pos = temp.GetComponent<RectTransform>().anchoredPosition;
                pos.y = uiQueue.Count * -50f;
                temp.GetComponent<RectTransform>().anchoredPosition = pos;
                UIQueueEntry entry = temp.GetComponent<UIQueueEntry>();
                entry.Init(item.icon, item.name, item.maxTime);
                uiQueue.Add(entry);
            }
        }
    }

    public void ClearQueue()
    {
        for (int i = uiQueue.Count - 1; i >= 0; i--)
        {
            //Debug.Log("clearing");
            Destroy(uiQueue[i].gameObject);
        }

        uiQueue.Clear();
    }

    public void UpdateQueue(Building b)
    {
        for (int i = 0; i < uiQueue.Count; i++)
        {
            //Debug.Log("updating");
            uiQueue[i].SetValue(b.GetUniversalQueue().GetQueue()[i].time);
        }
    }

    private void Update()
    {
        if (HumanController.GetInstance().GetSelectedObjects().Length == 1)
        {
            if(uiQueue.Count > 0)
            {
                UpdateQueue((Building)HumanController.GetInstance().GetSelectedObjects()[0]);
            }
        }

    }

}

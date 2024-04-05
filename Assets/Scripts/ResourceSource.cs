using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ResourceType
{
    COPPER = 1, STONE = 2, WOOD = 3
}
[System.Serializable]
public class Resource
{
    public ResourceType type;
    public int quantity;

    public Resource(ResourceType t, int q)
    {
        type = t;
        quantity = q;
    }
}

public class ResourceSource : SelectableObject
{
    [Header("ResourceSource")]
    [SerializeField] Resource resource;
    public Resource GetResource() { return resource; }

    List<Worker> currentWorkers = new List<Worker>();
    List<float> timers = new List<float>();

    [SerializeField] float mineTime = 0.5f;
    [SerializeField] AudioClip mineSound;

    private void Start()
    {
        Init(resource);
    }

    public void Init(Resource res)
    {
        resource = res;

        GetUniversalBar().SetMaxValue(res.quantity);
        GetUniversalBar().SetValue(res.quantity);
    }

    public void AddWorker(Worker worker)
    {
        currentWorkers.Add(worker);
        timers.Add(mineTime);
    }

    public void RemoveWorker(Worker worker)
    {
        timers.RemoveAt(currentWorkers.IndexOf(worker));
        currentWorkers.Remove(worker);
    }
    
    private void OnTriggerStay(Collider other)
    {
        Worker w = other.GetComponent<Worker>();
        if(currentWorkers.Contains(w))
        {
            if (w.GetCarry() == null)
            {
                //if (timers[currentWorkers.IndexOf(w)] > mineTime)
                //{
                GetComponent<AudioSource>().PlayOneShot(mineSound);
                timers[currentWorkers.IndexOf(w)] = 0;
                w.SetCarry(new Resource(resource.type, 2));
                resource.quantity -= 2;
                GetUniversalBar().SetValue(resource.quantity);
                if (resource.quantity == 0)
                {
                    SFX.GetInstance().DestroySound(mineSound, transform.position, 1f);
                    Destroy(this.gameObject);
                }
                //}
                //else timers[currentWorkers.IndexOf(w)] += Time.deltaTime;
            }
        }
    }


}

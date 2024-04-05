using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public delegate void TriggerEnter(Collider other);
    public delegate void TriggerExit(Collider other);
    public TriggerEnter triggerEnter;
    public TriggerExit triggerExit;

    private void OnTriggerEnter(Collider other)
    {
        triggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        triggerExit(other);
    }
}

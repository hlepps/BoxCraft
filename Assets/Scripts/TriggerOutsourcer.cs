using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOutsourcer : MonoBehaviour
{
    public delegate void OnTriggerEnterDelegate(Collider other);
    public OnTriggerEnterDelegate onTriggerEnterDelegate;
    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnterDelegate?.Invoke(other);
    }

    public delegate void OnTriggerStayDelegate(Collider other);
    public OnTriggerStayDelegate onTriggerStayDelegate;
    private void OnTriggerStay(Collider other)
    {
        onTriggerStayDelegate?.Invoke(other);
    }

    public delegate void OnTriggerExitDelegate(Collider other);
    public OnTriggerExitDelegate onTriggerExitDelegate;
    private void OnTriggerExit(Collider other)
    {
        onTriggerExitDelegate?.Invoke(other);
    }

}

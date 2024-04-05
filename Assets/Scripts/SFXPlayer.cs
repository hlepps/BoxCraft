using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    static SFXPlayer instance;
    public static SFXPlayer GetInstance() {  return instance; }

    [SerializeField] SFXPlayer player;

    private void Start()
    {
        instance = this;
    }

    public void PlaySound(int number, Vector3 pos)
    {
        
    }
}

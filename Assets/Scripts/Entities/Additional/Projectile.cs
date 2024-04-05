using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public void Init()
    {
        Sustain();
    }

    public void Sustain()
    {
        lifetime = 1;
    }

    public float lifetime = 1f;

    private void Update()
    {
        if(lifetime <= 0)
            Finish();

        lifetime -= Time.deltaTime;
    }

    public void Finish()
    {
        Destroy(this.gameObject);
    }


}

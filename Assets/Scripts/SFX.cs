using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    static SFX instance;
    public static SFX GetInstance() { return instance; }

    public AudioClip dropResourceSound;
    public AudioClip newUnitSound;
    public AudioClip destroySelectableSound;

    private void Start()
    {
        instance = this;
    }

    public void DestroySound(AudioClip sound, Vector3 pos, float time)
    {
        StartCoroutine(RunDestroySound(sound, pos, time));
    }

    IEnumerator RunDestroySound(AudioClip sound, Vector3 pos, float time)
    {
        var obj = new GameObject();
        obj.transform.position = pos;
        obj.AddComponent<AudioSource>();
        AudioSource aso = obj.GetComponent<AudioSource>();
        aso.loop = false;
        aso.spatialBlend = 1;
        aso.dopplerLevel = 0;
        aso.spread = 0;
        aso.maxDistance = 10;

        aso.PlayOneShot(sound);

        float x = time;
        while(x > 0)
        {
            yield return null;
            x -= Time.deltaTime;
        }
        Destroy(obj);
    }
}

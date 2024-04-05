using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> music = new List<AudioClip>();
    int current = 0;

    static MusicManager instance;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            current = Random.Range(0, music.Count);
            GetComponent<AudioSource>().clip = music[current];
            GetComponent<AudioSource>().Play();
        }
        else
            Destroy(this.gameObject);
    }

    private void Update()
    {
        if(!GetComponent<AudioSource>().isPlaying)
        {
            current = Random.Range(0, music.Count);
            GetComponent<AudioSource>().clip = music[current];
            GetComponent<AudioSource>().Play();
        }    
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] List<Wave> waves = new List<Wave>();
    [SerializeField] float waveTime = 60;
    [SerializeField] AudioClip newWave;
    [SerializeField] GameObject gameOver;
    [SerializeField] TextMeshProUGUI turnsText;
    int count = 1;
    float time = 30;
    private void Start()
    {
        time = waveTime*3;
    }
    bool startedGame = false;
    void Update()
    {
        if (Building.GetTeamMainBase('0') != null)
        {
            startedGame = true;
            if(time <= 0)
            {
                time = waveTime;
                StartCoroutine(SpawnWave());
                GetComponent<AudioSource>().PlayOneShot(newWave);
            }
            else
            {
                time -= Time.deltaTime;
            }
        }
        else
        {
            if(startedGame)
            {
                gameOver.SetActive(true);
                turnsText.text = "You lasted " + count + " waves";
            }
        }    
        WaveBox.GetInstance().UpdateText(count, (int)time);
    }

    public IEnumerator SpawnWave()
    {
        for (int i = 0; i < count; i++)
        {
            int side = UnityEngine.Random.Range(0, 4);
            float pos = UnityEngine.Random.Range(-100f, 100f);
            int wave = UnityEngine.Random.Range(0, waves.Count);
            Vector3 final = new Vector3(0, 0, 0);
            switch (side)
            {
                case 0:
                    final.x = 100;
                    final.z = pos;
                    break;
                case 1:
                    final.x = pos;
                    final.z = 100;
                    break;
                case 2:
                    final.x = -100;
                    final.z = pos;
                    break;
                case 3:
                    final.x = pos;
                    final.z = -100;
                    break;
            }
            foreach (GameObject obj in waves[wave].entities)
            {
                yield return null;
                var x = Instantiate(obj);
                x.transform.position = final;
                x.GetComponent<NavMeshAgent>().Warp(final);
                x.GetComponent<Entity>().MoveWithoutStoppingJob(Building.GetTeamMainBase('0'));
                Action a = () => { x.GetComponent<Entity>().MoveWithoutStoppingJob(Building.GetTeamMainBase('0')); };
                x.GetComponent<Entity>().StartJob(a, a, null);
            }
        }
        count++;
    }
}

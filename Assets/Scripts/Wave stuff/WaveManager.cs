using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class WaveManager : MonoBehaviour {
    public Wave[] waves;
    public static int enemyCount;
    public int waveIndex = 0;
    public Text enemyUI;
    public Text waveUI;
    public Text idiumUI;
    private GridGraph gridgraph;
    public bool spawningEnemies = false;
    public float timeBetweenWaves = 5f;
    private float waveEndTime;
    public GameObject partBurrow;

    [Range(0, 1)]
    public float waveProb = 0; //Probability that a wave will happen every second
    public Vector3 probStage0;
    public Vector3 probStage1;
    public Vector3 probStage2;
    public int probStage = 0;
    public GameObject partRichter;

    // Update is called once per frame
    void Start() {
        waveEndTime = Time.time;
        gridgraph = AstarPath.active.data.gridGraph;
        waveIndex = 0;
        enemyCount = 0;

        InvokeRepeating("waveCheck", 0, 1);
    }

    public void Update() {
        //Handle seismo code here
        probStage0 = new Vector3(((Mathf.PerlinNoise(0, Time.time) - 0.5f) * 2f) * Mathf.Sin(Time.time * 2f), -10, 3.68f);
        probStage1 = new Vector3(((Mathf.PerlinNoise(0, Time.time) - 0.5f) * 6f) * Mathf.Sin(Time.time * 6f), -10, 3.68f);
        probStage2 = new Vector3(((Mathf.PerlinNoise(0, Time.time) - 0.5f) * 12f) * Mathf.Sin(Time.time * 12f), -10, 3.68f);

        if (waveProb < 0.01) {
            probStage = 0;
        } else if (waveProb < 0.02) {
            probStage = 1;
        } else probStage = 2;

        if (spawningEnemies) probStage = 2;

        switch (probStage) {
            case 0:
                partRichter.transform.position = probStage0;
                break;
            case 1:
                partRichter.transform.position = probStage1;
                break;
            case 2:
                partRichter.transform.position = probStage2;
                break;
            default:
                partRichter.transform.position = probStage0;
                break;
        }
    }

    public void waveCheck() {
        //If there are no enemies currently and no enemies left in the wave, we are between waves and should start calculating probability
        if (!spawningEnemies && enemyCount <= 0) {
            //waveProb += (Time.time - waveEndTime) / 200000000f; //Base it off of time?
            if (ConCollector.collecting) {
                waveProb += 0.005f;
            }

        }
        if (Random.value < waveProb) { //Start spawning enemies
            waveProb = 0;
            spawningEnemies = true;
            //Spawn enemies coroutine
            if (waveIndex < waves.Length) {
                //Spawn wave at current waveIndex
                StartCoroutine("SpawnWave");
            } else {
                //ProcGen waves can be handled here
                spawningEnemies = false;
                waveEndTime = Time.time;
            }
        }
        updateUI();
    }

    IEnumerator SpawnWave() {
        Debug.Log("Spawning wave: " + waveIndex);
        for (var i = 0; i < waves[waveIndex].spawns.Length; i++) {
            //Each group gets a different position
            var pos = RandomOnPath();
            var rot = Quaternion.FromToRotation(Vector3.back, Vector3.right);

            var burrowParts = Instantiate(partBurrow, pos, rot).GetComponent<ParticleSystem>();
            burrowParts.Stop();
            var main = burrowParts.main;
            main.duration = waves[waveIndex].spawns.Length;
            burrowParts.Play();
            //This part looks complex but it's just looping through the num in our group structures
            for (var j = 0; j < waves[waveIndex].spawns[i].num; j++) {
                //Give them a lil offset
                pos.x += Random.Range(-1f, 1f);
                pos.z += Random.Range(-1f, 1f);
                //If we spawn a hostile, up the enemy count
                Instantiate(waves[waveIndex].spawns[i].hostile, pos, rot);
                enemyCount++;
                yield return new WaitForSeconds(1f);
            }

        }
        //We have finished spawning our wave
        waveIndex++;
        waveProb = 0;
        spawningEnemies = false;
        waveEndTime = Time.time;
        ConCollector.collecting = false;
    }

    public void updateUI() {
        enemyUI.text = "HOSTILES: " + enemyCount;
        waveUI.text = "WAVE: " + waveIndex;
        idiumUI.text = "IDIUM: " + ConCollector.bank;
    }

    public Vector3 RandomOnPath() {
        //Repeat until we find a pos that works
        int x, z;
        do {
            //Generate random x,y based on depth and width
            x = Random.Range(0, gridgraph.depth);
            z = Random.Range(0, gridgraph.width);
        } while (!gridgraph.GetNode(x, z).Walkable);
        Vector3 pos = (Vector3)gridgraph.GetNode(x, z).position;
        return pos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class WaveManager : MonoBehaviour
{
    public Wave[] waves;
    public static int enemyCount;
    public int waveIndex = 0;
    public float enemyGrowthRate = 1.5f;
    public Text enemyUI;
    public Text waveUI;
    public Text idiumUI;
    [Range(0, 1)]
    public float waveProb = 0; //Probability that a wave will happen
    private GridGraph gridgraph;
    public bool spawningEnemies = false;
    public float timeBetweenWaves = 5f;
    private float waveEndTime;
    public GameObject partBurrow;
    public GameObject partRichter;

    // Update is called once per frame
    void Start()
    {
        waveEndTime = Time.time;
        gridgraph = AstarPath.active.data.gridGraph;
        waveIndex = 0;
        enemyCount = 0;

        InvokeRepeating("waveCheck", 0, 1);
    }

    public void Update() {
        //Handle richter code here
        partRichter.transform.position = new Vector3((waveProb*100)*Mathf.Sin(Time.time*8), -10, 3.68f);
    }

    public void waveCheck() {
        //If there are no enemies currently and no enemies left in the wave, we are between waves and should start calculating probability
        if (!spawningEnemies && enemyCount <= 0) {
            //waveProb += (Time.time - waveEndTime) / 200000000f; //Base it off of time?


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
        spawningEnemies = false;
        waveEndTime = Time.time;
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
        } while (!gridgraph.GetNode(x,z).Walkable);
        Vector3 pos = (Vector3)gridgraph.GetNode(x, z).position;
        return pos;
    }
}

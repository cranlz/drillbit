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
    [Range(0, 1)]
    public float waveProb = 0; //Probability that a wave will happen
    private GridGraph gridgraph;
    public bool spawningEnemies = false;
    public float timeBetweenWaves = 5f;
    private float waveEndTime;

    // Update is called once per frame
    void Start()
    {
        waveEndTime = Time.time;
        gridgraph = AstarPath.active.data.gridGraph;
        waveIndex = 0;
        enemyCount = 0;
    }
    void Update()
    {
        //Check if wave is over
        //Debug.Log(enemyCount);
        //Wave is over, start checking probability
        /*
        if (enemyCount <= 0 && ) { 
            var pos = RandomOnPath();
            var rot = Quaternion.FromToRotation(Vector3.forward, Vector3.zero);
            enemyCount = 0;

            //start coroutine spawning enemies?
            if (waveIndex < waves.Length) {
                Debug.Log("Starting wave " + waveIndex);
                for (var i = 0; i < waves[waveIndex].spawns.Length; i++) {
                    //This part looks complex but it's just looping through the num in our group structures
                    for (var j = 0; j < waves[waveIndex].spawns[i].num; j++) {
                        pos.x += Random.Range(-1f, 1f);
                        pos.z += Random.Range(-1f, 1f);
                        //If we spawn a hostile, up the enemy count
                        if (Instantiate(waves[waveIndex].spawns[i].hostile, pos, rot)) enemyCount++;
                    }

                }
                waveIndex++;
            }
            updateUI();
        }*/

        //NEW CODE
        //If there are no enemies currently and no enemies left in the wave, we are between waves and should start calculating probability
        if (!spawningEnemies && enemyCount <= 0) {
            if (Time.time - waveEndTime >= timeBetweenWaves) waveProb = 1;
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
        for (var i = 0; i < waves[waveIndex].spawns.Length; i++) {
            //Each group gets a different position
            var pos = RandomOnPath();
            var rot = Quaternion.FromToRotation(Vector3.forward, Vector3.zero);
            //This part looks complex but it's just looping through the num in our group structures
            for (var j = 0; j < waves[waveIndex].spawns[i].num; j++) {
                //Give them a lil offset
                pos.x += Random.Range(-1f, 1f);
                pos.z += Random.Range(-1f, 1f);
                //If we spawn a hostile, up the enemy count
                if (Instantiate(waves[waveIndex].spawns[i].hostile, pos, rot)) enemyCount++;
                yield return new WaitForSeconds(1f);
            }

        }
        //We have finished spawning our wave
        waveIndex++;
        spawningEnemies = false;
        waveEndTime = Time.time;
    }

    public void updateUI() {
        enemyUI.text = "Enemies: " + enemyCount;
        waveUI.text = "Wave: " + waveIndex;
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

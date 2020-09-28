using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public static int enemyCount;
    public int waveIndex = 0;
    public float enemyGrowthRate = 1.5f;
    public Text enemyUI;
    public Text waveUI;

    // Update is called once per frame
    void Start()
    {
        waveIndex = 0;
        enemyCount = 0;
    }
    void Update()
    {
        //Check if wave is over
        //Debug.Log(enemyCount);
        if (enemyCount <= 0) {
            //Start new wave
            Debug.Log("Starting wave " + waveIndex);
            var pos = RandomCircle(Vector3.zero, 50f);
            var rot = Quaternion.FromToRotation(Vector3.forward, Vector3.zero);
            waveIndex++;
            updateWaveUI();
            //start coroutine spawning enemies
            enemyCount = 0;
            for (var i = 0; i < Mathf.Pow(waveIndex, enemyGrowthRate); i++) {
                Instantiate(enemyPrefabs[0], pos, rot);
                enemyCount++;
                updateEnemyUI();
            }
        }
    }

    public void updateEnemyUI()
    {
        enemyUI.text = "Enemies: " + enemyCount;
    }

    public void updateWaveUI()
    {
        waveUI.text = "Wave: " + waveIndex;
    }

     public Vector3 RandomCircle(Vector3 center, float radius) {
     // create random angle between 0 to 360 degrees
     var ang = Random.value * 360;
     Vector3 pos;
     pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
     pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
     pos.y = center.y;
     return pos;
 }

    
}

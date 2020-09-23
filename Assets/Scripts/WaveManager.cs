using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public static int enemyCount;
    public int waveIndex = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check if wave is over
        Debug.Log(enemyCount);
        if (enemyCount <= 0) {
            //Start new wave
            Debug.Log("Starting wave " + waveIndex);
            var pos = RandomCircle(Vector3.zero, 5f);
            var rot = Quaternion.FromToRotation(Vector3.forward, Vector3.zero);
            waveIndex++;
            //start coroutine spawning enemies
            for (var i = 0; i < waveIndex; i++) {
                Instantiate(enemyPrefabs[0], pos, rot);
                enemyCount++;
            }
        }

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

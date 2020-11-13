using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave {
    //public float randomProb = 0f; //Probability of randomization
    //public Hostile[] possibleSpawns; //For randomizing based on difficulty
    public group[] spawns; //List of enemies to spawn, in order of spawning
    public int burrowNum = 1; //How many groups enemies should spawn in
}

[System.Serializable] 
public struct group {
    public Hostile hostile;
    public int num;
}
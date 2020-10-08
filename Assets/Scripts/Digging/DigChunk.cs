using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigChunk : MonoBehaviour
{
    //haha temp
    //Is the chunk dug out?
    public bool state;
    //X, Z position in grid
    public Vector2 pos;
    //Whether surrounding chunk walls are visible
    //Use this to simplify mesh construction
    public bool[] neighbors = new bool[] { false, false, false, false };

    //Constructor: take in position
    
    //Method: construct mesh
}

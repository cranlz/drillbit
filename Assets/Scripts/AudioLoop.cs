using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoop : MonoBehaviour
{
    public AudioSource intenseMusic;
    [Range(0, 1)]
    public float intensity;

    void Update()
    {
        intenseMusic.volume = intensity;
    }
}

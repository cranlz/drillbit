using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoop : MonoBehaviour
{
    public AudioSource intenseMusic;
    public AudioSource calmMusic;
    [Range(0, 1)]
    public float intensity;

    void Update()
    {
        //Crank up the intensity if enemies are around
        if (WaveManager.enemyCount > 0) { intensity = Mathf.Lerp(intensity, 1, 0.01f); } 
        else intensity = Mathf.Lerp(intensity, 0, 0.005f);

        intenseMusic.volume = intensity;
        calmMusic.volume = (1 - intensity) * 0.5f;
    }
}

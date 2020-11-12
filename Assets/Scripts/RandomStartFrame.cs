using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Utility script to offset animations
public class RandomStartFrame : MonoBehaviour
{
    private Animator anim;
    
    void Start() {
        anim = GetComponent<Animator>();
        anim.Play(0, 0, Random.value);
    }
}
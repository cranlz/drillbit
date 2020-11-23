using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ConcHostile : Hostile
{
    public float deployTime = 5f;
    public GameObject servantPrefab;

    public override void Start() {
        base.Start();

        StartCoroutine("deploy");
    }

    IEnumerator deploy() {
        yield return new WaitForSeconds(deployTime);
        var origSpeed = ai.maxSpeed;
        var origAnimSpeed = animator.speed;
        ai.maxSpeed = 0;
        animator.playbackTime = 0;
        animator.speed = 0;
        yield return new WaitForSeconds(1f);
        Instantiate(servantPrefab, transform.position, transform.rotation);
        Instantiate(servantPrefab, transform.position, transform.rotation);
        Instantiate(servantPrefab, transform.position, transform.rotation);
        Instantiate(servantPrefab, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        ai.maxSpeed = origSpeed;
        animator.speed = origAnimSpeed;
    }
}

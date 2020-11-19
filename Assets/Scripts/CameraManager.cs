using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{

    private Vector3 focusPoint;
    public List<Transform> targets;
    public GameObject player;
    public GameObject baseCollector;

    void Update() {
        focusPoint = Vector3.zero;
        /* OLD CODE
        //Stank method of getting the center-ish of multiple things
        foreach (var i in targets) {
            focusPoint += i.position;
        }
        focusPoint /= targets.Count + 1;
        focusPoint.y = transform.position.y;

        transform.position = Vector3.Lerp(transform.position, focusPoint, 0.5f);
        */

        //New, temporary system should focus on player and base collector
        //If base and player are close enough together, camera centers between them
        //Debug.Log((baseCollector.transform.position - player.transform.position).sqrMagnitude);
        if ((baseCollector.transform.position - player.transform.position).sqrMagnitude < 400) {
            focusPoint += baseCollector.transform.position;
            focusPoint += player.transform.position;
            focusPoint /= 2;
        } else { //Else focus on just the player
            focusPoint = player.transform.position;
        }
        focusPoint.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, focusPoint, 0.05f);
    }
}
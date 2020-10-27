using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{

    private Vector3 focusPoint = new Vector3(0, 0, 0);
    public List<Transform> targets;

    void Update() {

        //Stank method of getting the center-ish of multiple things
        foreach (var i in targets) {
            focusPoint += i.position;
        }
        focusPoint /= targets.Count + 1;
        focusPoint.y = transform.position.y;

        transform.position = Vector3.Lerp(transform.position, focusPoint, 0.5f);

    }
}
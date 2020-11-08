using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//When our towers become more varied, we can inherit from this
public class ConWeapon : Construct {

    public List<GameObject> targets = new List<GameObject>();
    public float rotationSpeed = 180.0f;
    private GameObject target;
    public Transform barrelExit;
    public int damage = 1;
    public float rateOfFire = 1.0f;
    public int maxFireAngle = 10;
    private float timer = 0.0f;
    private LineRenderer shotLine;
    private WaitForSeconds shotDuration = new WaitForSeconds(.05f);
    public float range = 50.0f;
    public GameObject bitToRotate;

    void Start() {
        shotLine = GetComponent<LineRenderer>();
    }

    void Update() {
        //Check if any enemies in range
        if (targets.Count != 0) { //Active state

            //NOTE: should this part only run when current target dies?
            //Remove any dead enemies
            targets = targets.Where(item => item != null).ToList();
            //Loop through possible targets and find the closest
            target = FindClosestHostile();
            //One last failsafe
            if(target == null) return;

            //Rotate to face target
            var lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var endRot = Quaternion.LookRotation(lookPos);
            var newRot = Quaternion.RotateTowards(bitToRotate.transform.rotation, endRot, Time.deltaTime * rotationSpeed);
            bitToRotate.transform.rotation = newRot;

            //Fire every rateOfFire seconds, but only if enemy is in our sights
            timer += Time.deltaTime;
            if (Vector3.Angle(bitToRotate.transform.forward, lookPos) < maxFireAngle && timer > rateOfFire) {
                //Shoot out our raycast and apply damage to enemy
                StartCoroutine(ShotEffect());
                RaycastHit hit;
                shotLine.SetPosition(0, transform.InverseTransformPoint(barrelExit.position));
                if (Physics.Raycast(barrelExit.position, bitToRotate.transform.forward, out hit, range)) {
                    shotLine.SetPosition(1, transform.InverseTransformPoint(hit.point));
                    Hostile hScript = hit.collider.GetComponent<Hostile>();
                    //Debug.Log("hit " + hit.collider.gameObject.name);
                    if (hScript != null) {
                        hScript.Damage(damage);
                        //Make sure to remove from targets if dead
                        if (hit.collider.GetComponent<Hostile>().hp <= 0) {
                            targets.Remove(hit.collider.gameObject);
                        }
                    }
                }
                else shotLine.SetPosition(1, bitToRotate.transform.InverseTransformPoint(bitToRotate.transform.forward * range));
                timer = 0f;
            }
        } else { //Idle state
            bitToRotate.transform.Rotate(0f, 0.1f, 0f, Space.Self);
        }

    }

    //Doing this to maintain a list of only the hostiles in range
    private void OnTriggerEnter(Collider col) {
        if (col.CompareTag("hostile")) targets.Add(col.gameObject);
    }
    private void OnTriggerExit(Collider col) {
        if (col.CompareTag("hostile")) targets.Remove(col.gameObject);
    }

    //Returns the closest hostile inside our trigger
    public GameObject FindClosestHostile() {
        var distance = float.MaxValue;
        var hostiles = GameObject.FindGameObjectsWithTag("hostile");
        GameObject closest = null;
        foreach (var i in hostiles) {
            var curDistance = (i.transform.position - transform.position).sqrMagnitude;
            if (curDistance < distance) {
                closest = i;
                distance = curDistance;
            }
        }
        return closest;
    }

    //Activate laser for our shotDuration
    private IEnumerator ShotEffect() {
        shotLine.enabled = true;
        yield return shotDuration;
        shotLine.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BasicTowerController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> targets = new List<GameObject>();
    public float rotationSpeed = 180.0f;
    private GameObject currentTarget;
    public Transform barrel;
    public int damage = 1;
    public float rateOfFire = 1.0f;
    private float timer = 0.0f;
    private LineRenderer shotLine;
    private WaitForSeconds shotDuration = new WaitForSeconds(.05f);
    public float range = 50.0f;

    void Start()
    {
        shotLine = GetComponent<LineRenderer>();
    }
    void Update()
    {
        //Check if any enemies in range
        if (targets.Count != 0)
        {
            var distance = float.MaxValue;
            GameObject target = null;

            //Remove any dead enemies
            targets = targets.Where(item => item != null).ToList();
            //Loop through possible targets and find the closest
            foreach (var i in targets)
            {
                var diff = i.transform.position - transform.position;
                var curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    target = i;
                    distance = curDistance;
                }
            }

            if(target == null) return;

            //Rotate to face target
            var lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var endRot = Quaternion.LookRotation(lookPos);
            var newRot = Quaternion.RotateTowards(transform.rotation, endRot, Time.deltaTime * rotationSpeed);
            transform.rotation = newRot;

            //Fire every rateOfFire seconds, but only if enemy is in our sights
            var angle = 10;
            timer += Time.deltaTime;
            if (Vector3.Angle(transform.forward, lookPos) < angle && timer > rateOfFire)
            {
                //Shoot out our raycast and apply damage to enemy
                StartCoroutine(ShotEffect());
                RaycastHit hit;
                shotLine.SetPosition(0, barrel.position);
                if (Physics.Raycast(barrel.position, transform.forward, out hit, range))
                {
                    shotLine.SetPosition(1, hit.point);
                    BasicEnemyController health = hit.collider.GetComponent<BasicEnemyController>();
                    if (health != null)
                    {
                        health.Damage(damage);
                        targets.Remove(target);
                        Destroy(target);
                    }
                }
                else
                {
                    shotLine.SetPosition(1, transform.forward * range);
                }
                timer = 0f;
            }
        }

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("enemy")) targets.Add(col.gameObject);
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("enemy")) targets.Remove(col.gameObject);
    }

    private IEnumerator ShotEffect()
    {
        shotLine.enabled = true;
        yield return shotDuration;
        shotLine.enabled = false;
    }
}

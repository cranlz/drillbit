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
    public Transform barrelExit;
    public int damage = 1;
    public float rateOfFire = 1.0f;
    public int maxFireAngle = 10;
    private float timer = 0.0f;
    private LineRenderer shotLine;
    private WaitForSeconds shotDuration = new WaitForSeconds(.05f);
    public float range = 50.0f;
    public int health = 10;
    public bool markedForDeletion = false;
    //public SphereCollider sphereCol;

    void Start()
    {
        shotLine = GetComponent<LineRenderer>();
        //sphereCol = GetComponent<SphereCollider>();
    }

    void Update()
    {
        //Check if any enemies in range
        if (targets.Count != 0) //Active state
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
            timer += Time.deltaTime;
            if (Vector3.Angle(transform.forward, lookPos) < maxFireAngle && timer > rateOfFire)
            {
                //Shoot out our raycast and apply damage to enemy
                StartCoroutine(ShotEffect());
                RaycastHit hit;
                shotLine.SetPosition(0, transform.InverseTransformPoint(barrelExit.position));
                if (Physics.Raycast(barrelExit.position, transform.forward, out hit, range))
                {
                    shotLine.SetPosition(1, transform.InverseTransformPoint(hit.point));
                    BasicEnemyController health = hit.collider.GetComponent<BasicEnemyController>();
                    //Debug.Log("hit " + hit.collider.gameObject.name);
                    if (health != null)
                    {
                        health.Damage(damage, gameObject);
                    }
                }
                else
                {
                    shotLine.SetPosition(1, transform.InverseTransformPoint(transform.forward * range));
                }
                timer = 0f;
            }
        } else //Idle state
        {
            //transform.Rotate(0f, 0.1f, 0f, Space.Self);
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
    public void Damage(int damageAmount, GameObject killer)
    {
        health -= damageAmount;
        if (health <= 0 && !markedForDeletion)
        {
            markedForDeletion = true;
            killer.GetComponent<BasicEnemyController>().updateDestination(gameObject);
            Destroy(gameObject);
        }
    }
}

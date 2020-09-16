using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTowerController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> targets = new List<GameObject>();
    public float range = 1.0f;
    public float rateOfFire = 1.0f;
    public float rotationSpeed = 120.0f;
    private GameObject currentTarget;
    void Start()
    {
        CircleCollider2D rangeCollider = this.gameObject.AddComponent<CircleCollider2D>();
        rangeCollider.radius = range;
        rangeCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if any enemies in range
        if (targets.Count != 0) 
        {
            var distance = float.MaxValue;
            GameObject target = null;

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
        //Rotate to face target
        var lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        var endRotation = Quaternion.LookRotation(lookPos);
        var startRotation = transform.rotation;
        //var angle = 

        //Fire every rateOfFire frames
        }
        
    }

    private void OnTriggerEnter (Collider col) {
        if (col.CompareTag("enemy")) targets.Add(col.gameObject);
    }
    private void OnTriggerExit(Collider col) {
        if (col.CompareTag("enemy")) targets.Remove(col.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Hostile : MonoBehaviour
{
    //integer value of a hostile's health
    public int hp;
    //Mark true if Destroy() has been called
    public bool markedForDeletion = false;
    //Current construct to target
    public GameObject target = null;
    //Pathfinding script
    private IAstarAI ai;
    //public GameObject waveM;
    
    public SpriteRenderer sprite;
    public WaitForSeconds flashDuration = new WaitForSeconds(.05f);
    public Material flashMat;

    public GameObject bloodPart;
    public GameObject flashPart;

    public int damage;
    public float attackRate;

    void Start() {
        
        ai = GetComponent<IAstarAI>();
        target = FindClosestConstruct();
        ai.destination = target.transform.position;
        InvokeRepeating("Attack", 0, attackRate);
    }

    //Deal some damage to the hostile
    //Can heal by using negative ints
    public void Damage(int dmg) {
        hp -= dmg;
        StartCoroutine(FlashEffect());
        //If damage causes the hostile's health to be
        // negative, destroy it
        if (hp <= 0 && !markedForDeletion) {
            Instantiate(bloodPart, gameObject.transform.position, gameObject.transform.rotation);
            Instantiate(flashPart, gameObject.transform.position, gameObject.transform.rotation);
            markedForDeletion = true;
            WaveManager.enemyCount -= 1;
            //waveM.updateUI();
            Destroy(gameObject);
        }
    }

    //Sets the current HP to a specific value
    public void SetHP(int newHP) {
        hp = newHP;
    }

    //Returns the closest GameObject tagged 'construct'
    //Optional argument to ignore a tower when searching
    public GameObject FindClosestConstruct(GameObject ignore = null) {
        var distance = float.MaxValue;
        var towers = GameObject.FindGameObjectsWithTag("construct");
        GameObject closest = null;
        foreach (var i in towers) {
            if(i == ignore) continue;
            var curDistance = (i.transform.position - transform.position).sqrMagnitude;
            if (curDistance < distance) {
                closest = i;
                distance = curDistance;
            }
        }
        return closest;


    }

    //Activate flash for our flashDuration
    private IEnumerator FlashEffect() {
        var original = sprite.material;
        sprite.material = flashMat;
        yield return flashDuration;
        sprite.material = original;
    }

    //should be called whenever our target is within range
    public void Attack() {
        //instead of doing all this, we really only need to deal a set amount
        //of damage to our target when in range. no need for raycasting.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f)) {
            Debug.DrawRay(transform.position, transform.forward * 2f);
            BasicTowerController towerScript = hit.collider.GetComponent<BasicTowerController>();
            BasicCollector collectorScript = hit.collider.GetComponent<BasicCollector>();
            if (towerScript != null) {
                towerScript.Damage(damage, gameObject);
                Debug.Log("hit " + hit.collider.gameObject.name);
            } else if (collectorScript != null) {
                collectorScript.Damage(damage, gameObject);
                Debug.Log("hit " + hit.collider.gameObject.name);
            }
        }

        //make sprite face the right way
        //should only need to be called when target changes!
        bool isLeft;
        if (ai.velocity.x == 0) {
            isLeft = sprite.flipX;
        } else if (ai.velocity.x < 0) {
            isLeft = true;
        } else isLeft = false;
        sprite.flipX = isLeft;
    }

    //It might be worth handing object pooling here for performance later
}

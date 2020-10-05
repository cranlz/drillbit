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
    
    private SpriteRenderer sprite;

    void Start() {
        ai = GetComponent<IAstarAI>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        target = FindClosestConstruct();
        ai.destination = target.transform.position;
    }

    //Deal some damage to the hostile
    //Can heal by using negative ints
    public void Damage(int dmg) {
        hp -= dmg;
        //If damage causes the hostile's health to be
        // negative, destroy it
        if (hp <= 0 && !markedForDeletion) {
            markedForDeletion = true;
            WaveManager.enemyCount -= 1;
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

    //It might be worth handing object pooling here for performance later
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BasicEnemyController : MonoBehaviour
{

    public int health = 1;
    private IAstarAI ai;
    public GameObject currentTarget;
    private bool markedForDeletion = false;
    public float attackRate = 0.5f;
    public int damage = 1;
    public GameObject spriteObject;
    private SpriteRenderer sprite;
    public GameObject waveManager;
    private WaveManager waveM;

    void Start()
    {
        currentTarget = null;
        ai = GetComponent<IAstarAI>();
        sprite = spriteObject.GetComponent<SpriteRenderer>();
        InvokeRepeating("Attack", 0, attackRate);
        waveManager = GameObject.Find("WaveManager");
        waveM = waveManager.GetComponent<WaveManager>();
        SetNewTarget();
    }

    public void Damage(int damageAmount, GameObject killer)
    {
        
        health -= damageAmount;
        if (health <= 0 && !markedForDeletion)
        {
            markedForDeletion = true;
            killer.GetComponent<BasicTowerController>().targets.Remove(gameObject);
            WaveManager.enemyCount--;
            waveM.updateEnemyUI();
            //BasicCollector.bank += 1;
            Destroy(gameObject);
        }
    }

    public GameObject SetNewTarget(GameObject tower = null)
    {
        var distance = float.MaxValue;
        var towers = GameObject.FindGameObjectsWithTag("construct");
        foreach (var i in towers)
            {
                if(tower != null && i == tower)
                {
                    Debug.Log("Detected Current Target");
                    continue;
                }
                var diff = i.transform.position - transform.position;
                var curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    currentTarget = i;
                    distance = curDistance;
                }
            }
        if (ai != null) ai.destination = currentTarget.transform.position;
        return currentTarget;
    }

    //should be called whenever our target is within range
    public void Attack()
    {
        //instead of doing all this, we really only need to deal a set amount
        //of damage to our target when in range. no need for raycasting.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            Debug.DrawRay(transform.position, transform.forward * 2f);
            BasicTowerController towerScript = hit.collider.GetComponent<BasicTowerController>();
            BasicCollector collectorScript = hit.collider.GetComponent<BasicCollector>();
            if (towerScript != null)
            {
                towerScript.Damage(damage, gameObject);
                Debug.Log("hit " + hit.collider.gameObject.name);
            }
            else if (collectorScript != null)
            {
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
}

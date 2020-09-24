using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyController : MonoBehaviour
{

    public int health = 1;
    private NavMeshAgent agent;
    public GameObject currentTarget;
    private bool markedForDeletion = false;
    public float attackRate = 0.5f;
    public int damage = 1;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(SetNewTarget().transform.position);
        InvokeRepeating("Attack", 0, attackRate);
    }

    public void Damage(int damageAmount, GameObject killer)
    {
        health -= damageAmount;
        if (health <= 0 && !markedForDeletion)
        {
            markedForDeletion = true;
            killer.GetComponent<BasicTowerController>().targets.Remove(gameObject);
            WaveManager.enemyCount--;
            Destroy(gameObject);
        }
    }

    private GameObject SetNewTarget()
    {
        var distance = float.MaxValue;
        var towers = GameObject.FindGameObjectsWithTag("construct");
        foreach (var i in towers)
            {
                var diff = i.transform.position - transform.position;
                var curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    currentTarget = i;
                    distance = curDistance;
                }
            }
        return currentTarget;
    }

    public void Attack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1))
        {
            BasicTowerController towerScript = hit.collider.GetComponent<BasicTowerController>();
            BasicCollector collectorScript = hit.collider.GetComponent<BasicCollector>();
            Debug.Log("hit " + hit.collider.gameObject.name);
            if (towerScript != null)
            {
                towerScript.Damage(damage, gameObject);
            }
            else if (collectorScript != null)
            {
                collectorScript.Damage(damage, gameObject);
            }
        }
    }
}

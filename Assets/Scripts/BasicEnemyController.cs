﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyController : MonoBehaviour
{

    public int health = 1;
    private NavMeshAgent agent;
    public GameObject currentTarget;
    private bool markedForDeletion = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(getClosest().transform.position);
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

    private GameObject getClosest()
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
}

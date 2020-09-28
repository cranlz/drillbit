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
    public float attackRate = 0.5f;
    public int damage = 1;
    public GameObject spriteObject;
    private SpriteRenderer sprite;
    public GameObject waveManager;
    private WaveManager waveM;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(SetNewTarget().transform.position);
        sprite = spriteObject.GetComponent<SpriteRenderer>();
        InvokeRepeating("Attack", 0, attackRate);
        waveManager = GameObject.Find("WaveManager");
        waveM = waveManager.GetComponent<WaveManager>();
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
            BasicCollector.bank += 2;
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
        bool isLeft;
        if (agent.velocity.x == 0) {
            isLeft = sprite.flipX;
        } else if (agent.velocity.x < 0) {
            isLeft = true;
        } else isLeft = false;
        sprite.flipX = isLeft;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public LayerMask whatCanBeClickedOn;
    private NavMeshAgent agent;
    public GameObject[] towers;
    public int towerCost = 5;
    private Transform buildPreview;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        buildPreview = transform.Find("BuildPreview");
    }

    void Update() {
        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButton(1)) {
            //rotate to mouse
            if (Physics.Raycast(myRay, out hit, 100, whatCanBeClickedOn)) {
                var lookPos = hit.point - transform.position;
                lookPos.y = 0;
                transform.rotation = Quaternion.LookRotation(lookPos);
            }
            if (Input.GetMouseButtonDown(0) && BasicCollector.bank >= towerCost) {
                Instantiate(towers[0], buildPreview.position, buildPreview.rotation);
                BasicCollector.bank -= towerCost;
            }
        }
        else if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(myRay, out hit, 100, whatCanBeClickedOn)) {
                agent.SetDestination(hit.point);
            }
        }


        if (Input.GetMouseButtonDown(1)) {
            buildPreview.gameObject.SetActive(true);
        }
        if (Input.GetMouseButtonUp(1)) {
            buildPreview.gameObject.SetActive(false);
        }
    }
}

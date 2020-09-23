using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public LayerMask whatCanBeClickedOn;
    private NavMeshAgent myAgent;
    public GameObject enemy;
    public GameObject[] towers;
    public int towerCost = 5;
    private Transform buildPreview;
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        buildPreview = transform.Find("BuildPreview");
    }

    void Update() {
        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Input.GetMouseButton(1)) {
            //rotate to mouse
            if (Physics.Raycast(myRay, out hitInfo, 100, whatCanBeClickedOn)) {
                var lookPos = hitInfo.point - transform.position;
                lookPos.y = 0;
                transform.rotation = Quaternion.LookRotation(lookPos);
            }
            if (Input.GetMouseButtonDown(0) && BasicCollector.bank >= towerCost) {
                Instantiate(towers[0], buildPreview.position, buildPreview.rotation);
                BasicCollector.bank -= towerCost;
            }
        }
        else if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(myRay, out hitInfo, 100, whatCanBeClickedOn)) {
                myAgent.SetDestination(hitInfo.point);
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

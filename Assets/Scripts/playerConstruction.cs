using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class should handle all player controls dealing with tower construction and placement
public class playerConstruction : MonoBehaviour {
    public LayerMask whatCanBeClickedOn;
    //What are we able to build?
    public constructType[] constructs;

    private Transform buildPreview;
    private constructType currentConstruct;

    private void Start() {
        buildPreview = transform.Find("BuildPreview");
        currentConstruct = constructs[0];
    }

    void Update() {
        //Switch whatever construct we want with num keys
        var num = getNumDown();
        if (num > -1) {
            currentConstruct = constructs[(num - 1) % constructs.Length];
        }

        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButton(1)) {
            //Show build preview
            //Rotate to mouse
            if (Physics.Raycast(myRay, out hit, 100, whatCanBeClickedOn)) {
                var lookPos = hit.point - transform.position;
                lookPos.y = 0;
                transform.rotation = Quaternion.LookRotation(lookPos);
            }
            if (Input.GetMouseButtonDown(0) && ConCollector.bank >= currentConstruct.cost) {
                //Build our new construct
                //Handle position stuff here

                GameObject newTower;
                //If the construct is a collector, make sure it is on a resource deposit
                if (currentConstruct.tag == "collector") {
                    var deposit = closestDeposit();
                    if (deposit != null && (deposit.transform.position - buildPreview.position).sqrMagnitude < 5) {
                        newTower = Instantiate(currentConstruct.prefab, deposit.transform.position, deposit.transform.rotation);
                        deposit.SetActive(false);
                    ConCollector.bank -= currentConstruct.cost;
                    Camera.main.GetComponent<CameraManager>().targets.Add(newTower.transform);
                    Debug.Log("made collector");
                    }
                } else {
                    //make sure our tower isn't colliding with anything
                    Collider[] hitColliders = Physics.OverlapBox(buildPreview.position, buildPreview.localScale / 2, buildPreview.rotation);
                    //Debug.Log(hitColliders.Length);
                    if (hitColliders.Length == 0) {
                        var spawnPos = buildPreview.position;
                        spawnPos.y = 0f;
                        newTower = Instantiate(currentConstruct.prefab, spawnPos, buildPreview.rotation);
                        ConCollector.bank -= currentConstruct.cost;
                        Camera.main.GetComponent<CameraManager>().targets.Add(newTower.transform);
                        Debug.Log("made construct");
                    }
                }
            }
        }

        //Enable our preview object
        if (Input.GetMouseButtonDown(1)) {
            buildPreview.gameObject.SetActive(true);
        }
        if (Input.GetMouseButtonUp(1)) {
            buildPreview.gameObject.SetActive(false);
        }
    }

    //This is just to get the number keys in a more sophistocated, efficient manner.
    //Returns -1 if no keys are pressed, 0-9 if a key has been pressed.
    public int getNumDown() {
        var num = -1;
        if (Input.GetKeyDown(KeyCode.Alpha1)) { num = 1; return num; };
        if (Input.GetKeyDown(KeyCode.Alpha2)) { num = 2; return num; };
        if (Input.GetKeyDown(KeyCode.Alpha3)) { num = 3; return num; };
        if (Input.GetKeyDown(KeyCode.Alpha4)) { num = 4; return num; };
        if (Input.GetKeyDown(KeyCode.Alpha5)) { num = 5; return num; };
        if (Input.GetKeyDown(KeyCode.Alpha6)) { num = 6; return num; };
        if (Input.GetKeyDown(KeyCode.Alpha7)) { num = 7; return num; };
        if (Input.GetKeyDown(KeyCode.Alpha8)) { num = 8; return num; };
        if (Input.GetKeyDown(KeyCode.Alpha9)) { num = 9; return num; };
        if (Input.GetKeyDown(KeyCode.Alpha0)) { num = 0; return num; };
        return num;
    }

    public GameObject closestDeposit() {
        var deposits = GameObject.FindGameObjectsWithTag("deposit");
        var distance = float.MaxValue;
        GameObject closest = null;
        if (deposits != null) {
            foreach (var deposit in deposits) {
                var curDistance = (deposit.transform.position - buildPreview.position).sqrMagnitude;
                if (curDistance < distance) {
                    closest = deposit;
                    distance = curDistance;
                }
            }
        }
        return closest;
    }
}

//Class for holding constructable data
[System.Serializable]
public class constructType {
    //What specific construct are you?
    public GameObject prefab;
    //Are you a collector? bomb? etc?
    public string tag;
    //What number key should we press to select you?
    public int slotNum;
    //How much do you cost?
    public int cost;
}
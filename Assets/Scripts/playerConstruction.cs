using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class should handle all player controls dealing with tower construction and placement
public class playerConstruction : MonoBehaviour {
    public LayerMask whatCanBeClickedOn;
    //What are we able to build?
    public GameObject[] constructs;
    //Break this up into a per-tower basis
    public int towerCost = 5;

    private Transform buildPreview;
    private GameObject currentConstruct;

    private void Start() {
        buildPreview = transform.Find("BuildPreview");
        currentConstruct = constructs[0];
    }

    void Update() {
        var num = getNumDown();
        if (num > -1) {
            currentConstruct = constructs[(num - 1) % constructs.Length];
        }

        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButton(1)) {
            //rotate to mouse
            if (Physics.Raycast(myRay, out hit, 100, whatCanBeClickedOn)) {
                var lookPos = hit.point - transform.position;
                lookPos.y = 0;
                transform.rotation = Quaternion.LookRotation(lookPos);
            }
            if (Input.GetMouseButtonDown(0) && ConCollector.bank >= towerCost) {
                var newTower = Instantiate(currentConstruct, buildPreview.position, buildPreview.rotation);
                ConCollector.bank -= towerCost;
                Camera.main.GetComponent<CameraManager>().targets.Add(newTower.transform);
                Debug.Log("made tower");
            }
        }

        

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
}

//Class for holding constructable data
public class constructType {
    //What specific construct are you?
    public GameObject prefab;
    //Are you a collector? bomb? etc?
    public string tag;
    //What number key should we press to select you?
    public int slotNum;
}
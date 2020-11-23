using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConCollector : Construct {
    public float setupTime = 1.0f;
    public float rateOfCollection = 1.0f;
    public int collectionAmount = 1;
    public static int bank = 0;
    public GameObject text;
    private TextMeshPro textMesh;
    public static bool collecting = false;

    //Get any components, and start collecting after setupTime
    void Start() {
        textMesh = text.GetComponent<TextMeshPro>();
        InvokeRepeating("Collect", setupTime, rateOfCollection);
    }

    //Might be worth getting rid of this in favor of updating
    // only when we need to
    void Update() {
        textMesh.SetText("{0}", bank);
        if (Input.GetKeyDown(KeyCode.C)) {
            collecting = true;
        }
    }

    //Self explanatory
    private void Collect() {
        if (collecting) {
            bank += collectionAmount;
        }
    }
}

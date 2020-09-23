using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicCollector : MonoBehaviour
{
    public float rateOfCollection = 1.0f;
    public int collectionAmount = 1;
    public static int bank = 0;
    public GameObject text;
    private TextMeshPro textMesh;
    void Start()
    {
        textMesh = text.GetComponent<TextMeshPro>();
        InvokeRepeating("Collect", rateOfCollection, rateOfCollection);
    }

    private void Collect()
    {
        bank += collectionAmount;
        textMesh.SetText("{0}", bank);
    }
}

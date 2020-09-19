using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCollector : MonoBehaviour
{
    public float rateOfCollection = 1.0f;
    public int collectionAmount = 1;
    public int bank = 0;
    void Start()
    {
        InvokeRepeating("Collect", rateOfCollection, rateOfCollection);
    }

    private void Collect()
    {
        bank += collectionAmount;
    }
}

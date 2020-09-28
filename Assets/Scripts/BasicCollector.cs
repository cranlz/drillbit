using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicCollector : MonoBehaviour
{
    public float rateOfCollection = 1.0f;
    public int collectionAmount = 1;
    public static int bank = 10;
    public GameObject text;
    private TextMeshPro textMesh;
    public float health = 10f;
    public bool markedForDeletion = false;

    void Start()
    {
        textMesh = text.GetComponent<TextMeshPro>();
        InvokeRepeating("Collect", rateOfCollection, rateOfCollection);
    }

    void Update()
    {
        textMesh.SetText("{0}", bank);
    }

    private void Collect()
    {
        bank += collectionAmount;
    }
    public void Damage(int damageAmount, GameObject killer)
    {
        health -= damageAmount;
        if (health <= 0 && !markedForDeletion)
        {
            markedForDeletion = true;
            //killer.GetComponent<BasicTowerController>().targets.Remove(gameObject);
            gameObject.SetActive(false);
        }
    }
}

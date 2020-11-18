using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BuildingComponent", menuName = "BuildingComponent", order = 1)]
public class BuildingComponent : ScriptableObject
{
    public GameObject PreviewPrefab;
    public GameObject BuildingElementPrefab;
    public CircularMenuElement menuElement;
}

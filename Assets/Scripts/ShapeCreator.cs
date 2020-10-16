using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sebastian.Geometry;

public class ShapeCreator : MonoBehaviour {
    public MeshFilter meshFilterHoles;
    public MeshFilter meshFilterSolid;

    [HideInInspector]
    public List<Shape> shapes = new List<Shape>();
    [HideInInspector]
    public bool showShapesList;

    public float handleRadius = .1f;

    public void UpdateMeshDisplay() {
        CompositeHoleShape compHoleShape = new CompositeHoleShape(shapes);
        meshFilterHoles.mesh = compHoleShape.GetMesh();
        AssetDatabase.CreateAsset( compHoleShape.GetMesh(), "Assets/Resources/HoleMesh.asset" );
        AssetDatabase.SaveAssets();

        CompositeSolidShape compSolidShape = new CompositeSolidShape(shapes);
        meshFilterSolid.mesh = compSolidShape.GetMesh();
    }
}

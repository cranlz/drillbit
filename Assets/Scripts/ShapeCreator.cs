using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sebastian.Geometry;
using ClipperLib;

public class ShapeCreator : MonoBehaviour {
    public MeshFilter meshFilterHoles;
    public MeshFilter meshFilterSolid;
    public AstarPath path;

    public bool updateAIGraph = false;

    [HideInInspector]
    public List<Shape> shapes = new List<Shape>();
    [HideInInspector]
    public bool showShapesList;

    public float handleRadius = .1f;

    public void UpdateMeshDisplay() {
        CompositeHoleShape compHoleShape = new CompositeHoleShape(shapes);
        meshFilterHoles.mesh = compHoleShape.GetMesh();

        meshFilterHoles.gameObject.GetComponent<MeshCollider>().sharedMesh = null;
        meshFilterHoles.gameObject.GetComponent<MeshCollider>().sharedMesh = meshFilterHoles.sharedMesh;

        //Saving a mesh to our filesystem for navmesh creation
        //AssetDatabase.CreateAsset( compHoleShape.GetMesh(), "Assets/Resources/HoleMesh.asset" );
        //AssetDatabase.SaveAssets();

        CompositeSolidShape compSolidShape = new CompositeSolidShape(shapes);
        meshFilterSolid.mesh = compSolidShape.GetMesh();

        meshFilterSolid.gameObject.GetComponent<MeshCollider>().sharedMesh = null;
        meshFilterSolid.gameObject.GetComponent<MeshCollider>().sharedMesh = meshFilterSolid.sharedMesh;

        if (updateAIGraph) path.Scan();
    }
}

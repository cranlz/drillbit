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

    private void OnMouseUp()
    {
        //var cookieCutter = List<IntPoint>
    }

    //Convert our polygon to a list of IntPoints
    public List<IntPoint> ShapeToPath(Shape shape) {
        var points = new List<IntPoint>();
        for(var i = 0; i < shape.points.Count; i++) {
            points.Add(new IntPoint(shape.points[i].x, shape.points[i].z));
        }
        return points;
    }

    //Convert a list of IntPoints back to a shape
    public Shape PathToShape(List<IntPoint> points) {
        var shape = new Shape();
        for(var i = 0; i < points.Count; i++) {
            shape.points.Add(new Vector3(points[i].X, 0f, points[i].Y));
        }
        return shape;
    }

    /*
    public Paths ClipperSolution(ClipType type, ) {

    }
    */
}

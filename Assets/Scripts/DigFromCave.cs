using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sebastian.Geometry;
using ClipperLib;

public class DigFromCave : MonoBehaviour
{
    public ShapeCreator caveMeshes;

    private void OnMouseUp()
    {
        //var cookieCutter = List<IntPoint>
        
        Mesh digObject = gameObject.GetComponent<MeshFilter>().mesh;
        Debug.Log(digObject.name);

        var shapeToCut = ShapeToPath(caveMeshes.shapes[0]);
        var cookieCutter = new List<IntPoint>();
        cookieCutter.Add(new IntPoint(gameObject.transform.position.x+2, gameObject.transform.position.z));
        cookieCutter.Add(new IntPoint(gameObject.transform.position.x, gameObject.transform.position.z));
        cookieCutter.Add(new IntPoint(gameObject.transform.position.x, gameObject.transform.position.z+2));
        cookieCutter.Add(new IntPoint(gameObject.transform.position.x+2, gameObject.transform.position.z+2));

        List<List<IntPoint>> solution = new List<List<IntPoint>>();
        Clipper c = new Clipper();
        c.AddPath(shapeToCut, PolyType.ptSubject, true);
        c.AddPath(cookieCutter, PolyType.ptClip, true);
        c.Execute(ClipType.ctDifference, solution, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);
        caveMeshes.shapes[0] = PathToShape(solution[0]);
        caveMeshes.UpdateMeshDisplay();
    }

    //Convert our polygon to a list of IntPoints
    public List<IntPoint> ShapeToPath(Shape shape)
    {
        var points = new List<IntPoint>();
        for (var i = 0; i < shape.points.Count; i++)
        {
            points.Add(new IntPoint(shape.points[i].x, shape.points[i].z));
        }
        return points;
    }

    //Convert a list of IntPoints back to a shape
    public Shape PathToShape(List<IntPoint> points)
    {
        var shape = new Shape();
        for (var i = 0; i < points.Count; i++)
        {
            shape.points.Add(new Vector3(points[i].X, 0f, points[i].Y));
        }
        return shape;
    }

    /*
    public Paths ClipperSolution(ClipType type, ) {

    }
    */
}

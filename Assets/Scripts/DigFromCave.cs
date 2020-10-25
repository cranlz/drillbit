using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sebastian.Geometry;
using ClipperLib;
using System.Runtime;

public class DigFromCave : MonoBehaviour {
    public ShapeCreator caveMeshes;
    public Shape cookieCutter = new Shape();
    public float circAng = 10f;
    public float circRad = 5f;
    public bool snip = false;

    private void Start() {

    }

    private void Update()
    {
        //Update our cookiecutter points
        cookieCutter.points.Clear();
        var ang = 0f;
        while (ang < 360f) {
            var posX = circRad * Mathf.Sin(ang * Mathf.Deg2Rad) + gameObject.transform.position.x;
            var posY = circRad * Mathf.Cos(ang * Mathf.Deg2Rad) + gameObject.transform.position.z;
            cookieCutter.points.Add(new Vector3(posX, 0f, posY));
            ang += circAng;
        }

        if (snip) {
            snip = false;
            var shapeToCut = ShapeToPath(caveMeshes.shapes[1]);
            var cookieCutterShape = ShapeToPath(cookieCutter);

            var solution = new List<List<IntPoint>>();
            var c = new Clipper();

            if (!c.AddPath(shapeToCut, PolyType.ptSubject, true)) Debug.Log("path 1 failure");
            if (!c.AddPath(cookieCutterShape, PolyType.ptClip, true)) Debug.Log("path 2 failure");

            if (!c.Execute(ClipType.ctUnion, solution, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd)) Debug.Log("solution failure");

            caveMeshes.shapes[1] = PathToShape(solution[0]);
            caveMeshes.UpdateMeshDisplay();
        }
    }

    private void OnDrawGizmos()
    {
        foreach( var i in cookieCutter.points)
        {
            Gizmos.DrawSphere(i, 0.1f);
        }
    }

    //Convert our polygon to a list of IntPoints
    public List<IntPoint> ShapeToPath(Shape shape)
    {
        var points = new List<IntPoint>();
        for (var i = 0; i < shape.points.Count; i++)
        {
            //Convert each float to a long, but retain 4 decimal points of accuracy
            var s = string.Format("{0:0.0000}", shape.points[i].x);
            long posX = long.Parse(s.Replace(".", ""));
            s = string.Format("{0:0.0000}", shape.points[i].z);
            long posY = long.Parse(s.Replace(".", ""));
            //Debug.Log(posX + " " + posY);
            points.Add(new IntPoint(posX, posY));
        }
        Debug.Log(points.Count);
        return points;
    }

    //Convert a list of IntPoints back to a shape
    public Shape PathToShape(List<IntPoint> points)
    {
        var shape = new Shape();
        for (var i = 0; i < points.Count; i++)
        {
            //Convert each long back into a float, and get back our 4 decimal points
            var posX = points[i].X / 10000f;
            var posY = points[i].Y / 10000f;
            //Debug.Log(posX + " " + posY);
            shape.points.Add(new Vector3(posX, 0f, posY));
        }
        return shape;
    }
}

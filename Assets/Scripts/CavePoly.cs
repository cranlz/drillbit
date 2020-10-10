using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavePoly : MonoBehaviour
{
    public Vector2[] verts;
    public Vector2[] outer; 
    public float dotRadius = 0.1f;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector3 lastVert = new Vector3(0, 0, 0);
        foreach(var i in verts) {
            var temp = new Vector3(i.x, 0f, i.y);
            if(lastVert != Vector3.zero) {
                Gizmos.DrawLine(lastVert, temp);
            }
            Gizmos.DrawSphere(temp, dotRadius);
            lastVert = temp;
        }
        lastVert = Vector3.zero;
        foreach(var i in outer) {
            var temp = new Vector3(i.x, 0f, i.y);
            if(lastVert != Vector3.zero) {
                Gizmos.DrawLine(lastVert, temp);
            }
            Gizmos.DrawSphere(temp, dotRadius);
            lastVert = temp;
        }
    }
}

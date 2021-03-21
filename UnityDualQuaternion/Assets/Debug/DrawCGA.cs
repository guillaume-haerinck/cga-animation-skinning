using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlgeoSharp;

public class DrawCGA : MonoBehaviour
{
    AlgeoSharp.Visualization.DebugDrawGizmo ddraw;

    void Awake()
    {
        ddraw = GetComponent<AlgeoSharp.Visualization.DebugDrawGizmo>();
        if (ddraw == null)
            Debug.LogError("DebugDrawGizmo script missing !");
    }

    void Start()
    {
        // Create some points
        MultiVector p1 = IPNS.CreatePoint(2, 3, 0);
        MultiVector p2 = IPNS.CreatePoint(-3, -4, 3);
        MultiVector p3 = IPNS.CreatePoint(1, -5, 0);
        MultiVector p4 = IPNS.CreatePoint(-1, -2, -3);

        ddraw.Add(p1, Color.yellow);
        ddraw.Add(p2, Color.yellow);
        ddraw.Add(p3, Color.yellow);
        ddraw.Add(p4, Color.red);

        // Calculate sphere with four points
        MultiVector s = (p1 ^ p2 ^ p3 ^ p4).Dual;

        // Calculate plane with three points
        MultiVector p = (p1 ^ p2 ^ p3 ^ Basis.E8).Dual;

        // Add sphere and plane
        ddraw.Add(s, Color.gray);
        ddraw.Add(p, Color.cyan);

        // Calculate circle defined by intersection of the sphere and the plane and add it
        ddraw.Add(s ^ p, Color.yellow);

        // Calculate line through two points and add it
        ddraw.Add((p1 ^ p2 ^ Basis.E8).Dual, Color.white);
    }
}

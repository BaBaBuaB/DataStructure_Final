using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class NodePath
{
    public Vector2 position;
    public NodePath parent;
    public float gCost;
    public float hCost;
    public float fCost => gCost + hCost;

    public NodePath(Vector2 pos)
    {
        position = pos;
    }
}

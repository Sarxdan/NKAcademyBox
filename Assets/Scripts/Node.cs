using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Node : IComparable<Node>
{
    public int _gridX, _gridY;
    public Vector3 Position;
    public bool IsWalkable;
    public float gCost = 0; 
    public float hCost;
    
    public float fCost 
    {
        get {return gCost+hCost;}
    }

    public Node Parent;

    public Node(bool isWalkable, Vector3 position, int gridX, int gridY)
    {
        IsWalkable = isWalkable;
        Position = position;
        _gridX = gridX;
        _gridY = gridY;
    }

    public int CompareTo(Node other)
    {
        return fCost.CompareTo(other.fCost);
    }
}

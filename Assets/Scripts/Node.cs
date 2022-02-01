using UnityEngine;

public class Node
{
    private int _gridX, _gridY;
    public Vector3 Position;
    public bool IsWalkable;
    public int gCost, hCost;
    
    public int fCost {get {return gCost+hCost;}}

    public Node Parent;

    public Node(bool isWalkable, Vector3 position, int gridX, int gridY)
    {
        IsWalkable = isWalkable;
        Position = position;
        _gridX = gridX;
        _gridY = gridY;
    }
}

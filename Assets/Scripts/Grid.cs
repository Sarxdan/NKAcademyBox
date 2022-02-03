using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public LayerMask WallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float Distance;

    public float nodeDiameter;
    public int gridSizeX, gridSizeY;

    public List<Node> finalPath;


    //public int gridWidth = 30;
    //[FormerlySerializedAs("nodeRadius")] public int nodesPerRow = 50; // Number of tiles per radius
    //public int _nodeWidth; // Width per tile

    public Node[,] _grid;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        _grid = new Node[gridSizeX, gridSizeY];
        
        InitiateGrid();
    }

    public void InitiateGrid()
    {
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 -
                             Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
                                     Vector3.forward * (y * nodeDiameter + nodeRadius);

                bool Wall = !Physics.CheckSphere(worldPoint, nodeRadius, WallMask);

                _grid[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 3, gridWorldSize.y));

        if (_grid != null)
        {
            foreach (Node node in _grid)
            {
                if (node.IsWalkable)
                {
                    Gizmos.color = Color.yellow;
                }
                else
                {
                    Gizmos.color = Color.black;
                }

                if (finalPath != null)
                {
                    Gizmos.color = Color.red;
                }
                
                Gizmos.DrawCube(node.Position, Vector3.one*(nodeDiameter - Distance));
                
            }
        }
    }

    public Node GetNodeFromPos(Vector3 pos)
    {
        /*float xPoint = ((pos.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float yPoint = ((pos.y + gridWorldSize.y / 2) / gridWorldSize.y);

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeX - 1) * yPoint);*/


        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                bool xMatch = (pos.x > _grid[x, y].Position.x - nodeRadius) &&
                             (pos.x < _grid[x, y].Position.x + nodeRadius);
                bool yMatch = (pos.y > _grid[x, y].Position.y - nodeRadius) &&
                             (pos.y < _grid[x, y].Position.y + nodeRadius);

                if (xMatch && yMatch)
                {
                    return _grid[x, y];
                }
            }
        }

        return null;
    }
}

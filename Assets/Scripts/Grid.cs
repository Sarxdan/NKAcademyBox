using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public int gridWidth = 30;
    [FormerlySerializedAs("nodeRadius")] public int nodesPerRow = 50; // Number of tiles per radius
    public int _nodeWidth; // Width per tile

    public Node[,] _grid;


    public Grid()
    {
        for (int x = 0; x <= gridWidth; x++)
        {
            for (int y = 0; y <= gridWidth; y++)
            {
                _grid[x, y] = new Node(true, Vector3.zero, x, y); // Calculate position
            }
        }
    }

    public Node GetNodeFromPos(Vector3 pos)
    {
        return _grid[0, 0]; // Update to correct node
    }
}

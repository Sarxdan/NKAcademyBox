using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public int gridWidth = 30;
    public int nodeRadius = 50; // Number of tiles per radius
    private int _nodeWidth; // Width per tile

    private Node[,] _grid;


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

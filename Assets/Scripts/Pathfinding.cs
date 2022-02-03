using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public GameObject gridGO;
    private Grid _grid;
    
    public Transform startPos;
    public Transform goalPos;

    private List<Node> _finalPath; // READ FROM BACK TO FRONT

    private void Start()
    {
        _grid = gridGO.GetComponent<Grid>();
        _finalPath = new List<Node>();
        FindPath();
    }

    private void FindPath()
    {
        Node start = _grid.GetNodeFromPos(startPos.position);
        Node goal = _grid.GetNodeFromPos(goalPos.position);

        start.gCost = 0;
        start.hCost = 0;

        PriorityQueue open = new PriorityQueue();
        open.Enqueue(start);

        while (open.Count != 0)
        {
            Node currentNode = open.Dequeue();
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    int neighbourX = currentNode._gridX + x;
                    int neighbourY = currentNode._gridY + y;
                    // Check if in range and actually a neighbour
                    //TODO Add condition to ignore if it's the currentNode

                    if (neighbourX >= 0 && neighbourX <= _grid.gridSizeX-1 && neighbourY >= 0 && neighbourY <= _grid.gridSizeY-1)
                    {
                        Node node = _grid._grid[neighbourX, neighbourY];
                        
                        // Wall check
                        if (!node.IsWalkable)
                        {
                            continue;
                        }
                        
                        // If goal is reach, end algorithm and build the path
                        if (node == goal)
                        {
                            node.Parent = currentNode;
                            BuildPath(node, start);
                            return;
                        }
                        
                        // Cost from start -> current
                        float gCost = currentNode.gCost + 1;
                        
                        // Diagonal distance heuristic
                        int dx = Math.Abs(node._gridX - goal._gridX);
                        int dy = Math.Abs(node._gridY - goal._gridY);
                        float hCost = _grid.nodeDiameter * (dx + dy) + ((float)Math.Sqrt(_grid.nodeDiameter * 2) - 2 * _grid.nodeDiameter) * Math.Min(dx,dy);
                        
                        // Manhattan distance
                        //node.hCost = Math.Abs(node._gridX - goal._gridX) + Math.Abs(node._gridY - goal._gridY);

                        // Check if this path to node is better than previously documented
                        if (node.fCost == 0.0f || gCost + hCost < node.fCost)
                        {
                            node.Parent = currentNode;
                            node.gCost = gCost;
                            node.hCost = hCost;

                            if (!open.Contains(node))
                            {
                                open.Enqueue(node);
                            }
                        }
                    }
                }
            }
        }

    }

    private void BuildPath(Node current, Node start)
    {
        while (current != null)
        {
            _finalPath.Add(current);
            if (current != start)
            {
                current = current.Parent;
            }
            else
            {
                break;
            }
        }

        _grid.finalPath = _finalPath;
    }
}


 public class PriorityQueue
    {
        private readonly List<Node> _pq = new List<Node>();

        public int Count => _pq.Count;

        public void Enqueue(Node item)
        {
            _pq.Add(item);
            BubbleUp();
        }

        private void BubbleUp()
        {
            var childIndex = _pq.Count - 1;
            while (childIndex > 0)
            {
                var parentIndex = (childIndex - 1) / 2;
                if (_pq[childIndex].CompareTo(_pq[parentIndex]) >= 0)
                {
                    break;
                }
                Swap(childIndex, parentIndex);
                childIndex = parentIndex;
            }
        }

        public Node Dequeue()
        {
            var highestPrioritizedItem = _pq[0];

            MoveLastItemToTheTop();
            SinkDown();

            return highestPrioritizedItem;
        }

        public bool Contains(Node item)
        {
            foreach (Node searchItem in _pq)
            {
                if (searchItem == item)
                {
                    return true;
                }
            }
            
            return false;
        }

        private void MoveLastItemToTheTop()
        {
            var lastIndex = _pq.Count - 1;
            _pq[0] = _pq[lastIndex];
            _pq.RemoveAt(lastIndex);
        }

        private void SinkDown()
        {
            var lastIndex = _pq.Count - 1;
            var parentIndex = 0;

            while (true)
            {
                var firstChildIndex = parentIndex * 2 + 1;
                if (firstChildIndex > lastIndex)
                {
                    break;
                }
                var secondChildIndex = firstChildIndex + 1;
                if (secondChildIndex <= lastIndex && _pq[secondChildIndex].CompareTo(_pq[firstChildIndex]) < 0)
                {
                    firstChildIndex = secondChildIndex;
                }
                if (_pq[parentIndex].CompareTo(_pq[firstChildIndex]) < 0)
                {
                    break;
                }
                Swap(parentIndex, firstChildIndex);
                parentIndex = firstChildIndex;
            }
        }

        private void Swap(int index1, int index2)
        {
            (_pq[index1], _pq[index2]) = (_pq[index2], _pq[index1]);
        }
    }

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public GameObject gridGO;
    private Grid grid;
    
    public Transform startPos;
    public Transform goalPos;
    public Node startNode;

    public List<Node> finalPath; // READ FROM BACK TO FRONT

    private void Start()
    {
        grid = gridGO.GetComponent<Grid>();
        FindPath();
    }

    public void FindPath()
    {
        Node start = grid.GetNodeFromPos(startPos.position);
        Node goal = grid.GetNodeFromPos(goalPos.position);

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
                    //TODO Clean this shit up

                    int neighbourX = currentNode._gridX + x;
                    int neighbourY = currentNode._gridY + y;
                    // Check if in range
                    if (neighbourX > 0 && neighbourX <= grid.gridSizeX-1 && neighbourY + y > 0 && neighbourY + y <= grid.gridSizeY-1)
                    {
                        Node node = grid._grid[neighbourX, neighbourY];
                        
                        // Wall check
                        if (!node.IsWalkable)
                        {
                            continue;
                        }
                        
                        if (node == goal)
                        {
                            BuildPath(node, start);
                            return;
                        }
                        
                        float gCost = currentNode.gCost + 1;
                        // Diagonal distance heuristic
                        int dx = Math.Abs(node._gridX - goal._gridX);
                        int dy = Math.Abs(node._gridY - goal._gridY);
                        float hCost = grid.nodeDiameter * (dx + dy) + ((float)Math.Sqrt(grid.nodeDiameter * 2) - 2 * grid.nodeDiameter) * Math.Min(dx,dy);
                        
                        // Manhattan distance
                        //node.hCost = Math.Abs(node._gridX - goal._gridX) + Math.Abs(node._gridY - goal._gridY);

                        //Check if this path is better than previously documented
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

    public void BuildPath(Node current, Node start)
    {
        while (current != start)
        {
            finalPath.Add(current);
            current = current.Parent;
        }

        grid.finalPath = finalPath;
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

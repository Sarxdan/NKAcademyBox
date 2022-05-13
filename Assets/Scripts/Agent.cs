using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Queue<Node> targetPath;
    private Node currentTarget;
    private bool atTarget;

    private void Awake()
    {
        targetPath = new Queue<Node>();
    }

    private void Update()
    {
        if (!atTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.Position, speed);
            
            // Position check
            if (transform.position == currentTarget.Position)
            {
                if (targetPath.Count > 0)
                {
                    currentTarget = targetPath.Dequeue();
                }
                else
                {
                    atTarget = true;
                }
            }
        }
    }

    public void NewPath(List<Node> newPath)
    {
        atTarget = false;
        
        targetPath.Clear(); 

        for (int i = 0; i < newPath.Count; i++)
        {
            targetPath.Enqueue(newPath[i]);
        }

        currentTarget = targetPath.Dequeue();
        
    }
}

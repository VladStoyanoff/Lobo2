using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeNode : MonoBehaviour 
{
    [SerializeField] GameObject[] walls;

    void Start()
    {
        gameObject.tag = "Maze Node";
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).tag = "Wall";
        }
    }

    public void RemoveWall(int wallToRemove)
    {
        walls[wallToRemove].gameObject.SetActive(false);
    }

    public Vector2 GetMazeNodePosition() => transform.position;
}


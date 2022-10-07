using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeNode nodePrefab;
    [SerializeField] Vector2Int mazeSize;
    List<MazeNode> nodes = new List<MazeNode>();

    void Start()
    {
        GameManager.OnGameStarted += GameManager_OnGameStarted;
    }

    void GameManager_OnGameStarted(object sender, EventArgs e)
    {
        if (FindObjectOfType<GameManager>().GetIsGameActiveBool() == false) return;
        GenerateMaze();
    }

    void GenerateMaze()
    {
        nodes.Clear();

        // Create Nodes
        for (int x = 0; x < mazeSize.x; x++)
        {
            for (int y = 0; y < mazeSize.y; y++)
            {
                // centers the maze around (0,0) instead of the far left corner
                var nodePos = new Vector2(x - (mazeSize.x / 2f), y - (mazeSize.y / 2f));
                var newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
                nodes.Add(newNode);
            }
        }

        var currentPath = new List<MazeNode>();
        var completedNodes = new List<MazeNode>();

        // Choose Starting Node
        currentPath.Add(nodes[UnityEngine.Random.Range(0, nodes.Count)]);

        while (completedNodes.Count < nodes.Count)
        {
            var possibleNextNodes = new List<int>();
            var possibleDirections = new List<int>();

            var currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            var currentNodeX = currentNodeIndex / mazeSize.y;
            var currentNodeY = currentNodeIndex % mazeSize.y;

            void CheckNeighbourNode(int nodePosition,
                                    int outmostWallPosition,
                                    bool boolean,
                                    int changeDirectionsVariable,
                                    int indexesToNeighbouringNode,
                                    int directionToMoveTo)
            {
                if (nodePosition >= outmostWallPosition == boolean) return;

                var neighbouringNodeIndex = currentNodeIndex + changeDirectionsVariable * indexesToNeighbouringNode;

                if (completedNodes.Contains(nodes[neighbouringNodeIndex])) return;
                if (currentPath.Contains(nodes[neighbouringNodeIndex])) return;

                possibleDirections.Add(directionToMoveTo);
                possibleNextNodes.Add(neighbouringNodeIndex);
            }

            // Check node to the right of the current node
            CheckNeighbourNode(currentNodeX, mazeSize.x - 1, true, 1, mazeSize.y, 1);

            // Check node to the left of the current node
            CheckNeighbourNode(currentNodeX, 1, false, -1, mazeSize.y, 2);

            // Check node above the current node
            CheckNeighbourNode(currentNodeY, mazeSize.y - 1, true, 1, 1, 3);

            // Check node below the current node
            CheckNeighbourNode(currentNodeY, 1, false, -1, 1, 4);

            // Manage density settings
            var chanceToRemoveWall = 100 / FindObjectOfType<UIManager>().GetDensitySetting();
            if (UnityEngine.Random.Range(0, 100) <= chanceToRemoveWall)
            {
                nodes[currentNodeIndex].RemoveWall(UnityEngine.Random.Range(0, 4));
                nodes[currentNodeIndex].RemoveWall(UnityEngine.Random.Range(0, 4));
            }

            // Ensure the outmost walls stay active
            if (currentNodeX == mazeSize.x - 1)
                nodes[currentNodeIndex].transform.GetChild(1).gameObject.SetActive(true);
            if (currentNodeX == 0)
                nodes[currentNodeIndex].transform.GetChild(2).gameObject.SetActive(true);
            if (currentNodeY == mazeSize.y - 1)
                nodes[currentNodeIndex].transform.GetChild(3).gameObject.SetActive(true);
            if (currentNodeY == 0)
                nodes[currentNodeIndex].transform.GetChild(4).gameObject.SetActive(true);

            // Choose next node
            if (possibleDirections.Count > 0)
            {
                var chosenDirection = UnityEngine.Random.Range(0, possibleDirections.Count);
                var chosenNode = nodes[possibleNextNodes[chosenDirection]];

                // Remove walls between nodes that are connected in a path
                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenNode.RemoveWall(1);
                        currentPath[currentPath.Count - 1].RemoveWall(0);
                        break;
                    case 2:
                        chosenNode.RemoveWall(0);
                        currentPath[currentPath.Count - 1].RemoveWall(1);
                        break;
                    case 3:
                        chosenNode.RemoveWall(3);
                        currentPath[currentPath.Count - 1].RemoveWall(2);
                        break;
                    case 4:
                        chosenNode.RemoveWall(2);
                        currentPath[currentPath.Count - 1].RemoveWall(3);
                        break;
                }

                currentPath.Add(chosenNode);
            }

            // Backtrack
            else
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);
                currentPath.RemoveAt(currentPath.Count - 1);
            }
        }
    }

    public List<MazeNode> GetMazeNodesList() => nodes;
}

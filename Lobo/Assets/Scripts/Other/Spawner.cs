using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    MazeGenerator mazeGenerator;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject enemyBasePrefab;

    GameObject player;

    List<GameObject> enemyBases = new List<GameObject>();

    void Awake()
    {
        mazeGenerator = FindObjectOfType<MazeGenerator>();
    }

    void Start()
    {
        GameManager.OnGameStarted += GameManager_OnGameStarted;
    }

    void GameManager_OnGameStarted(object sender, EventArgs e)
    {
        if (FindObjectOfType<GameManager>().GetIsGameActiveBool() == false) return;
        SpawnPlayer();
        SpawnEnemyBases();
    }

    public void SpawnPlayer()
    {
        var allNodes = mazeGenerator.GetMazeNodesList();
        var spawnPlayerHere = allNodes[UnityEngine.Random.Range(0, allNodes.Count)];
        allNodes.Remove(spawnPlayerHere);
        if (FindObjectOfType<PlayerController>() == null)
        {
            player = Instantiate(playerPrefab, spawnPlayerHere.transform.position, Quaternion.identity);
        }
        else
        {
            player.GetComponent<PlayerController>().enabled = true;
            player.transform.position = spawnPlayerHere.transform.position;
            player.GetComponent<FuelTank>().RefillTank();
        }
    }

    void SpawnEnemyBases()
    {
        var basesToSpawn = 6;
        enemyBases.Clear();
        for (int i = 0; i < basesToSpawn; i++)
        {
            var allNodes = mazeGenerator.GetMazeNodesList();
            var randomNode = allNodes[UnityEngine.Random.Range(0, allNodes.Count)];
            allNodes.Remove(randomNode);
            var enemyBase = Instantiate(enemyBasePrefab, randomNode.GetMazeNodePosition(), Quaternion.identity, transform);
            enemyBases.Add(enemyBase);
        }
    }

    public List<GameObject> GetEnemyBases() => enemyBases;
}

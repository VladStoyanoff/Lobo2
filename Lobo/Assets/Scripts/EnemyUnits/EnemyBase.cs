using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    GameManager gameManager;
    UIManager uiManager;

    [SerializeField] float spawnRate;
    [SerializeField] GameObject[] enemyUnitPrefabs;
    int index;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player Bullet"))
        {
            FindObjectOfType<Spawner>().GetEnemyBases().Remove(gameObject);
            FindObjectOfType<FuelTank>().RefillTank();
            FindObjectOfType<ScoreManager>().ModifyScore(100);
            Destroy(gameObject);
        }
    }

    // The switch statement manages the chance of spawning a specific enemy from the bases. From case 3 to case 8, sometimes it assigns index twice.
    // This design was chosen in order to avoid "staircase complexity" where there's lots of indententions of code consecutively.
    IEnumerator SpawnEnemies()
    {
        int randomModifier;
        var basicUnitIndex = 0;
        var chasePlayerUnitIndex = 1;
        var ramPlayerUnitIndex = 2;
        while (gameManager.GetIsGameActiveBool())
        {
            randomModifier = Random.Range(0, 100);
            switch (uiManager.GetLevelSetting())
            {
                case 1:
                    index = basicUnitIndex;
                    break;
                case 2:
                    index = randomModifier < 80 ? index = basicUnitIndex : index = chasePlayerUnitIndex;
                    break;
                case 3:
                    index = randomModifier > 70 && randomModifier < 90 ? index = chasePlayerUnitIndex : index = ramPlayerUnitIndex;
                    if (randomModifier < 70) index = basicUnitIndex;
                    break;
                case 4:
                    index = randomModifier > 50 && randomModifier < 75 ? index = chasePlayerUnitIndex : index = ramPlayerUnitIndex;
                    if (randomModifier < 50) index = basicUnitIndex;
                    break;
                case 5:
                    index = randomModifier > 40 && randomModifier < 70 ? index = chasePlayerUnitIndex : index = ramPlayerUnitIndex;
                    if (randomModifier < 40) index = basicUnitIndex;
                    break;
                case 6:
                    index = randomModifier > 33 && randomModifier < 66 ? index = chasePlayerUnitIndex : index = ramPlayerUnitIndex;
                    if (randomModifier < 33) index = basicUnitIndex;
                    break;
                case 7:
                    index = randomModifier > 25 && randomModifier < 40 ? index = chasePlayerUnitIndex : index = ramPlayerUnitIndex;
                    if (randomModifier < 25) index = basicUnitIndex;
                    break;
                case 8:
                    index = randomModifier > 10 && randomModifier < 45 ? index = chasePlayerUnitIndex : index = ramPlayerUnitIndex;
                    if (randomModifier < 10) index = basicUnitIndex;
                    break;
                case 9:
                    index = randomModifier < 50 ? index = chasePlayerUnitIndex : index = ramPlayerUnitIndex;
                    break;
            }

            Instantiate(enemyUnitPrefabs[index], transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnRate);
        }
    }
}

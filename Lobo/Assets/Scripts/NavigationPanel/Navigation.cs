using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Navigation : MonoBehaviour
{

    List<GameObject> enemyBases = new List<GameObject>();

    [SerializeField] RawImage[] radarRawImageArray;
    [SerializeField] RawImage[] cannonRotationRawImageArray;
    bool[] worldCompassDirections = new bool[8];

    Spawner spawner;

    const int PI = 180;

    [SerializeField] GameObject cannon;
    [SerializeField] int radarRadius = 3;

    void Awake()
    {
        spawner = FindObjectOfType<Spawner>();
    }

    void Start()
    {
        var navigationpanel = GameObject.FindGameObjectWithTag("UI").transform.GetChild(1);

        for (int i = 0; i < 4; i++)
        {
            radarRawImageArray[i] = navigationpanel.GetChild(5).GetChild(i).GetComponent<RawImage>();
            radarRawImageArray[i].color = Color.black;
        }

        for (int i = 0; i < 8; i++)
        {
            cannonRotationRawImageArray[i] = navigationpanel.GetChild(6).GetChild(0).GetChild(0).GetChild(i).GetComponent<RawImage>();
        }
    }

    void Update()
    {
        if (spawner.GetEnemyBases().Count == 0) return;
        UpdateRadarForAllBases();
        UpdateCannonDirectionRadar();
    }

    void UpdateRadarForAllBases()
    {
        enemyBases = spawner.GetEnemyBases();
        var vectorLengthList = new List<float>();

        // Reset boolean compass indicators
        for (int i = 0; i < worldCompassDirections.Length; i++)
        {
            if (worldCompassDirections[i] == true) worldCompassDirections[i] = false;
        }

        // Find distances to all enemy bases and add them to a list. If the distance is beyond the radar's range dont try to locate the base
        for (int i = 0; i < enemyBases.Count; i++)
        {
            if (enemyBases[i] == null) continue;
            var distanceBetweenPlayerBase = Vector2.Distance(gameObject.transform.position, enemyBases[i].transform.position);
            vectorLengthList.Add(distanceBetweenPlayerBase);
            if (distanceBetweenPlayerBase > radarRadius) continue;
            UpdateRadarForSingleBase(i, radarRadius);
        }
        ManageColorsForRadar();

        // If the radar hasnt located a single base, locate the closest one to the player.
        if (vectorLengthList.Min() <= radarRadius) return;
        var closestenemybase = vectorLengthList.Min();
        var index = vectorLengthList.IndexOf(closestenemybase);
        UpdateRadarForSingleBase(index, Mathf.Infinity);
        ManageColorsForRadar();
    }

    void ManageColorsForRadar()
    {
        foreach (var rawImage in radarRawImageArray)
        {
            rawImage.color = Color.black;
        }

        TryActivateSprite(worldCompassDirections[0], radarRawImageArray[0], radarRawImageArray[1]);
        TryActivateSprite(worldCompassDirections[1], radarRawImageArray[1], radarRawImageArray[3]);
        TryActivateSprite(worldCompassDirections[2], radarRawImageArray[0], radarRawImageArray[2]);
        TryActivateSprite(worldCompassDirections[3], radarRawImageArray[2], radarRawImageArray[3]);
        TryActivateSprite(worldCompassDirections[4], radarRawImageArray[1], null);
        TryActivateSprite(worldCompassDirections[5], radarRawImageArray[0], null);
        TryActivateSprite(worldCompassDirections[6], radarRawImageArray[3], null);
        TryActivateSprite(worldCompassDirections[7], radarRawImageArray[2], null);
    }

    void TryActivateSprite(bool boolean, RawImage imageOne, RawImage imageTwo)
    {
        if (boolean == false) return;
        imageOne.color = Color.green;
        if (imageTwo == null) return;
        imageTwo.color = Color.green;
    }

    void UpdateRadarForSingleBase(int index, float radarRadius)
    {
        var vectorToEnemyBase = enemyBases[index].transform.position - gameObject.transform.position;
        var angle = Vector2.Angle(vectorToEnemyBase.normalized, Vector2.right);

        if (vectorToEnemyBase.x < 1f / 2f * radarRadius &&
            vectorToEnemyBase.x > -1f / 2f * radarRadius &&
            vectorToEnemyBase.y > 0 &&
            vectorToEnemyBase.y < radarRadius &&
            angle > PI / 3f &&
            angle < 2 * PI / 3f)

        {
            worldCompassDirections[0] = true;
        }

        if (vectorToEnemyBase.x > 0 &&
            vectorToEnemyBase.x < radarRadius &&
            vectorToEnemyBase.y < 1f / 2f * radarRadius &&
            vectorToEnemyBase.y > -1f / 2f * radarRadius &&
            // the angle calculation ensures for both boundaries, because Vector3.Angle does not calculate angles between 180 and 360
            angle < PI / 6)

        {
            worldCompassDirections[1] = true;
        }

        if (vectorToEnemyBase.x < 0 &&
            vectorToEnemyBase.x > -radarRadius &&
            vectorToEnemyBase.y < 1f / 2f * radarRadius &&
            vectorToEnemyBase.y > -1f / 2f * radarRadius &&
            // the angle calculation ensures for both boundaries, because Vector3.Angle does not calculate angles between 180 and 360
            angle > 5 * PI / 6)

        {
            worldCompassDirections[2] = true;
        }

        if (vectorToEnemyBase.x < 1f / 2f * radarRadius &&
            vectorToEnemyBase.x > -1f / 2f * radarRadius &&
            vectorToEnemyBase.y < 0 &&
            vectorToEnemyBase.y > -radarRadius &&
            angle > PI / 3f &&
            angle < 2 * PI / 3f)

        {
            worldCompassDirections[3] = true;
        }

        if (vectorToEnemyBase.x > 0 &&
            vectorToEnemyBase.x < Mathf.Sqrt(3f) / 2f * radarRadius &&
            vectorToEnemyBase.y > 0 &&
            vectorToEnemyBase.y < Mathf.Sqrt(3f) / 2f * radarRadius &&
            angle > PI / 6 &&
            angle < PI / 3)

        {
            worldCompassDirections[4] = true;
        }

        if (vectorToEnemyBase.x < 0 &&
            vectorToEnemyBase.x > -Mathf.Sqrt(3f) / 2f * radarRadius &&
            vectorToEnemyBase.y > 0 &&
            vectorToEnemyBase.y < Mathf.Sqrt(3f) / 2f * radarRadius &&
            angle > 2 * PI / 3 &&
            angle < 5 * PI / 6)

        {
            worldCompassDirections[5] = true;
        }

        if (vectorToEnemyBase.x > 0 &&
            vectorToEnemyBase.x < Mathf.Sqrt(3f) / 2f * radarRadius &&
            vectorToEnemyBase.y < 0 &&
            vectorToEnemyBase.y > -Mathf.Sqrt(3f) / 2f * radarRadius &&
            angle > PI / 6 &&
            angle < PI / 3)

        {
            worldCompassDirections[6] = true;
        }

        if (vectorToEnemyBase.x < 0 &&
            vectorToEnemyBase.x > -Mathf.Sqrt(3f) / 2f * radarRadius &&
            vectorToEnemyBase.y < 0 &&
            vectorToEnemyBase.y > -Mathf.Sqrt(3f) / 2f * radarRadius &&
            angle > 2 * PI / 3 &&
            angle < 5 * PI / 6)

        {
            worldCompassDirections[7] = true;
        }
    }

    void UpdateCannonDirectionRadar()
    {
        UpdateCannonRotationRadar(0, -PI / 6, PI / 6);
        UpdateCannonRotationRadar(1, PI / 6, PI / 3);
        UpdateCannonRotationRadar(2, PI / 3, 2 * PI / 3);
        UpdateCannonRotationRadar(3, 2 * PI / 3, 5 * PI / 6);
        UpdateCannonRotationRadar(4, 5 * PI / 6, 7 * PI / 6);
        UpdateCannonRotationRadar(5, 7 * PI / 6, 4 * PI / 3);
        UpdateCannonRotationRadar(6, 4 * PI / 3, 5 * PI / 3);
        UpdateCannonRotationRadar(7, 5 * PI / 3, 11 * PI / 6);
    }

    void UpdateCannonRotationRadar(int index, int leftBoundary, int rightBoundary)
    {
        if (cannon.transform.localEulerAngles.z > leftBoundary && cannon.transform.localEulerAngles.z < rightBoundary)
        {
            foreach (var rawImage in cannonRotationRawImageArray)
            {
                rawImage.gameObject.SetActive(false);
            }
            if (index == 0) return;
            cannonRotationRawImageArray[index].gameObject.SetActive(true);
        }
    }
}

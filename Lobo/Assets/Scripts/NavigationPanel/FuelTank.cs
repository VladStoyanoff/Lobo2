using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FuelTank : MonoBehaviour
{
    Image fuelTank;
    PlayerController playerController;
    float fuelLostAmount = .0001f;
    float fuelFillAmount = .01f;
    float secondsBetweenFill = .01f;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        fuelTank = GameObject.FindGameObjectWithTag("UI").transform.GetChild(1).GetChild(4).GetChild(0).GetChild(0).GetComponent<Image>();
        StartCoroutine(FillTank());
    }

    void Update()
    {
        if (playerController.GetMovementInput() == new Vector2(0, 0)) return;
        fuelTank.fillAmount -= fuelLostAmount;
        if (fuelTank.fillAmount > 0) return;
        StartCoroutine(playerController.RestartPlayerPosition());
    }

    public void RefillTank()
    {
        fuelTank.fillAmount = 1;
    }

    public void EmptyTank()
    {
        fuelTank.fillAmount = 0;
    }

    IEnumerator FillTank()
    {
        fuelTank.enabled = true;
        fuelTank.fillAmount = 0;
        while (fuelTank.fillAmount < 1)
        {
            fuelTank.fillAmount += fuelFillAmount;
            yield return new WaitForSeconds(secondsBetweenFill);
        }
    }
}

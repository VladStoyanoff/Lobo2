using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cinemachineCamera;
    PlayerController playerController;

    void Start()
    {
        GameManager.OnGameStarted += GameManager_OnGameStarted;
    }

    void GameManager_OnGameStarted(object sender, EventArgs e)
    {
        if (FindObjectOfType<GameManager>().GetIsGameActiveBool() == false) return;
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null) return;
        cinemachineCamera.LookAt = cinemachineCamera.Follow = playerController.transform;
    }
}

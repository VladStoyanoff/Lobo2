using System.Collections;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float timeSinceLastShot = Mathf.Infinity;
    InputActions inputActionsScript;
    GameManager gameManager;
    Spawner spawner;
    AudioManager audioManager;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject cannon;
    [SerializeField] GameObject tankSprite;
    [SerializeField] Transform bulletSpawnPoint;

    [SerializeField] float bulletSpeed = 2f;
    [SerializeField] float cannonRotationSpeed = 100;

    const float FIRE_RATE = .5f;
    const float SLOW_DOWN_AMOUNT = .01f;
    const int ROTATE_AMOUNT = 45;

    [SerializeField] float moveSpeed;
    Vector2 movementInput;

    [SerializeField] ParticleSystem explosion;

    void Awake()
    {
        inputActionsScript = new InputActions();
        inputActionsScript.Player.Enable();

        gameManager = FindObjectOfType<GameManager>();
        spawner = FindObjectOfType<Spawner>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        UpdateMovement();
        AlwaysMoveForward();
        TryRotateCannon();
        TryShootProjectile();

        if (gameManager.GetCollidedBool() == false) return;
        gameManager.SetCollidedBool(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.GetCollidedBool()) return;
        StartCoroutine(RestartPlayerPosition());
    }

    public IEnumerator RestartPlayerPosition()
    {
        var explosionDuration = 1.5f;
        moveSpeed = 0;
        explosion.Play();
        audioManager.PlayDestroyedEnemyClip();
        gameManager.ReduceLives();
        GetComponent<PlayerController>().enabled = false;
        yield return new WaitForSeconds(explosionDuration);
        spawner.SpawnPlayer();
    }

    void UpdateMovement()
    {
        UpdateRotation(Input.GetKeyDown(KeyCode.D), -1);
        UpdateRotation(Input.GetKeyDown(KeyCode.A), 1);
        UpdateSpeed(Input.GetKey(KeyCode.S), -1);
        UpdateSpeed(Input.GetKey(KeyCode.W), 1);
    }

    void UpdateRotation(bool key, float changeSigns)
    {
        if (!key) return;
        var rotationVector = Vector3.zero;
        rotationVector.z = ROTATE_AMOUNT * changeSigns;
        tankSprite.transform.localEulerAngles += rotationVector;
        cannon.transform.localEulerAngles += rotationVector;
    }

    void UpdateSpeed(bool key, float changeSigns)
    {
        if (!key) return;
        moveSpeed += SLOW_DOWN_AMOUNT * changeSigns;
        moveSpeed = Mathf.Clamp(moveSpeed, 0, int.MaxValue);
    }

    void AlwaysMoveForward()
    {
        transform.position += moveSpeed * Time.deltaTime * tankSprite.transform.up;
    }

    void TryRotateCannon()
    {
        var rotationVector = Vector3.zero;
        rotationVector.z = inputActionsScript.Player.CannonRotation.ReadValue<float>();
        cannon.transform.localEulerAngles += rotationVector * cannonRotationSpeed * Time.deltaTime;
    }

    void TryShootProjectile()
    {
        if (timeSinceLastShot < FIRE_RATE) return;
        if (inputActionsScript.Player.Shoot.IsPressed() == false) return;
        audioManager.PlayShootingClip();
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.Euler(cannon.transform.localEulerAngles));
        bullet.tag = "Player Bullet";
        bullet.GetComponent<Rigidbody2D>().velocity = bulletSpawnPoint.up * bulletSpeed;
        timeSinceLastShot = 0;
    }

    public Vector2 GetMovementInput() => movementInput;
}

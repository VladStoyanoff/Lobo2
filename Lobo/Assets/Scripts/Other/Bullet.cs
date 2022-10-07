using System.Collections;
using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float explosionRadius;
    [SerializeField] ParticleSystem explosion;

    ScoreManager scoreManager;
    AudioManager audioManager;

    int scoreForBasicUnit = 2;
    int scoreForChasePlayerUnit = 4;
    int scoreForRamPlayerUnit = 6;

    void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        explosion.Play();
        foreach (var col in colliders)
        {
            if (gameObject.CompareTag("Enemy Bullet") && col.CompareTag("Building Block")) continue;
            if (col.CompareTag("Player")) continue;
            if (col.gameObject.CompareTag("Player Bullet") || col.gameObject.CompareTag("Enemy Bullet")) continue;
            audioManager.PlayHitBlockClip();
            if (col.CompareTag("Wall")) continue;
            Destroy(col.gameObject);
        }

        if (gameObject.CompareTag("Player Bullet") && collision.gameObject.layer == 11)
        {
            audioManager.PlayDestroyedEnemyClip();

            switch (collision.tag)
            {
                case "BasicUnit":
                    {
                        scoreManager.ModifyScore(scoreForBasicUnit);
                        break;
                    }
                case "ChasePlayerUnit":
                    {
                        scoreManager.ModifyScore(scoreForChasePlayerUnit);
                        break;
                    }
                case "RamPlayerUnit":
                    {
                        scoreManager.ModifyScore(scoreForRamPlayerUnit);
                        break;
                    }
            }
        }
        StartCoroutine(DestroyBullet());
       
    }

    IEnumerator DestroyBullet()
    {
        var durationOfTheExplosion = 1.5f;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(durationOfTheExplosion);
        Destroy(gameObject);
    }
}

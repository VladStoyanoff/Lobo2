using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] AudioClip shootingSFX;
    [SerializeField][Range(0f, 1f)] float shootingSFXVolume;

    [Header("Hit Block")]
    [SerializeField] AudioClip hitBlockSFX;
    [SerializeField][Range(0f, 1f)] float hitblockSFXVolume;

    [Header("Destroyed Enemy")]
    [SerializeField] AudioClip destroyedEnemySFX;
    [SerializeField][Range(0f, 1f)] float destroyedEnemySFXVolume;

    public void PlayShootingClip()
    {
        PlayClip(shootingSFX, shootingSFXVolume);
    }

    public void PlayHitBlockClip()
    {
        PlayClip(hitBlockSFX, hitblockSFXVolume);
    }

    public void PlayDestroyedEnemyClip()
    {
        PlayClip(destroyedEnemySFX, destroyedEnemySFXVolume);
    }

    void PlayClip(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            var cameraPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(clip, cameraPos, volume);
        }
    }
}

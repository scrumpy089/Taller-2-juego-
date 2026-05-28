using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundEffectPlayer : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Player Action Sounds")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip damageTakenSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip healSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip walkSound;

    [Header("UI Sounds")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip sceneChangeSound;

    public void PlayButtonSound()
    {
        sfxSource.PlayOneShot(buttonClickSound, 1f);
    }

    public void ChangeSceneWithSound(int sceneIndex)
    {
        StartCoroutine(ChangeSceneRoutine(sceneIndex));
    }

    private IEnumerator ChangeSceneRoutine(int sceneIndex)
    {
        sfxSource.PlayOneShot(sceneChangeSound, 1f);

        yield return new WaitForSecondsRealtime(sceneChangeSound.length);

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneIndex);
    }

    public void ChangeSceneWithSound2(int sceneIndex)
    {
        StartCoroutine(ChangeSceneRoutine2(sceneIndex));
    }

    private IEnumerator ChangeSceneRoutine2(int sceneIndex)
    {
        sfxSource.PlayOneShot(buttonClickSound, 1f);

        yield return new WaitForSecondsRealtime(sceneChangeSound.length);

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneIndex);
    }

    public void PlayHitSound()
    {
        sfxSource.PlayOneShot(hitSound);
    }

    public void PlayDamageSound()
    {
        sfxSource.PlayOneShot(damageTakenSound);
    }

    public void PlayDeathSound()
    {
        sfxSource.PlayOneShot(deathSound);
    }

    public void PlayHealSound()
    {
        sfxSource.PlayOneShot(healSound);
    }

    public void PlayJumpSound()
    {
        sfxSource.PlayOneShot(jumpSound, 0.5f);
    }

    public void PlayWalkSound()
    {
        sfxSource.PlayOneShot(walkSound, 1.5f);
    }
}
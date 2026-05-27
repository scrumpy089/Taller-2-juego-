using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private AudioClip JumpSound;
    [SerializeField] private AudioClip WalkSound;

    [Header("Enemy Attack Sounds")]
    [SerializeField] private AudioClip[] humanAttackSound;
    //[SerializeField] private AudioClip[] trashAttackSound;
    //[SerializeField] private AudioClip cucarachoAttackSound;
    [SerializeField] private AudioClip[] cocoHitSound;

    [Header("Scene Transition Sounds")]
    [SerializeField] private AudioClip sceneChange;

    // Start is called before the first frame update
    void Start()
    {

        sfxSource.PlayOneShot(sceneChange);

    }

    // Update is called once per frame
    void Update()
    {

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
        sfxSource.PlayOneShot(JumpSound);
    }

    public void PlayWalkSound()
    {
        sfxSource.PlayOneShot(WalkSound);
    }
    public void PlayCocoHitSound()
    {
        if (cocoHitSound.Length == 0)
            return;

        int randomIndex = Random.Range(0, cocoHitSound.Length);

        sfxSource.PlayOneShot(cocoHitSound[randomIndex]);
    }

    public void humanAttack()
    {
        if (humanAttackSound.Length == 0)
            return;

        int randomIndex = Random.Range(0, humanAttackSound.Length);

        sfxSource.PlayOneShot(humanAttackSound[randomIndex]);
    }

    /*
    public void trashAttack()
    {
        if (trashAttackSound.Length == 0)
            return;
        int randomIndex = Random.Range(0, trashAttackSound.Length);
        sfxSource.PlayOneShot(trashAttackSound[randomIndex]);
    }
    */

    /*
    public void cucarachoAttack()
    {
        sfxSource.PlayOneShot(cucarachoAttackSound);
    }
    */
}

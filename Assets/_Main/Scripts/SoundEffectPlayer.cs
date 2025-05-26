using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip damageTakenSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip healSound;
    [SerializeField] private AudioClip cocoHitSound;

    // Start is called before the first frame update
    void Start()
    {
        
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
    public void PlayCocoHitSound()
    {
        sfxSource.PlayOneShot(cocoHitSound);
    }

}

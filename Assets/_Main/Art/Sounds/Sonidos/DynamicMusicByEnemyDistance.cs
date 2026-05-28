using UnityEngine;

public class DynamicMusicByEnemyDistance : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemy;

    [Header("Música")]
    [SerializeField] private AudioSource ambientMusic;
    [SerializeField] private AudioSource dangerMusic;

    [Header("Distancias")]
    [SerializeField] private float maxDistance = 12f;
    [SerializeField] private float minDistance = 3f;

    [Header("Volumen")]
    [SerializeField] private float ambientMaxVolume = 1f;
    [SerializeField] private float ambientMinVolume = 0.25f;
    [SerializeField] private float dangerMaxVolume = 1f;

    [Header("Suavizado")]
    [SerializeField] private float fadeSpeed = 2f;

    private void Start()
    {
        ambientMusic.loop = true;
        dangerMusic.loop = true;

        ambientMusic.Play();
        dangerMusic.Play();

        dangerMusic.volume = 0f;
    }

    private void Update()
    {
        // Si el enemigo fue destruido o desactivado
        if (enemy == null || !enemy.gameObject.activeInHierarchy)
        {
            ambientMusic.volume = Mathf.Lerp(
                ambientMusic.volume,
                ambientMaxVolume,
                Time.deltaTime * fadeSpeed
            );

            dangerMusic.volume = Mathf.Lerp(
                dangerMusic.volume,
                0f,
                Time.deltaTime * fadeSpeed
            );

            // Detener completamente la música de peligro
            if (dangerMusic.volume < 0.01f && dangerMusic.isPlaying)
            {
                dangerMusic.Stop();
            }

            return;
        }

        // Volver a reproducir si el enemigo reaparece
        if (!dangerMusic.isPlaying)
        {
            dangerMusic.Play();
        }

        float distance = Vector2.Distance(
            player.position,
            enemy.position
        );

        float proximity = Mathf.InverseLerp(
            maxDistance,
            minDistance,
            distance
        );

        proximity = Mathf.Clamp01(proximity);

        float targetAmbient = Mathf.Lerp(
            ambientMaxVolume,
            ambientMinVolume,
            proximity
        );

        float targetDanger = Mathf.Lerp(
            0f,
            dangerMaxVolume,
            proximity
        );

        ambientMusic.volume = Mathf.Lerp(
            ambientMusic.volume,
            targetAmbient,
            Time.deltaTime * fadeSpeed
        );

        dangerMusic.volume = Mathf.Lerp(
            dangerMusic.volume,
            targetDanger,
            Time.deltaTime * fadeSpeed
        );
    }
}
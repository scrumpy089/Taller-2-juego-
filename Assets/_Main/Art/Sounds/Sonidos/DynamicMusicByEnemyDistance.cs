using UnityEngine;

public class DynamicMusicByEnemyDistance : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;

    [Header("Tags")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string bossTag = "Boss";

    [Header("Música base")]
    [SerializeField] private AudioSource ambientMusic;

    [Header("Leitmotiv enemigos normales")]
    [SerializeField] private AudioSource enemyLeitmotiv;

    [Header("Música de peligro / Boss")]
    [SerializeField] private AudioSource dangerMusic;

    [Header("Distancias Enemy")]
    [SerializeField] private float enemyMaxDistance = 12f;
    [SerializeField] private float enemyMinDistance = 3f;

    [Header("Distancias Boss")]
    [SerializeField] private float bossMaxDistance = 15f;
    [SerializeField] private float bossMinDistance = 4f;

    [Header("Volumen ambiente")]
    [SerializeField] private float ambientMaxVolume = 1f;
    [SerializeField] private float ambientMinVolumeEnemy = 0.65f;
    [SerializeField] private float ambientMinVolumeBoss = 0.25f;

    [Header("Volumen capas")]
    [SerializeField] private float enemyLeitmotivMaxVolume = 1f;
    [SerializeField] private float dangerMaxVolume = 1f;

    [Header("Suavizado")]
    [SerializeField] private float fadeSpeed = 2f;

    private void Start()
    {
        ambientMusic.loop = true;
        enemyLeitmotiv.loop = true;
        dangerMusic.loop = true;

        ambientMusic.Play();
        enemyLeitmotiv.Play();
        dangerMusic.Play();

        enemyLeitmotiv.volume = 0f;
        dangerMusic.volume = 0f;
    }

    private void Update()
    {
        float enemyDistance = GetClosestDistance(enemyTag);
        float bossDistance = GetClosestDistance(bossTag);

        bool enemyNear = enemyDistance != Mathf.Infinity;
        bool bossNear = bossDistance != Mathf.Infinity;

        float enemyProximity = 0f;
        float bossProximity = 0f;

        if (enemyNear)
        {
            enemyProximity = Mathf.InverseLerp(
                enemyMaxDistance,
                enemyMinDistance,
                enemyDistance
            );

            enemyProximity = Mathf.Clamp01(enemyProximity);
        }

        if (bossNear)
        {
            bossProximity = Mathf.InverseLerp(
                bossMaxDistance,
                bossMinDistance,
                bossDistance
            );

            bossProximity = Mathf.Clamp01(bossProximity);
        }

        float targetAmbientVolume = ambientMaxVolume;
        float targetEnemyLeitmotivVolume = 0f;
        float targetDangerVolume = 0f;

        // Enemy normal: baja un poco ambiente y sube leitmotiv
        if (enemyProximity > 0f)
        {
            targetAmbientVolume = Mathf.Lerp(
                ambientMaxVolume,
                ambientMinVolumeEnemy,
                enemyProximity
            );

            targetEnemyLeitmotivVolume = Mathf.Lerp(
                0f,
                enemyLeitmotivMaxVolume,
                enemyProximity
            );
        }

        // Boss: tiene prioridad sobre enemy normal
        // Baja más el ambiente y sube dangerMusic
        if (bossProximity > 0f)
        {
            targetAmbientVolume = Mathf.Lerp(
                ambientMaxVolume,
                ambientMinVolumeBoss,
                bossProximity
            );

            targetDangerVolume = Mathf.Lerp(
                0f,
                dangerMaxVolume,
                bossProximity
            );

            // Opcional: apagar leitmotiv normal cuando hay boss cerca
            targetEnemyLeitmotivVolume = 0f;
        }

        ambientMusic.volume = Mathf.Lerp(
            ambientMusic.volume,
            targetAmbientVolume,
            Time.deltaTime * fadeSpeed
        );

        enemyLeitmotiv.volume = Mathf.Lerp(
            enemyLeitmotiv.volume,
            targetEnemyLeitmotivVolume,
            Time.deltaTime * fadeSpeed
        );

        dangerMusic.volume = Mathf.Lerp(
            dangerMusic.volume,
            targetDangerVolume,
            Time.deltaTime * fadeSpeed
        );
    }

    private float GetClosestDistance(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in objects)
        {
            if (!obj.activeInHierarchy)
                continue;

            float distance = Vector2.Distance(
                player.position,
                obj.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
            }
        }

        return closestDistance;
    }
}
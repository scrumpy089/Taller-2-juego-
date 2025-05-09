using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{

    public float delayBeforeBreak = 0.5f;   // Tiempo antes de romperse
    public float respawnDelay = 3f;          // Tiempo para respawnear
    public float shakeAmount = 0.1f;         // Intensidad del temblor
    public float shakeDuration = 0.5f;       // Duración del temblor

    private Vector3 originalPosition;
    private bool isBreaking = false;

    private Collider2D col;
    private SpriteRenderer spriteRenderer; // Opcional si quieres desaparecer visualmente


    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Si es un Sprite
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !isBreaking)
        {
            isBreaking = true;
            StartCoroutine(BreakRoutine());
        }
    }


    private IEnumerator BreakRoutine()
    {
        // 1. TEMBLAR
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            transform.position = originalPosition + (Vector3)Random.insideUnitCircle * shakeAmount;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 2. DESACTIVAR
        transform.position = originalPosition;
        if (col != null) col.enabled = false;
        if (spriteRenderer != null) spriteRenderer.enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

        // 3. ESPERAR PARA RESPAWN
        yield return new WaitForSeconds(respawnDelay);

        // 4. REACTIVAR
        transform.position = originalPosition;
        if (col != null) col.enabled = true;
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;

        isBreaking = false; // Permite volver a romperse
    }
}

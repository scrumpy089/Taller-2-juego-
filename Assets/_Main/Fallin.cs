using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fallin : MonoBehaviour
{
    public float warningTime = 2f;          // Tiempo antes de que caiga
    public float respawnDelay = 3f;         // Tiempo antes de que vuelva a su posición inicial
    private Vector3 initialPosition;        // Donde debe reaparecer
    private Rigidbody2D rb;
    private Collider2D col;
    public float damage;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip fallSound;
    [SerializeField] private AudioClip impactSound;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        StartCoroutine(StartCycle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StartCycle()
    {
        // Esperar antes de caer
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;
        transform.position = initialPosition;
        col.enabled = true;

        yield return new WaitForSeconds(warningTime);

        // Caída
        rb.bodyType = RigidbodyType2D.Dynamic;

        if (fallSound != null)
        {
            sfxSource.PlayOneShot(fallSound);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si colisiona con el jugador
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            StartCoroutine(ResetTrap());
        }

        // Si colisiona con el suelo
        else if (collision.CompareTag("Ground"))
        {
            if (impactSound != null)
            {
                sfxSource.PlayOneShot(impactSound);
            }

            StartCoroutine(ResetTrap());
        }
    }

    private IEnumerator ResetTrap()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false; // Evita más colisiones
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(respawnDelay);
        StartCoroutine(StartCycle());
    }

}

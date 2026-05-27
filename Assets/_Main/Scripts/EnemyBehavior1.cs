using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior1 : MonoBehaviour
{
    [Header("Private Variables")]  // Título para agrupar las variables privadas que solo se usan dentro del script.
    [Space]

    [Tooltip("Referencia al objetivo de movimiento actual del enemigo.")]
    [SerializeField] private Transform actualObjective;
    [Tooltip("El Rigidbody2D del enemigo para aplicar movimiento físico.")]
    [SerializeField] private Rigidbody2D rb;
    [Tooltip("El Animator del enemigo para controlar las animaciones.")]
    [SerializeField] private Animator enemyAnimator;

    Vector2 movement;  // Variable que contiene la dirección de movimiento del enemigo (en el plano 2D).

    [Header("Public Variables")]  // Título para agrupar las variables públicas que pueden ser modificadas desde otros scripts o el Inspector.
    [Space]

    [Tooltip("Un arreglo de puntos de movimiento por los que el enemigo se desplazará.")]
    public Transform[] enemyMovementPoints;
    [Tooltip("La velocidad a la que se moverá el enemigo.")]
    public float enemySpeed;
    [Tooltip("La distancia mínima a la que el enemigo debe llegar a un objetivo para considerarlo \"alcanzado\".")]
    public float detectionRadius = 0.5f;
    //[Tooltip("La cantidad de fuerza con la que el enemigo empuja al jugador cuando lo golpea.")]
    //public float enemyHitStrength;
    [Tooltip("El daño que el enemigo le causa al jugador al golpearlo.")]
    public float enemyDamage;

    [SerializeField] public float enemyHitStrengthX;
    [SerializeField] public float enemyHitStrengthY;
    [SerializeField] public int hitTime;
    [SerializeField] public int particles;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip[] EnemyAttackSound;

    [Header("")]
    public int enemyIndex;

    // Start is called before the first frame update
    void Start()
    {
        // Se obtienen las referencias de los componentes Rigidbody2D y Animator del enemigo.
        rb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        // Se establece el primer objetivo de movimiento del enemigo como el primer punto en el arreglo de objetivos.
        actualObjective = enemyMovementPoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        // Calcula la distancia entre el enemigo y su objetivo actual.
        float distanceToObjective = Vector2.Distance(transform.position, actualObjective.position);

        // Compara la distancia entre el enemigo y su objetivo con el detectionRadius para ver si ha llegado al objetivo.
        if (distanceToObjective < detectionRadius)
        {
            // Si el enemigo llegó a su objetivo, se cambia al siguiente objetivo.
            if (actualObjective == enemyMovementPoints[0])
            {
                actualObjective = enemyMovementPoints[1];  // Cambia al siguiente punto en el arreglo.
            }
            else if (actualObjective == enemyMovementPoints[1])
            {
                actualObjective = enemyMovementPoints[0];  // Vuelve al primer punto.
            }
        }

        // Calcula la dirección en la que el enemigo debe moverse para alcanzar su objetivo.
        Vector2 direction = (actualObjective.position - transform.position).normalized;

        // Redondea la dirección de movimiento a un valor más limpio (evita movimientos diagonales).
        int roundedDirection = Mathf.RoundToInt(direction.x);

        // Establece el vector de movimiento en función de la dirección (solo horizontal).
        movement = new Vector2(roundedDirection, 0);

        // Rotación del enemigo según la dirección en la que se mueve.
        if (roundedDirection < 0)  // Si se mueve a la izquierda.
        {
            transform.localScale = new Vector3(1, 1, 1);  // Invierte la escala en el eje X para que mire hacia la izquierda.
        }
        else if (roundedDirection > 0)  // Si se mueve a la derecha.
        {
            transform.localScale = new Vector3(-1, 1, 1);  // Restaura la escala en el eje X para que mire hacia la derecha.
        }

        // Actualiza el parámetro de movimiento del Animator para que el enemigo tenga la animación de caminar.
        enemyAnimator.SetFloat("Movement", roundedDirection);

        // Mueve al enemigo en la dirección calculada con la velocidad establecida y Time.deltaTime para un movimiento suave.
        rb.MovePosition(rb.position + movement * enemySpeed * Time.deltaTime);
    }

    // Este método se llama cuando el enemigo colisiona con otro objeto.
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el enemigo ha colisionado con un objeto que tenga el tag "Player".
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obtiene la referencia al script de comportamiento del jugador para aplicar el daño y empuje.
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            // Llama al método del jugador para que reciba daño.
            player.particleCount = particles;
            player.spriteIndex = enemyIndex;

            player.TakeDamage(enemyDamage);
            // Establece el tiempo que el jugador estará siendo empujado después de la colisión.
            player.hitTime = hitTime;
            // Establece la fuerza con la que el enemigo empuja al jugador tanto en el eje X como en el Y.
            player.hitForceX = enemyHitStrengthX;
            player.hitForceY = enemyHitStrengthY;

            // Determina desde qué dirección está siendo golpeado el jugador para aplicar el empuje en la dirección correcta.
            if (collision.transform.position.x <= transform.position.x)
            {
                player.hitFromRight = true;  // El jugador está siendo golpeado desde la derecha del enemigo.
            }
            else if (collision.transform.position.x > transform.position.x)
            {
                player.hitFromRight = false;  // El jugador está siendo golpeado desde la izquierda del enemigo.
            }

            soundAttack();  // Reproduce el sonido de ataque del enemigo.

            // Activa el trigger en el Animator para iniciar la animación de ataque del enemigo.
            enemyAnimator.SetTrigger("Attack");

        }

    }

    public void soundAttack()
    {
        if (EnemyAttackSound.Length == 0)
            return;

        int randomIndex = Random.Range(0, EnemyAttackSound.Length);

        sfxSource.PlayOneShot(EnemyAttackSound[randomIndex]);
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wrench"))
        {
            this.gameObject.SetActive(false);
        }

    }
}

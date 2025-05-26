using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior1 : MonoBehaviour
{
    [Header("Private Variables")]  // T�tulo para agrupar las variables privadas que solo se usan dentro del script.
    [Space]

    [Tooltip("Referencia al objetivo de movimiento actual del enemigo.")]
    [SerializeField] private Transform actualObjective;
    [Tooltip("El Rigidbody2D del enemigo para aplicar movimiento f�sico.")]
    [SerializeField] private Rigidbody2D rb;
    [Tooltip("El Animator del enemigo para controlar las animaciones.")]
    [SerializeField] private Animator enemyAnimator;

    Vector2 movement;  // Variable que contiene la direcci�n de movimiento del enemigo (en el plano 2D).

    [Header("Public Variables")]  // T�tulo para agrupar las variables p�blicas que pueden ser modificadas desde otros scripts o el Inspector.
    [Space]

    [Tooltip("Un arreglo de puntos de movimiento por los que el enemigo se desplazar�.")]
    public Transform[] enemyMovementPoints;
    [Tooltip("La velocidad a la que se mover� el enemigo.")]
    public float enemySpeed;
    [Tooltip("La distancia m�nima a la que el enemigo debe llegar a un objetivo para considerarlo \"alcanzado\".")]
    public float detectionRadius = 0.5f;
    //[Tooltip("La cantidad de fuerza con la que el enemigo empuja al jugador cuando lo golpea.")]
    //public float enemyHitStrength;
    [Tooltip("El da�o que el enemigo le causa al jugador al golpearlo.")]
    public float enemyDamage;

    [SerializeField] public float enemyHitStrengthX;
    [SerializeField] public float enemyHitStrengthY;
    [SerializeField] public int hitTime;
    [SerializeField] public int particles;

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
            // Si el enemigo lleg� a su objetivo, se cambia al siguiente objetivo.
            if (actualObjective == enemyMovementPoints[0])
            {
                actualObjective = enemyMovementPoints[1];  // Cambia al siguiente punto en el arreglo.
            }
            else if (actualObjective == enemyMovementPoints[1])
            {
                actualObjective = enemyMovementPoints[0];  // Vuelve al primer punto.
            }
        }

        // Calcula la direcci�n en la que el enemigo debe moverse para alcanzar su objetivo.
        Vector2 direction = (actualObjective.position - transform.position).normalized;

        // Redondea la direcci�n de movimiento a un valor m�s limpio (evita movimientos diagonales).
        int roundedDirection = Mathf.RoundToInt(direction.x);

        // Establece el vector de movimiento en funci�n de la direcci�n (solo horizontal).
        movement = new Vector2(roundedDirection, 0);

        // Rotaci�n del enemigo seg�n la direcci�n en la que se mueve.
        if (roundedDirection < 0)  // Si se mueve a la izquierda.
        {
            transform.localScale = new Vector3(1, 1, 1);  // Invierte la escala en el eje X para que mire hacia la izquierda.
        }
        else if (roundedDirection > 0)  // Si se mueve a la derecha.
        {
            transform.localScale = new Vector3(-1, 1, 1);  // Restaura la escala en el eje X para que mire hacia la derecha.
        }

        // Actualiza el par�metro de movimiento del Animator para que el enemigo tenga la animaci�n de caminar.
        enemyAnimator.SetFloat("Movement", roundedDirection);

        // Mueve al enemigo en la direcci�n calculada con la velocidad establecida y Time.deltaTime para un movimiento suave.
        rb.MovePosition(rb.position + movement * enemySpeed * Time.deltaTime);
    }

    // Este m�todo se llama cuando el enemigo colisiona con otro objeto.
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el enemigo ha colisionado con un objeto que tenga el tag "Player".
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obtiene la referencia al script de comportamiento del jugador para aplicar el da�o y empuje.
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            // Llama al m�todo del jugador para que reciba da�o.
            player.particleCount = particles;
            player.spriteIndex = enemyIndex;

            player.TakeDamage(enemyDamage);
            // Establece el tiempo que el jugador estar� siendo empujado despu�s de la colisi�n.
            player.hitTime = hitTime;
            // Establece la fuerza con la que el enemigo empuja al jugador tanto en el eje X como en el Y.
            player.hitForceX = enemyHitStrengthX;
            player.hitForceY = enemyHitStrengthY;

            // Determina desde qu� direcci�n est� siendo golpeado el jugador para aplicar el empuje en la direcci�n correcta.
            if (collision.transform.position.x <= transform.position.x)
            {
                player.hitFromRight = true;  // El jugador est� siendo golpeado desde la derecha del enemigo.
            }
            else if (collision.transform.position.x > transform.position.x)
            {
                player.hitFromRight = false;  // El jugador est� siendo golpeado desde la izquierda del enemigo.
            }

            // Activa el trigger en el Animator para iniciar la animaci�n de ataque del enemigo.
            enemyAnimator.SetTrigger("Attack");

        }

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wrench"))
        {
            this.gameObject.SetActive(false);
        }

    }


}
